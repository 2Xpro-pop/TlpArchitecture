﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlpArchitectureCore.Docker;
public abstract class DockerFileBuilder
{
    public ProjectContext ProjectContext
    {
        get;
    }

    public string BuildPath
    {
        get;
    }

    public DockerFileBuilder(ProjectContext projectContext, string buildPath)
    {
        ProjectContext = projectContext;
        BuildPath = buildPath;
    }
    public abstract string Build();
}
