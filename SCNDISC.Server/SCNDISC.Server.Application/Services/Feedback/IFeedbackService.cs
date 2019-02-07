using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Domain.Queries.Feedbacks;

namespace SCNDISC.Server.Application.Services.Feedback
{
	public interface IFeedbackService
	{
		Task<Domain.Aggregates.Feedback> AddAsync(Domain.Aggregates.Feedback feedback);
		Task<IEnumerable<Domain.Aggregates.Feedback>> GetAll(FilterModel filterModel);
		Task<long> GetCount();
	}
}