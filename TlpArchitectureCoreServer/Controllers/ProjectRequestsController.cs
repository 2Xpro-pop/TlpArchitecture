using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TlpArchitectureCore.Models;
using TlpArchitectureCoreServer.Services;

namespace TlpArchitectureCoreServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectRequestsController : ControllerBase
{
    private readonly IProjectRequestService _projectRequestService;
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public ProjectRequestsController(IProjectRequestService projectRequestService, IAuthService authService, IUserService userService)
    {
        _projectRequestService = projectRequestService;
        _authService = authService;
        _userService = userService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IEnumerable<ProjectCreationMessage>> GetAllProjectRequests()
    {
        var user = await _userService.GetUser(User.Identity!.Name!);

        return user == null ? Array.Empty<ProjectCreationMessage>() : await _projectRequestService.GetAllProjectsForUser(user);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> RequestProject([FromBody] ProjectCreationMessage projectCreationMessage)
    {
        var user = await _userService.GetUser(User.Identity!.Name!);

        if (user == null)
        {
            return Unauthorized();
        }

        projectCreationMessage.Users = projectCreationMessage.Users.Prepend(user!.Id).ToArray();

        await _projectRequestService.RequestProject(projectCreationMessage);

        return Ok();
    }
}
