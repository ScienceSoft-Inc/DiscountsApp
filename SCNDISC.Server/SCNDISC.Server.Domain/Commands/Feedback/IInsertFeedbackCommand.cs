using System.Threading.Tasks;

namespace SCNDISC.Server.Domain.Commands.Feedback
{
	public interface IInsertFeedbackCommand
	{
		Task<Aggregates.Feedback> ExecuteAsync(Aggregates.Feedback category);
	}
}