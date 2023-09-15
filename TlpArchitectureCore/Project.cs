using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using TlpArchitectureCore.Docker;
using TlpArchitectureCore.Models;

namespace TlpArchitectureCore;
public class Project
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

    public ObservableCollection<Container> Containers
    {
        get;
    } = new();
}
