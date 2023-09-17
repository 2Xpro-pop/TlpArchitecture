using TlpArchitectureCore.Options;
using TlpArchitectureCore.Services;
using TlpArchitectureCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using TlpArchitectureProjectsServer.Services;
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


app.Run();