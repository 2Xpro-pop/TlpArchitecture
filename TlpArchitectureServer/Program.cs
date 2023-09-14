using TlpArchitectureCore.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<HostingOptions>(nameof(HostingOptions), builder.Configuration);

var app = builder.Build();

app.Run();