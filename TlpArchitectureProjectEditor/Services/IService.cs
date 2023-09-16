using System.Text.Json;
using TlpArchitectureCore;

namespace TlpArchitectureProjectEditor.Services;

public interface IService
{
    public ServiceStartInfo StartInfo
    {
        get;
    }

    public IObservable<string> Logs
    {
        get;
    }

    public IObservable<string> Errors
    {
        get;
    }

    public bool IsWork
    {
        get;
    }

    public string Ip
    {
        get;
    }

    /// <summary>
    /// Also can restart service
    /// </summary>
    /// <returns></returns>
    public Task StartAsync();
}
