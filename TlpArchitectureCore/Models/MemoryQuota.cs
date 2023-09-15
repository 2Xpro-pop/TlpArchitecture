using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TlpArchitectureCore.Services;

namespace TlpArchitectureCore.Models;

/// <summary>
/// It's quota for memory usage only.
/// When project done this struct will return to pool.
/// </summary>
public readonly struct MemoryQuota
{
    public static HostingPool DefaultPool
    {
        get; set;
    }
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

    internal MemoryQuota(int ram, int disk)
    {
        Ram = ram;
        Disk = disk;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
