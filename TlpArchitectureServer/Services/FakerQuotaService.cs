using TlpArchitectureCore.Models;
using TlpArchitectureCore.Services;

namespace TlpArchitectureServer.Services;

public class FakerQuotaService : IQuotaService
{
    public Task<Quota?> FindByIdAsync(int id) => Task.FromResult<Quota?>(new Quota()
    {
        Id = id,
        DiskUsage = 1000,
        RamUsage = 500
    });
}
