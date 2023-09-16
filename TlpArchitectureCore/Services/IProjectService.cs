using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlpArchitectureCore.Services;
public interface IProjectService
{
    public Task<ProjectInfo?> GetProjectAsync(Guid id);
    public Task<IEnumerable<ProjectInfo>> GetAllProjectsAsync();
    public Task<bool> CreateProjectAsync(ProjectInfo project);
    public Task<bool> DeleteProjectAsync(Guid id);
    public Task<bool> UpdateProjectAsync(ProjectInfo project);
    public Task<bool> IsUniqueDomain(ProjectInfo project);
    public Task<bool> IsUniqueDomain(string domain);
}
