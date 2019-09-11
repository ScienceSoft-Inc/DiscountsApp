using SCNDISC.Server.Domain.Aggregates.Partners;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCNDISC.Server.Domain.Queries.PartnerRating
{
    public interface IDeviceRatingsQuery
    {
        Task<IEnumerable<Rating>> RunAsync(string deviceId);
    }
}
