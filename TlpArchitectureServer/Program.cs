using TlpArchitectureCore.Options;
using TlpArchitectureCore.Services;
using TlpArchitectureCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using TlpArchitectureServer.Services;

var builder = WebApplication.CreateBuilder(args);

var hostingOptions = builder.Configuration.GetSection(nameof(HostingOptions));

builder.Services.AddSingleton<IQuotaService, FakerQuotaService>();
builder.Services.AddRabbitMqListener(hostingOptions);

var app = builder.Build();

app.MapGet("/", ([FromServices] ProjectCollection projects) => projects);

app.Run();