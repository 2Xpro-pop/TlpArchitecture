using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using TlpArchitectureCore.HostedService;
using TlpArchitectureCore.Models;

namespace TlpArchitectureCore.Services;
public sealed class ProjectCreationRequestListener : IProjectCreateRequestListener, IDisposable
{
    private readonly IQuotaService _quotaService;
    private readonly HostingPool _hostingPool;
    private readonly ILogger _logger;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ProjectCreationRequestListener(IQuotaService quotaService, HostingPool hostingPool, ILogger<ProjectCreationRequestListener> logger, IConnection connection, IServiceScopeFactory serviceScopeFactory)
    {
        _quotaService = quotaService;
        _hostingPool = hostingPool;
        _logger = logger;
        _connection = connection;
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(RabbitMqListener.ProjectCreationResultsQueue, false, false, false, null);
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<bool> HandleAsync(ProjectCreationMessage projectCreationMessage)
    {
        ProjectCreationResultMessage message;
        var quota = await _quotaService.FindByIdAsync(projectCreationMessage.QuotaId);

        if (quota == null)
        {
            _logger.LogError("Quota with id {QuotaId} not found", projectCreationMessage.QuotaId);

            message = new ProjectCreationResultMessage()
            {
                Id = projectCreationMessage.Id,
                IsSuccess = false,
                Message = $"Quota with id {projectCreationMessage.QuotaId} not found"
            };

            _channel.BasicPublish(exchange: string.Empty,
                                  RabbitMqListener.ProjectCreationResultsQueue,
                                  basicProperties: null,
                                  message.ToJsonBody());

            return false;
        }

        var memoryQuota = _hostingPool.TryRent(quota.RamUsage, quota.DiskUsage);

        if (memoryQuota == null)
        {
            _logger.LogError("Not enough resources to create project {ProjectName}", projectCreationMessage.Name);

            message = new ProjectCreationResultMessage()
            {
                Id = projectCreationMessage.Id,
                IsSuccess = false,
                Message = $"Not enough resources to create project {projectCreationMessage.Name}"
            };

            _channel.BasicPublish(exchange: string.Empty,
                                  RabbitMqListener.ProjectCreationResultsQueue,
                                  basicProperties: null,
                                  message.ToJsonBody());

            return false;
        }

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = projectCreationMessage.Name,
            MemoryQuota = memoryQuota
        };

        using var scope = _serviceScopeFactory.CreateScope();
        var projectCollection = scope.ServiceProvider.GetRequiredService<IProjectService>();

        await projectCollection.CreateProjectAsync(project);

        message = new ProjectCreationResultMessage()
        {
            Id = projectCreationMessage.Id,
            IsSuccess = true,
            ProjectId = project.Id
        };

        _channel.BasicPublish(exchange: string.Empty,
                              RabbitMqListener.ProjectCreationResultsQueue,
                              basicProperties: null,
                              message.ToJsonBody());

        _logger.LogInformation("Project {ProjectName} created", projectCreationMessage.Name);

        return true;


    }

    public void Dispose()
    {
        _channel.Dispose();
    }
}