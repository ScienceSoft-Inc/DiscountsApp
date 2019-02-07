using Microsoft.Extensions.Configuration;
using SCNDISC.Server.Infrastructure.Settings;

namespace SCNDISC.Server.Core.Infrastructure
{
    public class SettingsService : ISettingsInfService
    {
        public SettingsService(IConfiguration configuration)
        {
            MongoConnectionString = configuration.GetValue<string>("MongodbConnectionString");
            DatabaseName = configuration.GetValue<string>("MongodbName");
        }

        public string MongoConnectionString { get; }
        public string DatabaseName { get; }
    }
}