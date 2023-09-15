using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TlpArchitectureCore.Models;
using TlpArchitectureCore.Options;

namespace TlpArchitectureCore.Services;
public class HostingPool
{
    private readonly List<MemoryQuota> _rented = new();

    public HostingPool(IOptions<HostingOptions> hostingOptions)
    {
        AvailableRam = hostingOptions.Value.MaxAvailableRam;
        AvailableDisk = hostingOptions.Value.MaxAvailableDiskSpace;

        MemoryQuota.DefaultPool = this;
    }

    public int AvailableRam
    {
        get; private set;
    }

    public int AvailableDisk
    {
        get; private set;
    }

    public bool IsAvailable(int ram, int disk)
    {
        return AvailableRam >= ram && AvailableDisk >= disk;
    }

    public MemoryQuota? TryRent(int ram, int disk)
    {
        if (IsAvailable(ram, disk))
        {
            AvailableRam -= ram;
            AvailableDisk -= disk;

            var quota = new MemoryQuota(ram, disk);

            _rented.Add(quota);

            return quota;
        }
        return null;
    }

    public void Return(MemoryQuota quota)
    {
        if (_rented.Remove(quota))
        {
            AvailableRam += quota.Ram;
            AvailableDisk += quota.Disk;
        }
    }

}
