using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlpArchitectureCore.Docker;
public class DockerProcess : Process
{
    internal DockerProcess(string command, string arguments)
    {
        StartInfo.FileName = "docker";
        StartInfo.Arguments = $"{command} {arguments}";
        StartInfo.RedirectStandardOutput = true;
        StartInfo.RedirectStandardError = true;
        StartInfo.CreateNoWindow = false;
    }

    public static DockerProcess CreateDefault(string name, int ramUsage, int diskUsage, string image) =>
        new("run", $"--name {name} -m {ramUsage}m --storage-opt size={diskUsage}m {image}");

    public static DockerProcess CreateWithEnviroments(string name, int ramUsage, int diskUsage, string ip, string image, Dictionary<string, string> enviroments)
    {
        var arguments = $"--name {name} ";
        foreach (var kv in enviroments)
        {
            arguments += $"-e {kv.Key}={kv.Value} ";
        }
#if XFS_SUPPORT
        arguments += $"-m {ramUsage}m --storage-opt size={diskUsage}m ";
#else
        arguments += $"-m {ramUsage}m ";
#endif
        if (!string.IsNullOrWhiteSpace(ip))
        {
            arguments += $"--net tlparchitecture --ip {ip} ";
        }

        arguments += image;

        return new DockerProcess("run", arguments);
    }

    public static async Task<int> GetMomoryUsage(string containerName)
    {
        var process = new DockerProcess("stats", containerName);
        process.Start();

        await process.WaitForExitAsync();

        var output = await process.StandardOutput.ReadToEndAsync();
        var lines = output.Split(Environment.NewLine);

        var memoryLine = lines[1];
        var memory = memoryLine.Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray()[2];

        return int.Parse(memory.Replace("MiB", ""));
    }

}
