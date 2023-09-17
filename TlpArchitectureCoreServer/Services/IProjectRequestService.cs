using TlpArchitectureCore.Models;
using TlpArchitectureCoreServer.Models;

namespace TlpArchitectureCoreServer.Services;
public interface IProjectRequestService
{
    Task<IEnumerable<ProjectCreationMessage>> GetAllProjects();
    Task<IEnumerable<ProjectCreationMessage>> GetAllProjectsForUser(User user);
    Task RequestProject(ProjectCreationMessage projectCreationMessage);
}