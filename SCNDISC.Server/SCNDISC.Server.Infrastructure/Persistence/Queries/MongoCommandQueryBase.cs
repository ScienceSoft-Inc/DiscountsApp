using System.Threading.Tasks;
using MongoDB.Driver;
using SCNDISC.Server.Domain.Aggregates;
using SCNDISC.Server.Infrastructure.Persistence.Providers;

namespace SCNDISC.Server.Infrastructure.Persistence.Queries
{
	public abstract class MongoCommandQueryBase<TAggregate> where TAggregate : Aggregate
	{
		protected readonly IMongoCollectionProvider Provider;
		protected static readonly UpdateOptions UpsertOptions = new UpdateOptions {IsUpsert = true};

	    protected MongoCommandQueryBase(IMongoCollectionProvider provider)
		{
			Provider = provider;
		}

		protected IMongoCollection<TAggregate> Collection => Provider.GetCollection<TAggregate>();

	    protected async Task<TAggregate> Upsert(TAggregate aggregate)
		{
			var filter = new FilterDefinitionBuilder<TAggregate>().Where(p => p.Id == aggregate.Id);
			await Collection.ReplaceOneAsync(filter, aggregate, UpsertOptions);
			return aggregate;
		}
	}
}