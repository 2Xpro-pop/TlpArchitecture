using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlpArchitectureCore.Services;
public class ProjectCollection: ICollection<Project>
{
    private readonly List<Project> _projects = new();

    public int Count => ((ICollection<Project>)_projects).Count;

    public bool IsReadOnly => ((ICollection<Project>)_projects).IsReadOnly;

    public void Add(Project item) => ((ICollection<Project>)_projects).Add(item);
    public void Clear() => ((ICollection<Project>)_projects).Clear();
    public bool Contains(Project item) => ((ICollection<Project>)_projects).Contains(item);
    public void CopyTo(Project[] array, int arrayIndex) => ((ICollection<Project>)_projects).CopyTo(array, arrayIndex);
    public IEnumerator<Project> GetEnumerator() => ((IEnumerable<Project>)_projects).GetEnumerator();
    public bool Remove(Project item) => ((ICollection<Project>)_projects).Remove(item);
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_projects).GetEnumerator();
}
