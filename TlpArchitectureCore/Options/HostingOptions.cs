using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlpArchitectureCore.Options;
public class HostingOptions
{
    public int MaxAvailableRam
    {
        get; set;
    }

    public int MaxAvailableDiskSpace
    {
        get; set;
    }

    public string RabbitMqHost
    {
        get; set;
    } = null!;

}
