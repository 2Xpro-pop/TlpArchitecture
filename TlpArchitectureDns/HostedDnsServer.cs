using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DNS.Client.RequestResolver;
using DNS.Protocol.ResourceRecords;
using DNS.Server;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TlpArchitecture;
public class HostedDnsServer : BackgroundService
{
    private readonly ILogger _logger;
    private readonly IRequestResolver _requestResolver;
    public HostedDnsServer(ILogger<HostedDnsServer> logger, IRequestResolver requestResolver)
    {
        _logger = logger;
        _requestResolver = requestResolver;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var dnsServer = new DnsServer(_requestResolver, "8.8.8.8");

        dnsServer.Requested += (sender, args) =>
        {
            _logger.LogInformation("Request from {remote}", args.Remote);
        };

        dnsServer.Responded += (sender, args) =>
        {
            _logger.LogInformation("Responded from {remote}", args.Remote);
        };

        dnsServer.Requested += (sender, e) => Console.WriteLine(e.Request);
        dnsServer.Responded += (sender, e) => Console.WriteLine("{0} => {1}", e.Request, e.Response);
        dnsServer.Errored += (sender, e) => Console.WriteLine(e.Exception.Message);

        _logger.LogInformation("Starting DNS server");

        await dnsServer.Listen(ip: IPAddress.Parse("192.168.0.101"));

        _logger.LogInformation("DNS server ended");
    }
}
