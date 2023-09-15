using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using TlpArchitectureCore.HostedService;
using TlpArchitectureCore.Options;
using TlpArchitectureCore.Services;

namespace TlpArchitectureCore.Extensions;
public static class ServiceCollectionRabbitMqListenerExtensions
{
    public static IServiceCollection AddRabbitMqListener(this IServiceCollection services, IConfiguration? hostingOptions = null)
    {
        if (hostingOptions != null)
        {
            services.Configure<HostingOptions>(hostingOptions);
        }
        else
        {
            services.Configure<HostingOptions>(options =>
            {
                options.RabbitMqHost = "localhost";
            });
        }
        

        services.TryAddSingleton(services =>
        {
            var hostingOptions = services.GetRequiredService<IOptions<HostingOptions>>();

            var factory = new ConnectionFactory
            {
                HostName = hostingOptions.Value.RabbitMqHost
            };

            return factory.CreateConnection();
        });

        services.AddHostedService<RabbitMqListener>();

        services.TryAddSingleton<HostingPool>();
        services.TryAddSingleton<ProjectCollection>();
        services.TryAddSingleton<IProjectCreateRequestListener, ProjectCreationRequestListener>();
        services.TryAddScoped<IProjectService, ProjectService>();

        return services;
    }
}
