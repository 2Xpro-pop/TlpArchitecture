using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TlpArchitectureProjectEditor.Models;

namespace TlpArchitectureProjectEditor.Services;
public interface ILinkService
{
    public Task<int> ClearDuplicates();
    public Task<IEnumerable<ServicesLink>> GetAllLinksForProject(Guid projectId);
    public Task<IEnumerable<ServicesLink>> GetAllLinks(Guid serviceStartInfoId);
}
