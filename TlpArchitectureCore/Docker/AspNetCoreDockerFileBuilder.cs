using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlpArchitectureCore.Docker;
public class AspNetCoreDockerFileBuilder : DockerFileBuilder
{
    public string DllName
    {
        get; 
    } 

    public AspNetCoreDockerFileBuilder(ProjectContext projectContext, string buildPath, string dllName) : base(projectContext, buildPath)
    {
        DllName = dllName;
    }

    public override string Build()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS build-env");
        stringBuilder.AppendLine("COPY . ./");
        stringBuilder.AppendLine($"ENTRYPOINT [\"dotnet\", \"{DllName}\" ]");
        return stringBuilder.ToString();
    }
}
