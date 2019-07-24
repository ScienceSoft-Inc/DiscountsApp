namespace SCNDISC.Server.Infrastructure.Settings
{
    public class SettingsInfService : ISettingsInfService
    {
        public string MongoConnectionString
        {
            get
            {
	            return "mongodb://localhost/";
			}
        }

	    public string DatabaseName
	    {
		    get { return "SCNDISC_Test"; }
	    }

        public string FcmKey
        {
            get { return ""; }
        }
    }
}
