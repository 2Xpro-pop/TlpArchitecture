using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlpArchitectureCore.Services;
public interface IIpAddressesPool
{
    public Task<string> TakeIp();
}
