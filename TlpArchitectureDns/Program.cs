
using System.Net;
using DNS.Client.RequestResolver;
using DNS.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TlpArchitecture;
using TlpArchitecture.Services;
using TlpArchitectureCore.Extensions;
using TlpArchitectureCore.Options;

var builder = Host.CreateApplicationBuilder();

var mongoDbConnectionString = builder.Configuration.GetConnectionString("MongoDb") ??
    throw new NullReferenceException("MongoDb connection string is not set");

var hostingOptionsSection = builder.Configuration.GetSection(nameof(HostingOptions));

builder.Services.AddMongoDb(mongoDbConnectionString);

builder.Services.Configure<HostingOptions>(hostingOptionsSection);

builder.Services.AddSingleton<IRequestResolver, RequestResolver>();

builder.Services.AddHostedService<HostedDnsServer>();

var app = builder.Build();

app.Run();