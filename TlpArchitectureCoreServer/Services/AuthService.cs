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

    public AuthService(IMongoDatabase database, IPasswordHasher passwordHasher)
    {
        _database = database;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> ValidateUser(string username, string password)
    {
        var user = await GetUser(username);

        if (user == null)
        {
            return false;
        }

        return _passwordHasher.Verify(password, user.PasswordHash);
    }

    public async Task<User?> GetUser(string username)
    {
        var users = _database.GetCollection<User>("users");
        var user = await users.Find(u => u.Username == username).FirstOrDefaultAsync();

        return user;
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
