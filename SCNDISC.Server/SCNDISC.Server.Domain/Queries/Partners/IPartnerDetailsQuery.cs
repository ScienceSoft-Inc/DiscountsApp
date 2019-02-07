using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using SCNDISC.Server.Domain.Aggregates.Partners;

namespace SCNDISC.Server.Domain.Queries.Partners
{
    public interface IPartnerDetailsQuery
    {
        Task<IEnumerable<Branch>> Run(string partnerId);
    }
}
