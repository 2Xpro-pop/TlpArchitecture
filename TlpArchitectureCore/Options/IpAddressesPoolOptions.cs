using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlpArchitectureCore.Options;
public class IpAddressesPoolOptions
{
    public string IpAddress
    {
        get; set;
    }

    public int SubnetMask
    {
        get; set;
    }
}
