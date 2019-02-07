using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Partners;

namespace SCNDISC.Server.Domain.Queries.Partners
{
    public interface IPartnersOnlyQuery
    {
        Task<IEnumerable<Branch>> Run(string[] selectedCategories = null, string partnerName = null);
    }
}
