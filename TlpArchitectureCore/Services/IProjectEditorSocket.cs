using TlpArchitectureCore.Services.ProjectEditorCommands;

namespace TlpArchitectureCore.Services;

public interface IProjectEditorSocket: IDisposable
{
    public bool IsOpen
    {
        get;
    }
    public IObservable<ProjectEditorCommand> Commands
    {
        get;
    }
    public Project ProjectContext
    {
        get;
    }

    public Task SendCommand(ProjectEditorCommand command);
}