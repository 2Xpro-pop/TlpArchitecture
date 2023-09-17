using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TlpArchitectureCore.Services;
using TlpArchitectureCoreServer.Options;
using TlpArchitectureCoreServer.Services;
using TlpArchitectureCoreServer.ViewModels;

namespace TlpArchitectureCoreServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserService _userService;

    public AuthenticationController(IAuthService authService, IJwtTokenGenerator jwtTokenGenerator, IUserService userService)
    {
        _authService = authService;
        _jwtTokenGenerator = jwtTokenGenerator;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
    {
        if (!await _authService.ValidateUser(request.Username, request.Password))
        {
            return new UnauthorizedResult();
        }

        var user = await _userService.GetUser(request.Username);

        return Ok(_jwtTokenGenerator.GenerateTokenForUser(user));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthRequest request)
    {
        await _authService.CreateUser(request.Username, request.Password);

        return Ok();
    }
}
