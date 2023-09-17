using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using TlpArchitectureProjectEditor.Models;

namespace TlpArchitectureProjectEditor.Services;
public class LinkService : ILinkService
{
    private const string ServicesLink = "services_links";
    private readonly IMongoDatabase _database;
    private readonly IServiceStartInfosService _serviceStartInfosService;

    public LinkService(IMongoDatabase database, IServiceStartInfosService serviceStartInfosService)
    {
        _database = database;
        _serviceStartInfosService = serviceStartInfosService;
    }

    public async Task<IEnumerable<ServicesLink>> GetAllLinksForProject(Guid projectId)
    {
        var serviceStartInfos = await _serviceStartInfosService.GetAllServiceStartInfosForProject(projectId);
        var collection = _database.GetCollection<ServicesLink>(ServicesLink);

        var links = new List<ServicesLink>();

        foreach (var serviceStartInfo in serviceStartInfos)
        {
            var linksFrom = await collection.Find(x => x.FirstServiceStartInfoId == serviceStartInfo.Id).ToListAsync();
            var linksTo = await collection.Find(x => x.SecondServiceStartInfoId == serviceStartInfo.Id).ToListAsync();
            links.AddRange(linksFrom);
            links.AddRange(linksTo);
        }

        return links;
    }
    public async Task<IEnumerable<ServicesLink>> GetAllLinks(Guid serviceStartInfoId)
    {
        var collection = _database.GetCollection<ServicesLink>(ServicesLink);
        return await collection.Find(x => x.FirstServiceStartInfoId == serviceStartInfoId).ToListAsync();
    }
}
