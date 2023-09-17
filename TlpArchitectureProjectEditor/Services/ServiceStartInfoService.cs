using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace TlpArchitectureProjectEditor.Services;
public class ServiceStartInfoService : IServiceStartInfosService
{
    public const string ServiceStartInfoCollectionName = "service_start_infos";

    private readonly IMongoDatabase _database;

    public ServiceStartInfoService(IMongoDatabase database)
    {
        _database = database;
    }

    public async Task<IEnumerable<ServiceStartInfo>> GetAllServiceStartInfosForProject(Guid projectId)
    {
        var collection = _database.GetCollection<ServiceStartInfo>("service_start_infos");
        return await collection.Find(x => x.ProjectId == projectId).ToListAsync();
    }

    public async Task CreateService(ServiceStartInfo serviceStartInfo)
    {
        var collection = _database.GetCollection<ServiceStartInfo>(ServiceStartInfoCollectionName);
        await collection.InsertOneAsync(serviceStartInfo);
    }

    public async Task<IEnumerable<ServiceStartInfo>> GetAllServices()
    {
        var collection = _database.GetCollection<ServiceStartInfo>(ServiceStartInfoCollectionName);
        return await collection.Find(_ => true).ToListAsync();
    }

    public Task<ServiceStartInfo?> GetServiceById(Guid id)
    {
        var collection = _database.GetCollection<ServiceStartInfo>(ServiceStartInfoCollectionName);
        return collection.Find(x => x.Id == id).FirstOrDefaultAsync()!;
    }

    public Task<ServiceStartInfo?> GetServiceByIp(string ip)
    {
        var collection = _database.GetCollection<ServiceStartInfo>(ServiceStartInfoCollectionName);
        return collection.Find(x => x.IpAddress == ip).FirstOrDefaultAsync()!;
    }

    public async Task<int> ClearDuplicates()
    {
        var collection = _database.GetCollection<ServiceStartInfo>(ServiceStartInfoCollectionName);
        var services = await collection.Find(_ => true).ToListAsync();
        var duplicates = services.GroupBy(x => x.IpAddress).Where(x => x.Count() > 1).SelectMany(x => x.Skip(1)).ToList();
        
        foreach (var duplicate in duplicates)
        {
            await collection.DeleteOneAsync(x => x.Id == duplicate.Id);
        }

        return duplicates.Count;
    }
}
