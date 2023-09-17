using System.Text;
using System.Text.Json;

namespace TlpArchitectureCore.Models;

public class Message
{

    public byte[] ToJsonBody() => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this));
}