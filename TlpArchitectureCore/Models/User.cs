using MongoDB.Bson.Serialization.Attributes;

namespace TlpArchitectureCore.Models;

public class User
{
    [BsonId]
    public int Id
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
