using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TlpArchitectureCore.Models;
using TlpArchitectureCore.Options;

namespace TlpArchitectureCore.Services;
public class IpAddressesPool : IIpAddressesPool
{
    private readonly HostingOptions _hostingOptions;
    private readonly IMongoDatabase _mongoDatabase;

    public IpAddressesPool(IMongoDatabase mongoDatabase, IOptions<HostingOptions> options)
    {
        _mongoDatabase = mongoDatabase;
        _hostingOptions = options.Value;
    }

    public async Task<IpQuota> TakeIp()
    {
        var collection = _mongoDatabase.GetCollection<IpQuota>("ip_addresses");
        var ipsQuotas = await collection.Find(x => true).ToListAsync();

        var ips = ipsQuotas.Select(x => ParseIp(x.Ip)).ToList();
        var startIp = ParseIp(_hostingOptions.InternalIp);

        for (var i = 0; i < SubnetMaskToCount(_hostingOptions.SubnetMask); i++)
        {
            var ip = startIp + i;
            if (!ips.Contains(ip))
            {
                var ipQuota = new IpQuota(IntToIp(ip));
                await collection.InsertOneAsync(ipQuota);
                return ipQuota;
            }
        }

        return IpQuota.Empty;
    }

    private static int ParseIp(string ip)
    {
        var parts = ip.Split('.');
        var result = 0;
        for (var i = 0; i < parts.Length; i++)
        {
            result += int.Parse(parts[i]) << (8 * (3 - i));
        }

        return result;
    }

    private static int SubnetMaskToCount(int subnetMask)
    {
        return (int)Math.Pow(2, 32 - subnetMask);
    }

    private static string IntToIp(int ip)
    {
        var parts = new StringBuilder();
        for (var i = 0; i < 4; i++)
        {
            parts.Append(((ip >> (8 * (3 - i))) & 0xFF).ToString());
        }

        return string.Join('.', parts);
    }
}
