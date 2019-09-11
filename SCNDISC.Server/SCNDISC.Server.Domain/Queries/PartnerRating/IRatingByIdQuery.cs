using SCNDISC.Server.Domain.Aggregates.Partners;
using System.Threading.Tasks;

namespace SCNDISC.Server.Domain.Queries.PartnerRating
{
    public interface IRatingByIdQuery
    {
        Task<Rating> RunAsync(string id);
    }
}
