using TlpArchitectureCore.Models;

namespace TlpArchitectureCoreServer.Services;
public interface IUserService
{
    Task<User> CreateUser(string username, string password);
    Task<User?> GetUser(string username);
    Task<User> GetUserById(Guid id);
    Task<IEnumerable<User>> TakePage(int page, int pageSize);
    Task<IEnumerable<User>> TakePage(string name, int page, int pageSize);
}