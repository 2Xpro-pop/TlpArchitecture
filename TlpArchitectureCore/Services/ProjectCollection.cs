using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlpArchitectureCore.Services;
public class ProjectCollection: ICollection<ProjectInfo>
{
    private readonly List<ProjectInfo> _projects = new();

    public int Count => ((ICollection<ProjectInfo>)_projects).Count;

    public bool IsReadOnly => ((ICollection<ProjectInfo>)_projects).IsReadOnly;

    public void Add(ProjectInfo item) => ((ICollection<ProjectInfo>)_projects).Add(item);
    public void Clear() => ((ICollection<ProjectInfo>)_projects).Clear();
    public bool Contains(ProjectInfo item) => ((ICollection<ProjectInfo>)_projects).Contains(item);
    public void CopyTo(ProjectInfo[] array, int arrayIndex) => ((ICollection<ProjectInfo>)_projects).CopyTo(array, arrayIndex);
    public IEnumerator<ProjectInfo> GetEnumerator() => ((IEnumerable<ProjectInfo>)_projects).GetEnumerator();
    public bool Remove(ProjectInfo item) => ((ICollection<ProjectInfo>)_projects).Remove(item);
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_projects).GetEnumerator();
}
