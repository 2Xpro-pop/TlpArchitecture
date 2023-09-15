using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlpArchitectureCore.Models;
public class Quota
{
    public int Id
    {
        get; set;
    }

    /// <summary>
    /// in megabytes
    /// </summary>
    public int RamUsage
    {
        get; set;
    }

    /// <summary>
    /// in megabytes
    /// </summary>
    public int DiskUsage
    {
        get; set;
    }

    /// <summary>
    /// in days
    /// </summary>
    public double Duration
    {
        get; set;
    }

    /// <summary>
    /// in days
    /// </summary>
    public double Pause
    {
        get; set;
    }
}
