namespace SCNDISC.Server.Infrastructure.Settings
{
    public interface ISettingsInfService
    {
        string MongoConnectionString { get; }
        string DatabaseName { get; }
        string FcmKey { get; }
    }
}
