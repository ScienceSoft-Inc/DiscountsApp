using SCNDISC.Server.Domain.Aggregates;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCNDISC.Server.Domain.Queries.Notifications
{
    public interface INotificationListTodayQuery
    {
        Task<IEnumerable<Notification>> RunAsync(string language);
    }
}
