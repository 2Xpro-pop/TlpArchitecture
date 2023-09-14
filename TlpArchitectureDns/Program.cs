
using System.Net;
using DNS.Client.RequestResolver;
using DNS.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TlpArchitecture;
using TlpArchitecture.Services;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddSingleton<IRequestResolver, RequestResolver>();

builder.Services.AddHostedService<HostedDnsServer>();

var app = builder.Build();

app.Run();