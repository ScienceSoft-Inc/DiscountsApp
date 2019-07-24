using System.Threading.Tasks;
using SCNDISC.Server.Domain.Commands.Notifications;
using SCNDISC.Server.Infrastructure.Persistence.Providers;
using SCNDISC.Server.Infrastructure.Persistence.Queries;

namespace SCNDISC.Server.Infrastructure.Persistence.Commands.Notifications
{
    public class InsertNotificationCommand : MongoCommandQueryBase<Domain.Aggregates.Notification>, IInsertNotificationCommand
    {
        public InsertNotificationCommand(IMongoCollectionProvider provider)
            : base(provider)
        {
        }

        public async Task<Domain.Aggregates.Notification> ExecuteAsync(Domain.Aggregates.Notification notification)
        {
            await Collection.InsertOneAsync(notification);
            return notification;
        }
    }
}
