using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlpArchitectureProjectEditor.Services;
public interface IServiceFactory
{
    public IService? CreateService(ServiceStartInfo serviceStartInfo);
}
