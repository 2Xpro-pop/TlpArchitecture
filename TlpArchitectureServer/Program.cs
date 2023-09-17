using TlpArchitectureCore.Options;
using TlpArchitectureCore.Services;
using TlpArchitectureCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using TlpArchitectureServer.Services;
using TlpArchitectureProjectEditor.Services;
using TlpArchitectureCore;
using System.Reactive.Linq;

var builder = WebApplication.CreateBuilder(args);

var mongoDbConnectionString = builder.Configuration.GetConnectionString("MongoDb") ??
    throw new NullReferenceException("MongoDb connection string is not set");

builder.Services.AddMongoDb(mongoDbConnectionString);

var hostingOptions = builder.Configuration.GetSection(nameof(HostingOptions));

builder.Services.AddSingleton<IQuotaService, FakerQuotaService>();
builder.Services.AddRabbitMqListener(hostingOptions);
builder.Services.AddProjectEditor();

var app = builder.Build();

app.MapGet("/", ([FromServices] IProjectService projects) => projects.GetAllProjectsAsync());

app.MapGet("/testPostgres", async ([FromServices] IServiceFactory service) =>
{
    var projectInfo = new ProjectInfo()
    {

        Id = Guid.NewGuid(),
        Name = "Test project",
        Domain = "testproject"
    };
    var startInfo = new ServiceStartInfo()
    {
        Project = projectInfo,
        Name = "Testpostgres",
        InternalDomain = "testpostgres",
        ServiceType = "Postgresql",
        DiskUsage = 1024,
        RamUsage = 500,
        IpAddress = "172.23.11.123",
        Properties = new Dictionary<string, object>()
        {

            ["PostgressUser"] = "postgres",
            ["PostgressPassword"] = "postgres"
        }
    };

    var postgresService = service.CreateService(startInfo);

    if (await postgresService.StartAsync())
    {
        app.Logger.LogInformation("Postgres started");

        return postgresService.Ip;
    }

    app.Logger.LogError("Postgres not started: {Error}", postgresService.Error);
    return postgresService.Error;
});

app.Run();