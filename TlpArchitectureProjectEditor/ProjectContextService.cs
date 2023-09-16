using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;

namespace TlpArchitectureProjectEditor;
public class ProjectContextService
{
    public IObservable<IChangeSet<ProjectContext, Guid>> Projects => projectsCache.Connect();

    private readonly SourceCache<ProjectContext, Guid> projectsCache = new(x => x.ProjectId);

    public bool TryActivate(ProjectContext projectContext)
    {
        if (projectsCache.Lookup(projectContext.ProjectId).HasValue)
        {
            return false;
        }

        projectsCache.AddOrUpdate(projectContext);

        return true;
    }
}
