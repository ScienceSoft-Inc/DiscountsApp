using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates;

namespace SCNDISC.Server.Domain.Queries.Feedbacks
{
	public interface IFeedbackListQuery
	{
		Task<IEnumerable<Feedback>> Run(FilterModel filterModel);
	}
}