using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TlpArchitectureCore.Options;
using static MongoDB.Driver.WriteConcern;

namespace TlpArchitectureCore.HostedService;
public class RabbitMqListener : BackgroundService
{
    public const string ProjectCreationQueue = "Project Creation";

    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqListener(IOptions<HostingOptions> hostingOptions)
    {
        var factory = new ConnectionFactory
        {
            HostName = hostingOptions.Value.RabbitMqHost
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(ProjectCreationQueue, false, false, false, null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);

        return Task.CompletedTask;
    }
}
