using MongoDB.Driver;
using TlpArchitectureCore.Models;

namespace TlpArchitectureCoreServer.Services;

public class UserService : IUserService
{
    private readonly IMongoDatabase _database;

    public UserService(IMongoDatabase database)
    {
        _database = database;
    }

    public async Task<User?> GetUser(string username)
    {
        var users = _database.GetCollection<User>("users");
        var user = await users.Find(u => u.Username == username).FirstOrDefaultAsync();

        return user;
    }

    public async Task<User> GetUserById(Guid id)
    {
        var users = _database.GetCollection<User>("users");
        var user = await users.Find(u => u.Id == id).FirstOrDefaultAsync();

        return user;
    }

    public async Task<User> CreateUser(string username, string password)
    {
        var users = _database.GetCollection<User>("users");

        var user = new User
        {
            Username = username,
            PasswordHash = password
        };

        await users.InsertOneAsync(user);

        return user;
    }

    public async Task<IEnumerable<User>> TakePage(int page, int pageSize)
    {

        var users = _database.GetCollection<User>("users");
        var user = await users.Find("{}").Skip(page * pageSize).Limit(pageSize).ToListAsync();

        return user;
    }

    public async Task<IEnumerable<User>> TakePage(string name, int page, int pageSize)
    {
        var users = _database.GetCollection<User>("users");
        var user = await users.Find(u => u.Username.Contains(name)).Skip(page * pageSize).Limit(pageSize).ToListAsync();

        return user;
    }
}
