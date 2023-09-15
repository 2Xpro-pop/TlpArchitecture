using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlpArchitectureCore.Models;
public readonly struct Quota
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

    public Quota(int ram, int disk)
    {
        Ram = ram;
        Disk = disk;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
