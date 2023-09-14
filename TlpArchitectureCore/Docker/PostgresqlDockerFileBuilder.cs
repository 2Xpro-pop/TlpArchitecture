using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlpArchitectureCore.Docker;
public class PostgresqlDockerFileBuilder : DockerFileBuilder
{
    public string PgDataVolume
    {
        get; set;
    } = "pgdata";

    public override string Build()
    {
        var builder = new StringBuilder();
        builder.AppendLine("FROM postgres:latest");
        builder.AppendLine("PGDATA: \"/var/lib/postgresql/data/pgdata\"");
        return builder.ToString();
    }
}
