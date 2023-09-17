using TlpArchitectureCoreServer.Models;

namespace TlpArchitectureCoreServer.Services;
public interface IAuthService
{
    Task<User> CreateUser(string username, string password);
    Task<User?> GetUser(string username);
    Task<bool> ValidateUser(string username, string password);
}