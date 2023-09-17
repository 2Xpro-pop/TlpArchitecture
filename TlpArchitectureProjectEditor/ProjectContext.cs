using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using TlpArchitectureCore;
using TlpArchitectureProjectEditor.Services;

namespace TlpArchitectureProjectEditor;
public sealed class ProjectContext
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
    } = null!;

    public Guid ProjectId
    {
        get; set;
    }

    public int CurrentDiskUsage => ServiceStartInfos.Sum(x => x.DiskUsage);

    public int CurrentMemoryUsage => ServiceStartInfos.Sum(x => x.RamUsage);

    public List<ServiceStartInfo> ServiceStartInfos
    {
        get; set;
    } = new();


}
