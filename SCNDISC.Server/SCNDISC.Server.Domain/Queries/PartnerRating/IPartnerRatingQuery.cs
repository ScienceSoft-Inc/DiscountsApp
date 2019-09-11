using SCNDISC.Server.Domain.Aggregates.Partners;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCNDISC.Server.Domain.Queries.PartnerRating
{
    public interface IPartnerRatingQuery
    {
        Task<IEnumerable<Rating>> RunAsync(string partnerId);
    }
}
