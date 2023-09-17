using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TlpArchitectureCore.Models;
public sealed class ServiceActivationResultMessage: Message
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Error
    {
        get; set;
    }
    public bool IsSuccess
    {
        get; set;
    }
    public Guid SerivceId
    {
        get; set;
    }
}
