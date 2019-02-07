using System.Security.Cryptography.X509Certificates;

namespace SCNDISC.Server.Infrastructure.Settings
{
    public interface ISettingsInfService
    {
        string MongoConnectionString { get; }
        string DatabaseName { get; }
    }
}
