using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace TlpArchitectureCore.Docker;
public abstract class Container : IDisposable
{
    private DockerProcess _mainDockerProcess = null!;
    protected Subject<string?> logSubject = new();

    public Container(Project projectContext)
    {
        ProjectContext = projectContext;
        MainDockerProcess = CreateDefaultDockerProcess();
    }
    public Project ProjectContext
    {
        get;
    }
    public abstract string Name
    {
        get;
    }
    public abstract string Image
    {
        get; set;
    }
    public abstract int RamUsage
    {
        get; set;
    }
    public abstract int DiskUsage
    {
        get; set;
    }
    public abstract string BuildPath
    {
        get; set;
    }
    /// <summary>
    /// Logs from docker process
    /// </summary>
    public virtual IObservable<string?> InternalLogs => logSubject;

    protected DockerProcess MainDockerProcess
    {
        get => _mainDockerProcess;
        set
        {
            if (IsStarted)
            {
                throw new InvalidOperationException("Container already started");
            }
            _mainDockerProcess = value;

            if (_mainDockerProcess == null)
            {
                return;
            }
            _mainDockerProcess.OutputDataReceived += (sender, args) => Log(args.Data);
        }
    }

    protected bool Disposed
    {
        get;
        private set;
    }

    protected bool IsStarted
    {
        get;
        private set;
    }

    /// <summary>
    /// Start process 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public virtual Task StartProcessAsync(CancellationToken cancellationToken)
    {
        if (!MainDockerProcess.Start())
        {
            throw new InvalidOperationException("Docker process not started");
        }

        MainDockerProcess.BeginOutputReadLine();
        MainDockerProcess.BeginErrorReadLine();

        IsStarted = true;

        return Task.CompletedTask;
    }

    /// <summary>
    /// Stop only process without disposing
    /// </summary>
    /// <returns></returns>
    public async virtual Task StopProcessAsync(CancellationToken cancellationToken)
    {
        if (MainDockerProcess.HasExited)
        {
            return;
        }

        MainDockerProcess.Kill();

        await MainDockerProcess.WaitForExitAsync(cancellationToken);

        IsStarted = false;
        MainDockerProcess = null!;
    }

    protected virtual void Log(string? message) => logSubject.OnNext(message);

    protected async virtual Task RestartContainer(CancellationToken cancellationToken)
    {
        await StopProcessAsync(cancellationToken);

        MainDockerProcess = CreateDefaultDockerProcess();

        await StartProcessAsync(cancellationToken);
    }

    protected virtual DockerProcess CreateDefaultDockerProcess() =>
        DockerProcess.CreateDefault(Name, RamUsage, DiskUsage, Image);

    #region IDisposable Support
    protected virtual void Dispose(bool disposing)
    {
        if (!Disposed)
        {
            if (disposing)
            {
                // TODO: освободить управляемое состояние (управляемые объекты)
                logSubject.Dispose();
            }

            // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить метод завершения

            // TODO: установить значение NULL для больших полей
            MainDockerProcess = null!;
            logSubject = null!;
            Disposed = true;
        }
    }

    ~Container()
    {
        // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}
