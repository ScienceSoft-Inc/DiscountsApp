using MongoDB.Driver;
using SCNDISC.Server.Domain.Queries.Feedbacks;
using SCNDISC.Server.Infrastructure.Persistence.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCNDISC.Server.Infrastructure.Persistence.Queries.Feedback
{
    public class FeedbackListQuery : MongoCommandQueryBase<Domain.Aggregates.Feedback>, IFeedbackListQuery
	{
		public FeedbackListQuery(IMongoCollectionProvider provider) 
			: base(provider)
		{
		}

		public async Task<IEnumerable<Domain.Aggregates.Feedback>> Run(FilterModel filterModel)
        {
            return await Collection.Aggregate()
                .SortByDescending(x => x.Created)
                .Skip(filterModel.Skip)
                .Limit(filterModel.Take)
                .ToListAsync();
        }
	}

	public class FeedbackCountQuery : MongoCommandQueryBase<Domain.Aggregates.Feedback>, IFeedbackCountQuery
	{
		public FeedbackCountQuery(IMongoCollectionProvider provider) 
			: base(provider)
		{
		}

		public async Task<long> Run()
		{
			return await Collection.CountDocumentsAsync(x => true);
		}
	}
}