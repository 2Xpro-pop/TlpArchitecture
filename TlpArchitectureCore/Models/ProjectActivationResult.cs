using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlpArchitectureCore.Models;
public class ProjectActivationResultMessage : Message
{
    public int Id
    {
        get;
        set;
    }

    public Guid ProjectId
    {
        get;
        set;
    }
    public bool IsSuccess
    {
        get;
        set;
    }
    public string? Message
    {
        get;
        set;
    }
}
