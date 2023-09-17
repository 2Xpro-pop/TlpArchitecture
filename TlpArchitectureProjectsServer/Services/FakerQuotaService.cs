using TlpArchitectureCore.Models;
using TlpArchitectureCore.Services;

namespace TlpArchitectureProjectsServer.Services;

public class FakerQuotaService : IQuotaService
{
    public Task<Quota?> FindByIdAsync(int id) => Task.FromResult<Quota?>(new Quota()
    {
        Id = id,
        DiskUsage = 1000,
        RamUsage = 500
    });
}
