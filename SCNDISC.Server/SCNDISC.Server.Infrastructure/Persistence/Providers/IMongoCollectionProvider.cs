using MongoDB.Driver;
using SCNDISC.Server.Domain.Aggregates;

namespace SCNDISC.Server.Infrastructure.Persistence.Providers
{
    public interface IMongoCollectionProvider
    {
        IMongoCollection<TAggregate> GetCollection<TAggregate>() where TAggregate : Aggregate;
    }
}
