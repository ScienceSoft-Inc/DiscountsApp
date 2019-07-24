using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates;

namespace SCNDISC.Server.Domain.Queries.Notifications
{
    public interface INotificationListQuery
    {
        Task<IEnumerable<Notification>> RunAsync(FilterModel filterModel);
    }
}
