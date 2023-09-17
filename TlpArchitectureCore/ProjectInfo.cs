using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TlpArchitectureCore.Docker;
using TlpArchitectureCore.Models;

namespace TlpArchitectureCore;

[BsonIgnoreExtraElements]
public class ProjectInfo
{
    [BsonId]
    public Guid Id
    {
        get; set;
    }
    /// <summary>
    /// The unique name of the project
    /// </summary>
    public string Name
    {
        get;
        set;
    } = null!;

    [BsonIgnore]
    public bool IsStarted
    {
        get;
        set;
    }

    [BsonIgnore]
    public MemoryQuota? MemoryQuota
    {
        get;
        set;
    }

    public Quota Quota
    {
        get;
        set;
    } = null!;

    public bool IsPublicDomain
    {
        get;
        set;
    }

    public string Domain
    {
        get;
        set;
    } = null!;


}
