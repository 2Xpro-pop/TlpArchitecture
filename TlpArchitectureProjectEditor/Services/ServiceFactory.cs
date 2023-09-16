using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace TlpArchitectureProjectEditor.Services;
public class ServiceFactory : IServiceFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IService? CreateService(ServiceStartInfo serviceStartInfo)
    {
        var serviceTypeString = $"TlpArchitectureProjectEditor.Services.DefaultServices.{serviceStartInfo.ServiceType}Service";
        var serviceType = Type.GetType(serviceTypeString);

        if (serviceType == null)
        {
            return null;
        }

        var service = (IService?)ActivatorUtilities.CreateInstance(_serviceProvider, serviceType, serviceStartInfo);

        return service;
    }
}
