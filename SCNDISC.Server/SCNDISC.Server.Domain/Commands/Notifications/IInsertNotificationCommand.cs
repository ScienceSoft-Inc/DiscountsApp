using System.Threading.Tasks;

namespace SCNDISC.Server.Domain.Commands.Notifications
{
    public interface IInsertNotificationCommand
    {
        Task<Aggregates.Notification> ExecuteAsync(Aggregates.Notification pushNotification);
    }
}
