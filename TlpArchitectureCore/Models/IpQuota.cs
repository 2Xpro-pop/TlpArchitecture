using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace TlpArchitectureCore.Models;
public readonly struct IpQuota
{
    public static readonly IpQuota Empty = new(string.Empty);

    [BsonConstructor]
    public IpQuota(string ip)
    {
        Ip = ip;
    }

    public string Ip
    {
        get;
    }
}
