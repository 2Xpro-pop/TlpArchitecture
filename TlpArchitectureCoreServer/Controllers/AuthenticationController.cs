using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TlpArchitectureCoreServer.Models;
using TlpArchitectureCoreServer.Options;
using TlpArchitectureCoreServer.Services;
using TlpArchitectureCoreServer.ViewModels;

namespace TlpArchitectureCoreServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly AuthOptions _authOptions;
    private readonly IAuthService _authService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthenticationController(IOptions<AuthOptions> authOptions, IAuthService authService, IJwtTokenGenerator jwtTokenGenerator)
    {
        _authOptions = authOptions.Value;
        _authService = authService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
    {
        if (!await _authService.ValidateUser(request.Username, request.Password))
        {
            return new UnauthorizedResult();
        }

        var user = await _authService.GetUser(request.Username)!;

        return Ok(_jwtTokenGenerator.GenerateTokenForUser(user));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthRequest request)
    {

        await _authService.CreateUser(request.Username, request.Password);

        return Ok();
    }
}
