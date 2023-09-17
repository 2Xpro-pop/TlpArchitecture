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
    protected Subject<string> logSubject = new();
    protected ReplaySubject<string> errorSubject = new();

    public Container(ProjectInfo projectContext)
    {
        ProjectContext = projectContext;
    }
    public ProjectInfo ProjectContext
    {
        get;
    }
    public virtual string Name
    {
        get; protected set;
    }
    public virtual string ModifiedName => $"{ProjectContext.Domain}.{Name}";
    public virtual string Image
    {
        get; protected set;
    }
    public virtual int MaxRamUsage
    {
        get; set;
    }
    public virtual int MaxDiskUsage
    {
        get; set;
    }
    public virtual string Ip
    {
        get; set;
    }
    public virtual string BuildPath
    {
        get; set;
    }
    /// <summary>
    /// Logs from docker process
    /// </summary>
    public virtual IObservable<string> InternalLogs => logSubject;
    public virtual IObservable<string> InternalErrors => errorSubject;
    public virtual string? InternalError
    {
        get; protected set;
    }

    protected DockerProcess MainDockerProcess
    {
        get => _mainDockerProcess;
        set
        {
            if (_mainDockerProcess == value)
            {
                return;
            }
            if (IsStarted)
            {
                throw new InvalidOperationException("Container already started");
            }

            if (_mainDockerProcess != null)
            {
                _mainDockerProcess.OutputDataReceived -= (sender, args) => Log(args.Data);
                _mainDockerProcess.ErrorDataReceived -= (sender, args) => errorSubject.OnNext(args.Data);
            }

            _mainDockerProcess = value;

            if (_mainDockerProcess == null)
            {
                return;
            }

            _mainDockerProcess.OutputDataReceived += (sender, args) => Log(args.Data);
            _mainDockerProcess.ErrorDataReceived += (sender, args) => errorSubject.OnNext(args.Data);
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
    public async virtual Task<bool> StartProcessAsync(CancellationToken cancellationToken)
    {
        MainDockerProcess = CreateDefaultDockerProcess();

        if (!MainDockerProcess.Start())
        {
            throw new InvalidOperationException("Docker process not started");
        }

        await Task.Delay(400, cancellationToken);

        if (MainDockerProcess.HasExited)
        {
            InternalError = await MainDockerProcess.StandardError.ReadToEndAsync(cancellationToken);
            IsStarted = false;
            MainDockerProcess = null!;

            IsStarted = false;
            return false;
        }

        MainDockerProcess.BeginOutputReadLine();
        MainDockerProcess.BeginErrorReadLine();

        IsStarted = true;

        return true;
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
        await StartProcessAsync(cancellationToken);
    }

    protected virtual DockerProcess CreateDefaultDockerProcess() =>
        DockerProcess.CreateDefault(Name, MaxRamUsage, MaxDiskUsage, Image);

    #region IDisposable Support
    protected virtual void Dispose(bool disposing)
    {
        if (!Disposed)
        {
            if (disposing)
            {
                // TODO: освободить управляемое состояние (управляемые объекты)
                logSubject.Dispose();
                errorSubject.Dispose();
            }

            // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить метод завершения

            // TODO: установить значение NULL для больших полей
            IsStarted = false;
            MainDockerProcess = null!;
            logSubject = null!;
            errorSubject = null!;
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
