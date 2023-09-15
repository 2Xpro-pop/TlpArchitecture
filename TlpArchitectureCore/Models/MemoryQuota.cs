using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlpArchitectureCore.Models;

/// <summary>
/// It's quota for memory usage only.
/// When project done this struct will return to pool.
/// </summary>
public readonly struct MemoryQuota
{
    public Guid Id
    {
        get;
    } = Guid.NewGuid();
    public int Ram
    {
        get;
    }

    public int Disk
    {
        get;
    }

    public MemoryQuota(int ram, int disk)
    {
        Ram = ram;
        Disk = disk;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
