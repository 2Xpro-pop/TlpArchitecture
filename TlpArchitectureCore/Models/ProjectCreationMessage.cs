using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlpArchitectureCore.Models;
public class ProjectCreationMessage
{
    public int Id
    {
        get; set;
    }

    public string Name
    {
        get; set;
    } = null!;

    public string ProjectDomain
    {
        get; set;
    } = null!;

    public bool IsPublicDomain
    {
        get; set;
    } = true;

    public string? GitHubToken
    {
        get; set;
    }

    public int[] Users
    {
        get; set;
    } = Array.Empty<int>();

    public int QuotaId
    {
        get; set;
    }
}
