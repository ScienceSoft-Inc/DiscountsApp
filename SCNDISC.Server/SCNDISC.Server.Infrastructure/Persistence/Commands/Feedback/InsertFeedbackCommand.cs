using System.Threading.Tasks;
using SCNDISC.Server.Domain.Commands.Feedback;
using SCNDISC.Server.Infrastructure.Persistence.Providers;
using SCNDISC.Server.Infrastructure.Persistence.Queries;

namespace SCNDISC.Server.Infrastructure.Persistence.Commands.Feedback
{
	public class InsertFeedbackCommand : MongoCommandQueryBase<Domain.Aggregates.Feedback>, IInsertFeedbackCommand
	{
		public InsertFeedbackCommand(IMongoCollectionProvider provider)
			: base(provider)
		{
		}

		public async Task<Domain.Aggregates.Feedback> ExecuteAsync(Domain.Aggregates.Feedback feedback)
		{
			await Collection.InsertOneAsync(feedback);
			return feedback;
		}
	}
}