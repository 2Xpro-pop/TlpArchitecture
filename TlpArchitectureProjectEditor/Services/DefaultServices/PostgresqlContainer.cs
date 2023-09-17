using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TlpArchitectureCore;
using TlpArchitectureCore.Docker;

namespace TlpArchitectureProjectEditor.Services.DefaultServices;
public class PostgresqlContainer : Container
{
    public string PostgressUser
    {
        get;
    }

    public string PostgressPassword
    {
        get;
    }

    public PostgresqlContainer(ServiceStartInfo serviceStartInfo) : base(serviceStartInfo.Project)
    {
        Name = serviceStartInfo.Name;
        PostgressUser = (string)serviceStartInfo.Properties[nameof(PostgressUser)];
        PostgressPassword = (string)serviceStartInfo.Properties[nameof(PostgressPassword)];
        MaxDiskUsage = serviceStartInfo.DiskUsage;
        MaxRamUsage = serviceStartInfo.RamUsage;
        Ip = serviceStartInfo.IpAddress;
    }

    protected override DockerProcess CreateDefaultDockerProcess()
    {
        
        return DockerProcess.CreateWithEnviroments(ModifiedName, MaxRamUsage, MaxDiskUsage, Ip, "postgres", new Dictionary<string, string>()
        {
            ["POSTGRES_USER"] = PostgressUser,
            ["POSTGRES_PASSWORD"] = PostgressPassword
        });
    }
}
