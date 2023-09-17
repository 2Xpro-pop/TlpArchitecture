using TlpArchitectureCore.Models;

namespace TlpArchitectureCoreServer.Services;
public interface IAuthService
{
    Task<User> CreateUser(string username, string password);
    Task<bool> ValidateUser(string username, string password);
}