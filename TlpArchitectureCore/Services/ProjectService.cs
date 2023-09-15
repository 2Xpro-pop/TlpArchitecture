using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace TlpArchitectureCore.Services;
public class ProjectService : IProjectService
{
    private readonly IMongoDatabase _mongoDb;

    public ProjectService(IMongoDatabase mongoClient)
    {
        _mongoDb = mongoClient;
    }

    public async Task<Project?> GetProjectAsync(Guid id)
    {
        var collection = _mongoDb.GetCollection<Project>("projects");

        var project = await collection.Find(p => p.Id == id).FirstOrDefaultAsync();

        return project;
    }

    public async Task<IEnumerable<Project>> GetAllProjectsAsync()
    {
        var collection = _mongoDb.GetCollection<Project>("projects");

        var projects = await collection.Find("{}").ToListAsync();

        return projects;
    }

    public async Task<bool> CreateProjectAsync(Project project)
    {
        var collection = _mongoDb.GetCollection<Project>("projects");

        await collection.InsertOneAsync(project);

        return true;
    }

    public async Task<bool> DeleteProjectAsync(Guid id)
    {
        var collection = _mongoDb.GetCollection<Project>("projects");

        var result = await collection.DeleteOneAsync(p => p.Id == id);

        return result.DeletedCount > 0;
    }

    public async Task<bool> UpdateProjectAsync(Project project)
    {
        if (!await IsUniqueDomain(project))
        {
            return false;
        }
        var collection = _mongoDb.GetCollection<Project>("projects");

        var result = await collection.ReplaceOneAsync(p => p.Id == project.Id, project);

        return result.ModifiedCount > 0;
    }

    public Task<bool> IsUniqueDomain(Project project)
    {
        return IsUniqueDomain(project.Domain);
    }

    public async Task<bool> IsUniqueDomain(string domain)
    {
        var collection = _mongoDb.GetCollection<Project>("projects");

        var result = await collection.Find(p => p.Domain == domain).FirstOrDefaultAsync();

        return result == null;
    }
}
