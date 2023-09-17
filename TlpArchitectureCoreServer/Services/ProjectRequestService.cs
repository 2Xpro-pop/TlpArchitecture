using MongoDB.Driver;
using RabbitMQ.Client;
using TlpArchitectureCore.HostedService;
using TlpArchitectureCore.Models;
using TlpArchitectureCoreServer.Models;

namespace TlpArchitectureCoreServer.Services;


public class ProjectRequestService : IProjectRequestService
{
    public const string ProjectCreationResults = "project_creation_results";

    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IMongoDatabase _mongoDb;

    public ProjectRequestService(IConnection connection, IMongoDatabase mongoDb)
    {
        _connection = connection;
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(RabbitMqListener.ProjectCreationQueue, false, false, false, null);
        _mongoDb = mongoDb;
    }

    /// <summary>
    /// Send a message to the rabbitmq to create a new project.
    /// </summary>
    public Task RequestProject(ProjectCreationMessage projectCreationMessage)
    {
        _channel.BasicPublish("", RabbitMqListener.ProjectCreationQueue, null, projectCreationMessage.ToJsonBody());
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<ProjectCreationMessage>> GetAllProjects()
    {
        var collection = _mongoDb.GetCollection<ProjectCreationMessage>(ProjectCreationResults);
        return await collection.Find("{}").ToListAsync();
    }

    public async Task<IEnumerable<ProjectCreationMessage>> GetAllProjectsForUser(User user)
    {

        var collection = _mongoDb.GetCollection<ProjectCreationMessage>(ProjectCreationResults);
        return await collection.Find(x => x.Users[0] == user.Id).ToListAsync();
    }
}
