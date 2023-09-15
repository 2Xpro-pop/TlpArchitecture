using TlpArchitectureCore.Options;
using TlpArchitectureCore.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<HostingOptions>(nameof(HostingOptions), builder.Configuration);

builder.Services.AddSingleton<HostingPool>();

var app = builder.Build();

app.Run();