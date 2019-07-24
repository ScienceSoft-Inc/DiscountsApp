using SCNDISC.Server.Domain.Queries.Notifications;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using SCNDISC.Server.Infrastructure.Persistence.Providers;
using MongoDB.Bson;
using System;

namespace SCNDISC.Server.Infrastructure.Persistence.Queries.Notifications
{
    public class NotificationListQuery : MongoCommandQueryBase<Domain.Aggregates.Notification>, INotificationListQuery
    {
        public NotificationListQuery(IMongoCollectionProvider provider)
        : base(provider)
            {
            }

        public async Task<IEnumerable<Domain.Aggregates.Notification>> RunAsync(FilterModel filterModel)
        {
            if (string.IsNullOrEmpty(filterModel.Language))
            {
                return await Collection.Aggregate().
                    SortByDescending(n => n.Created).
                    Skip(filterModel.Skip).
                    Limit(filterModel.Take).
                    ToListAsync();
            }

            return await Collection.Aggregate().
                Match(n => n.Language == filterModel.Language).
                SortByDescending(n => n.Created).
                Skip(filterModel.Skip).
                Limit(filterModel.Take).
                ToListAsync();
        }
    }

    public class NotificationListTodayQuery : MongoCommandQueryBase<Domain.Aggregates.Notification>, INotificationListTodayQuery
    {
        public NotificationListTodayQuery(IMongoCollectionProvider provider)
            : base(provider)
            {
            }

        public async Task<IEnumerable<Domain.Aggregates.Notification>> RunAsync(string language)
        {
            if (!string.IsNullOrEmpty(language))
            {
                return await Collection.Aggregate().
                Match(n => n.Created >= DateTime.UtcNow.Date && n.Language == language).
                SortByDescending(x => x.Created).
                ToListAsync();
            }

            return await Collection.Aggregate().
            Match(n => n.Created >= DateTime.UtcNow.Date).
            SortByDescending(x => x.Created).
            ToListAsync();
        }
    }

    public class NotificationCountQuery : MongoCommandQueryBase<Domain.Aggregates.Notification>, INotificationCountQuery
    {
        public NotificationCountQuery(IMongoCollectionProvider provider)
            : base(provider)
        {
        }

        public async Task<long> RunAsync(string language)
        {
            if (string.IsNullOrEmpty(language))
                return await Collection.CountAsync(n => true);
            return await Collection.CountAsync(n => n.Language == language); 
        }
    }
}
