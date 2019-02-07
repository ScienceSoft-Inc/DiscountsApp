using System.Threading.Tasks;

namespace SCNDISC.Server.Domain.Queries.Feedbacks
{
	public interface IFeedbackCountQuery
	{
		Task<long> Run();
	}
}