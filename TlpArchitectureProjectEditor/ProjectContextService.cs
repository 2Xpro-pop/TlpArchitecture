using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Util;
using DynamicData;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using RabbitMQ.Client;
using TlpArchitectureCore.Models;
using TlpArchitectureProjectEditor.Services;
using TlpArchitectureProjectEditor.Services.DefaultServices;

namespace TlpArchitectureProjectEditor;
public class ProjectContextService
{
    private readonly IServiceFactory _serviceFactory;
    private readonly ILogger<ProjectContextService> _logger;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly SourceCache<ProjectContext, Guid> _projectsCache = new(x => x.ProjectId);
    private readonly SourceCache<ServiceStartInfo, Guid> _servicesCache = new(x => x.Id);

    public ProjectContextService(IServiceFactory serviceFactory, ILogger<ProjectContextService> logger, IConnection connection)
    {
        _serviceFactory = serviceFactory;
        _logger = logger;
        _connection = connection;
        _channel = connection.CreateModel();
        _channel.QueueDeclare("Service Activate Results", false, false, false, null);
        _channel.QueueDeclare("Project Activate Results", false, false, false, null);
    }

    public IObservable<IChangeSet<ProjectContext, Guid>> Projects => _projectsCache.Connect();
    public IObservable<IChangeSet<ServiceStartInfo, Guid>> Services => _servicesCache.Connect();

    public async Task<bool> TryActivate(ProjectContext projectContext)
    {
        if (_projectsCache.Lookup(projectContext.ProjectId).HasValue)
        {
            return false;
        }

        foreach (var serviceStartInfo in projectContext.ServiceStartInfos)
        {
            try
            {
                var service = _serviceFactory.CreateService(serviceStartInfo);

                if (service is null)
                {
                    _logger.LogError("Service {ServiceId} not found", serviceStartInfo.Id);
                    return false;
                }

                if (await service.StartAsync())
                {
                    _servicesCache.AddOrUpdate(serviceStartInfo);
                }
                else
                {
                    _logger.LogError(
                        "Error while starting service {ServiceId}. Error: {Error}",
                        serviceStartInfo.Id,
                        service.Error);

                    var message = new ServiceActivationResultMessage()
                    {
                        SerivceId = serviceStartInfo.Id,
                        IsSuccess = false,
                        Error = service.Error
                    }.ToJsonBody();
                    
                    _channel.BasicPublish("", "Service Activate Results", null, message);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while creating service {ServiceId}", serviceStartInfo.Id);
            }
        }

        return true;
    }
}
