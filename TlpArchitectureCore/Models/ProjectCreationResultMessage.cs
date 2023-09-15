using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TlpArchitectureCore.Models;
public class ProjectCreationResultMessage
{
    public int Id
    {
        get;
        set;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Guid ProjectId
    {

        get;
        set;
    }
    public bool IsSuccess
    {

        get;
        set;
    }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message
    {
        get;
        set;
    }

    public byte[] ToJsonBody() => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this));
}
