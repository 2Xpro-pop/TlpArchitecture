using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TlpArchitectureCore.Models;
using TlpArchitectureCoreServer.Options;

namespace TlpArchitectureCoreServer.Services;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly AuthOptions _authOptions;

    public JwtTokenGenerator(IOptions<AuthOptions> authOptions)
    {
        _authOptions = authOptions.Value;
    }

    public string GenerateTokenForUser(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, "user"),

        };

        var claimsIdentity = new ClaimsIdentity(
            claims,
            "Token",
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);

        var now = DateTime.UtcNow;

        var jwt = new JwtSecurityToken(
            issuer: _authOptions.Issuer,
            audience: _authOptions.Audience,
            notBefore: now,
            claims: claimsIdentity.Claims,
            expires: now.Add(TimeSpan.FromHours(8)),
            signingCredentials: new SigningCredentials(_authOptions.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256));

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }
}
