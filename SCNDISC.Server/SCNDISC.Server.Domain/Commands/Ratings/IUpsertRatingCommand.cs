using SCNDISC.Server.Domain.Aggregates.Partners;
using System.Threading.Tasks;

namespace SCNDISC.Server.Domain.Commands.Ratings
{
    public interface IUpsertRatingCommand
    {
        Task<Rating> UpsertRatingAsync(Rating rating);
    }
}
