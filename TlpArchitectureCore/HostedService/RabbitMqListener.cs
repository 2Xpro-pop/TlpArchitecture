using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TlpArchitectureCore.Models;
using TlpArchitectureCore.Options;
using TlpArchitectureCore.Services;

namespace TlpArchitectureCore.HostedService;
public class RabbitMqListener : BackgroundService
{
    public const string ProjectCreationQueue = "Project Creation";
    public const string ProjectCreationResultsQueue = "Project Creation Results";
    public const string ProcessingMessageError = "Error while processing message of {Queue}, message DeliveryTag {DeliveryTag}";

    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IProjectCreateRequestListener _projectCreateRequestListener;
    private readonly ILogger _logger;

    public RabbitMqListener(IConnection connection, IProjectCreateRequestListener projectCreateRequestListener, ILogger<RabbitMqListener> logger)
    {
        _connection = connection;
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(ProjectCreationQueue, false, false, false, null);
        _projectCreateRequestListener = projectCreateRequestListener;
        _logger = logger;
        
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            try
            {
                var projectCreationMessage = GetFromJsonBody<ProjectCreationMessage>(ea);

                var result = await _projectCreateRequestListener.HandleAsync(projectCreationMessage);

                if (result)
                {
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                }
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, ProcessingMessageError, ProjectCreationQueue, ea.DeliveryTag);
                _channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        _channel.BasicConsume(ProjectCreationQueue, false, consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        base.Dispose();
        _channel.Dispose();
        _connection.Dispose();
        _projectCreateRequestListener.Dispose();
    }

    private static T GetFromJsonBody<T>(BasicDeliverEventArgs args)
    {
        var body = args.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        return JsonSerializer.Deserialize<T>(message)!;
    }
}
