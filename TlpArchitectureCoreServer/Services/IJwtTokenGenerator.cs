using TlpArchitectureCoreServer.Models;

namespace TlpArchitectureCoreServer.Services;
public interface IJwtTokenGenerator
{
    string GenerateTokenForUser(User user);
}