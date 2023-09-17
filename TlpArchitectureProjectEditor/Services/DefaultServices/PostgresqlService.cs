using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace TlpArchitectureProjectEditor.Services.DefaultServices;
public class PostgresqlService : IService
{
    private PostgresqlContainer _container = null!;

    public PostgresqlService(ServiceStartInfo startInfo)
    {
        StartInfo = startInfo;
        Ip = startInfo.IpAddress;
    }

    public ServiceStartInfo StartInfo
    {
        get;
    }

    public IObservable<string> Logs => _container.InternalLogs;

    public IObservable<string> Errors => _errors;
    private readonly ReplaySubject<string> _errors = new();

    public string? Error
    {
        get; set;
    }

    public bool IsWork
    {
        get; set;
    }

    public string Ip
    {
        get; set;
    } = null!;

    public async Task<bool> StartAsync()
    {
        _container = new(StartInfo);

        _container.InternalErrors.Subscribe(_errors.OnNext);

        IsWork = await _container.StartProcessAsync(CancellationToken.None);

        if (!IsWork)
        {
            Error = _container.InternalError;
        }

        return IsWork;
    }
}
