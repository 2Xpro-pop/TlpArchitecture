using Microsoft.AspNetCore.Mvc;
using TlpArchitectureCore.Models;
using TlpArchitectureCoreServer.Services;

namespace TlpArchitectureCoreServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }


    [HttpGet("/page")]
    public async Task<IActionResult> TakePage(int page, int pageSize)
    {
        return Ok(await _userService.TakePage(page, pageSize));
    }

    [HttpGet("page/{name}")]
    public async Task<IActionResult> TakePage(string name, int page, int pageSize)
    {
        return Ok(await _userService.TakePage(name, page, pageSize));
    }

}
