using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DNS.Client.RequestResolver;
using DNS.Protocol;
using DNS.Protocol.ResourceRecords;
using DNS.Server;
using DnsClient.Protocol;
using TlpArchitectureProjectEditor.Models;
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

    public async Task<IResponse> Resolve(IRequest request, CancellationToken cancellationToken = default)
    {
        var question = request.Questions[0];

        var remoteService = await _serviceStartInfosService.GetServiceByIp(RemoteIp);
        var projectServices = await _serviceStartInfosService.GetAllServiceStartInfosForProject(remoteService.ProjectId);

        var domainService = projectServices.FirstOrDefault(s => s.InternalDomain.ToLower() == question.Name.ToString().ToLower());

        if (domainService is not null)
        {
            var links = await _linkService.GetAllLinks(domainService.Id);

            var hasRemoteToDomainLink = (ServicesLink x) => x.FirstServiceStartInfoId == remoteService.Id && x.SecondServiceStartInfoId == domainService.Id;
            var hasDomainToRemotenLink = (ServicesLink x) => x.FirstServiceStartInfoId == domainService.Id && x.SecondServiceStartInfoId == remoteService.Id;

            if (links.Any(x => hasDomainToRemotenLink(x) && hasRemoteToDomainLink(x)))
            {
                var response = Response.FromRequest(request);
                IResourceRecord record = new IPAddressResourceRecord(
                     question.Name, IPAddress.Parse(domainService.IpAddress));
                response.AnswerRecords.Add(record);
                return response;
            }
        }

        return await _masterFile.Resolve(request, cancellationToken);
    }
}
