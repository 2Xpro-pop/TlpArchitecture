using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNS.Client.RequestResolver;
using DNS.Protocol;
using DNS.Server;

namespace TlpArchitecture.Services;
public class RequestResolver: IRequestResolver
{
    private readonly MasterFile _masterFile = new();

    public Task<IResponse> Resolve(IRequest request, CancellationToken cancellationToken = default)
    {
        return _masterFile.Resolve(request, cancellationToken);
    }
}
