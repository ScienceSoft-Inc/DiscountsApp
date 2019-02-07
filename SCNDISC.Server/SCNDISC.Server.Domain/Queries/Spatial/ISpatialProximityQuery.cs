using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Partners;

namespace SCNDISC.Server.Domain.Queries.Spatial
{
    public interface ISpatialProximityQuery
    {
        Task<IEnumerable<Branch>> Run(double longitude, double latitude, int proximity);
    }
}
