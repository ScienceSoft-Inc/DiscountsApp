using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Domain.Commands.Feedback;
using SCNDISC.Server.Domain.Queries.Feedbacks;

namespace SCNDISC.Server.Application.Services.Feedback
{
	public class FeedbackService : IFeedbackService
	{
		private readonly IFeedbackListQuery _feedbackListQuery;
		private readonly IInsertFeedbackCommand _insertFeedbackCommand;
		private readonly IFeedbackCountQuery _feedbackCountQuery;

		public FeedbackService(IFeedbackListQuery feedbackListQuery, IInsertFeedbackCommand insertFeedbackCommand, IFeedbackCountQuery feedbackCountQuery)
		{
			_feedbackListQuery = feedbackListQuery;
			_insertFeedbackCommand = insertFeedbackCommand;
			_feedbackCountQuery = feedbackCountQuery;
		}

		public async Task<Domain.Aggregates.Feedback> AddAsync(Domain.Aggregates.Feedback feedback)
		{
			return await _insertFeedbackCommand.ExecuteAsync(feedback);
		}

		public async Task<IEnumerable<Domain.Aggregates.Feedback>> GetAll(FilterModel filterModel)
		{
			return await _feedbackListQuery.Run(filterModel);
		}

		public async Task<long> GetCount()
		{
			return await _feedbackCountQuery.Run();
		}
	}
}