using MongoDB.Bson.Serialization.Attributes;

namespace TlpArchitectureCoreServer.Models;

public class User
{
    [BsonId]
    public Guid Id
    {
        get; set;
    }

    public string Username
    {
        get; set;
    } = null!;

    public string PasswordHash
    {
        get; set;
    } = null!;
}
