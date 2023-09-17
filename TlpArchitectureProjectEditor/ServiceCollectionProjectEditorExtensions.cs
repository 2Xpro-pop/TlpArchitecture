using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TlpArchitectureCore.Services;
using TlpArchitectureProjectEditor.Services;

namespace TlpArchitectureProjectEditor.Extensions;
public static class ServiceCollectionProjectEditorExtensions
{
    public static IServiceCollection AddProjectEditor(this IServiceCollection services)
    {
        services.AddScoped<IServiceFactory, ServiceFactory>();
        services.AddScoped<ILinkService, LinkService>();
        services.AddScoped<IIpAddressesPool, IpAddressesPool>();


        return services;
    }

}
