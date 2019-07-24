using System.Threading.Tasks;

namespace SCNDISC.Server.Domain.Queries.Notifications
{
    public interface INotificationCountQuery
    {
        Task<long> RunAsync(string language);
    }
}
