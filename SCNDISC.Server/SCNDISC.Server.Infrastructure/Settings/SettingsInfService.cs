using System.Configuration;

namespace SCNDISC.Server.Infrastructure.Settings
{
    public class SettingsInfService : ISettingsInfService
    {
        public string MongoConnectionString
        {
            get
            {
	            return ConfigurationManager.AppSettings["MongodbConnectionString"];
			}
        }

	    public string DatabaseName
	    {
		    get { return ConfigurationManager.AppSettings["MongodbName"]; }
	    }
    }
}
