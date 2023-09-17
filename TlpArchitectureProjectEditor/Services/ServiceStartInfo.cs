using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using TlpArchitectureCore;

namespace TlpArchitectureProjectEditor.Services;
public class ServiceStartInfo
{
    [BsonId]
    public Guid Id
    {
        get; set;
    }

    [BsonIgnore]
    public ProjectInfo Project
    {
        get; set;
    }

    public Guid ProjectId
    {
        get; set;
    }

    public string IpAddress
    {
        get; set;
    }

    public string InternalDomain
    {
        get; set;
    }

    /// <summary>
    /// in megabytes
    /// </summary>
    public int DiskUsage
    {
        get; set;
    }

    /// <summary>
    /// in megabytes
    /// </summary>
    public int RamUsage
    {
        get; set;
    }

    public string Name
    {
        get; set;
    }

    [BsonExtraElements]
    public IDictionary<string, object> Properties
    {
        get; set;
    } = null!;

    public string ServiceType
    {
        get; set;
    }
}
