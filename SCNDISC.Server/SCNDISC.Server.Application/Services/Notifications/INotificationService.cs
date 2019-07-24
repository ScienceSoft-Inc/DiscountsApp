using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Domain.Queries.Notifications;

namespace SCNDISC.Server.Application.Services.Notifications
{
    public interface INotificationService
    {
        Task<string> PushNotificationAsync(string content);
        Task<Domain.Aggregates.Notification> AddAsync(Domain.Aggregates.Notification notification);
        Task<IEnumerable<Domain.Aggregates.Notification>> GetAll(FilterModel filterModel);
        Task<IEnumerable<Domain.Aggregates.Notification>> GetTodayNotificationAsync(string language);
        Task<long> GetCount(string language);
    }
}
