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
    }

    public ServiceStartInfo StartInfo
    {
        get;
    }

    public IObservable<string> Logs => _container.InternalLogs;

    public IObservable<string> Errors => _errors;
    private readonly Subject<string> _errors = new();

    public bool IsWork
    {
        get; set;
    }

    public string Ip
    {
        get; set;
    } = null!;

    public async Task StartAsync()
    {
        _container = new(StartInfo);

        await _container.StartProcessAsync(CancellationToken.None);

        IsWork = true;
        Ip = await _container.GetIp();
    }
}
