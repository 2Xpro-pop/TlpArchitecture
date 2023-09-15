using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlpArchitectureCore.Services;
public interface IProjectService
{
    public Task<Project?> GetProjectAsync(Guid id);
    public Task<IEnumerable<Project>> GetAllProjectsAsync();
    public Task<bool> CreateProjectAsync(Project project);
    public Task<bool> DeleteProjectAsync(Guid id);
    public Task<bool> UpdateProjectAsync(Project project);
    public Task<bool> IsUniqueDomain(Project project);
    public Task<bool> IsUniqueDomain(string domain);
}
