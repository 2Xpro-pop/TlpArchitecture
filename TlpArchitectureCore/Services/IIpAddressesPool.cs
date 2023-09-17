using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TlpArchitectureCore.Models;

namespace TlpArchitectureCore.Services;
public interface IIpAddressesPool
{
    public Task<IpQuota> TakeIp();
    public Task ReturnIp(IpQuota ip);
}
