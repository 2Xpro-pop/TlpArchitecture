using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlpArchitectureProjectEditor.Services;
public interface IServiceStartInfosService
{
    public Task CreateService(ServiceStartInfo serviceStartInfo);
    public Task<IEnumerable<ServiceStartInfo>> GetAllServices();
    Task<IEnumerable<ServiceStartInfo>> GetAllServiceStartInfosForProject(Guid projectId);
    public Task<ServiceStartInfo?> GetServiceById(Guid id);
    public Task<ServiceStartInfo?> GetServiceByIp(string ip);
}
