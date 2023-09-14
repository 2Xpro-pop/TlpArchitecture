using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TlpArchitectureCore.Docker;

namespace TlpArchitectureCore;
public class ProjectContext
{
    /// <summary>
    /// The unique name of the project
    /// </summary>
    public string Name
    {
        get;
        set;
    } = null!;

    public ObservableCollection<Container> Containers
    {
        get;
    } = new();
}
