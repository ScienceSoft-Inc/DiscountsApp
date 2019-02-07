using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using SCNDISC.Server.Domain.Queries.Feedbacks;
using SCNDISC.Server.Infrastructure.Persistence.Providers;

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
			return await Collection.Aggregate().
				Skip(filterModel.Skip).
				Limit(filterModel.Take).
				SortByDescending(x=>x.Created).
				ToListAsync();

			//return GetFeedbacks(filterModel.Skip, filterModel.Take);
		}

		private List<Domain.Aggregates.Feedback> GetFeedbacks(int skip, int take)
		{
			return new List<Domain.Aggregates.Feedback>
			{
				new Domain.Aggregates.Feedback
				{
					UserName = "user name 1",
					Message = GetMessgae()
				},
				new Domain.Aggregates.Feedback
				{
					UserName = "user name 2",
					Message = GetMessgae()
				},
				new Domain.Aggregates.Feedback
				{
					UserName = "user name 3",
					Message = GetMessgae()
				},
				new Domain.Aggregates.Feedback {UserName = "user name 4", Message = GetMessgae()},
				new Domain.Aggregates.Feedback {UserName = "user name 5", Message = GetMessgae()},
				new Domain.Aggregates.Feedback {UserName = "user name 6", Message = GetMessgae()},
				new Domain.Aggregates.Feedback {UserName = "user name 7", Message = GetMessgae()},
				new Domain.Aggregates.Feedback {UserName = "user name 8", Message = GetMessgae()},
				new Domain.Aggregates.Feedback {UserName = "user name 9", Message = GetMessgae()},
				new Domain.Aggregates.Feedback {UserName = "user name 10", Message = GetMessgae()},
				new Domain.Aggregates.Feedback {UserName = "user name 11", Message = GetMessgae()},
				new Domain.Aggregates.Feedback {UserName = "user name 12", Message = GetMessgae()}
			}.Skip(skip).Take(take).ToList();
		}

		private string GetMessgae()
		{
			return "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus placerat convallis nisi non laoreet. Sed eleifend sed quam vel egestas. In vitae odio vitae nisl lobortis laoreet in eu nibh. Aliquam luctus, augue vel egestas rutrum, purus est consectetur dui, ac blandit mi urna eget nunc. Suspendisse libero nunc, tempor at bibendum vitae, pellentesque id ex. Vestibulum et suscipit arcu, quis laoreet nulla. Maecenas tempor consequat neque, sed blandit nunc tempus in.";
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
			//return 12;
			return await Collection.CountAsync(x => true);
		}
	}
}