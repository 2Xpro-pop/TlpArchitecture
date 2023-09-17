using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNS.Client.RequestResolver;
using DNS.Protocol;
using DNS.Server;
using TlpArchitectureProjectEditor.Services;
using static DNS.Server.DnsServer;

namespace TlpArchitecture.Services;
public class RequestResolver : IRequestResolver
{
    private readonly ILinkService _linkService;
    private readonly IServiceStartInfosService _serviceStartInfosService;

    public string RemoteIp
    {
        get; set;
    }
    public RequestResolver(ILinkService linkService)
    {
        _linkService = linkService;
    }

    private readonly MasterFile _masterFile = new();

    public Task<IResponse> Resolve(IRequest request, CancellationToken cancellationToken = default)
    {
        var question = request.Questions[0];

        _serviceStartInfosService.GetServiceByIp(RemoteIp);



        return _masterFile.Resolve(request, cancellationToken);
    }
}
