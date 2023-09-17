using MongoDB.Driver;
using TlpArchitectureCore.Models;

namespace TlpArchitectureCoreServer.Services;

/// <summary>
/// Authentication service, which used mongoDb to store users
/// </summary>
public class AuthService : IAuthService
{
    private readonly IMongoDatabase _database;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserService _userService;

    public AuthService(IMongoDatabase database, IPasswordHasher passwordHasher, IUserService userService)
    {
        _database = database;
        _passwordHasher = passwordHasher;
        _userService = userService;
    }

    public async Task<bool> ValidateUser(string username, string password)
    {
        var user = await _userService.GetUser(username);

        if (user == null)
        {
            return false;
        }

        return _passwordHasher.Verify(password, user.PasswordHash);
    }


    public async Task<User> CreateUser(string username, string password)
    {

        var users = _database.GetCollection<User>("users");

        var user = new User
        {
            Username = username,
            PasswordHash = _passwordHasher.Hash(password)
        };

        await users.InsertOneAsync(user);

        return user;
    }
}
