using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using RabbitMQ.Client;
using TlpArchitectureCore.Services;

namespace TlpArchitectureProjectEditor.HostedService;
public class ProjectStarter : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger _logger;

    public ProjectStarter(IServiceScopeFactory serviceScopeFactory, ILogger<ProjectStarter> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();

        var projectService = scope.ServiceProvider.GetRequiredService<IProjectService>();

        var projects = await projectService.GetAllProjectsAsync();
        var projectContextService = scope.ServiceProvider.GetRequiredService<ProjectContextService>();

        var connection = scope.ServiceProvider.GetRequiredService<IConnection>();
        var channel = connection.CreateModel();
        channel.QueueDeclare("Project Activate Results", false, false, false, null);

        foreach (var project in projects)
        {
            try
            {
                var projectContext = new ProjectContext()
                {
                    Id = Guid.NewGuid(),
                    Project = project,
                    ProjectId = project.Id,
                };

                if (!projectContextService.TryActivate(projectContext))
                {
                    channel.BasicPublish("", "Project Activate Results", null, Encoding.UTF8.GetBytes($"Project {project.Id} already activated"));
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while activating project {ProjectId}", project.Id);
                channel.BasicPublish("", "Project Activate Results", null, Encoding.UTF8.GetBytes($"Error while activating project {project.Id}"));
            }
            
            
        }
    }
}
