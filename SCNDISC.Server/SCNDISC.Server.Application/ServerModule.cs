using Ninject.Extensions.Conventions;
using Ninject.Extensions.Conventions.Syntax;
using Ninject.Modules;

namespace SCNDISC.Server.Application
{
    public class ServerModule : NinjectModule
    {
        private const string serverAssemblyMask = "SCNDISC.Server*";

        public override void Load()
        {
            Register(serverAssemblyMask, "Service", p => p.InTransientScope());
            Register(serverAssemblyMask, "Query", p => p.InTransientScope());
            Register(serverAssemblyMask, "Command", p => p.InTransientScope());
            Register(serverAssemblyMask, "Provider", p => p.InSingletonScope());
        }

        private void Register(string assemlyMask, string nameEndsWith, ConfigurationAction configurationAction)
        {
            Kernel.Bind(x => x.FromAssembliesMatching(assemlyMask).Select(t => t.FullName.EndsWith(nameEndsWith)).BindDefaultInterface().Configure(configurationAction));
        }
    }
}
