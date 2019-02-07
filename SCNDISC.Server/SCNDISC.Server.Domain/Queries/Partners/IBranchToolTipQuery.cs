using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Partners;

namespace SCNDISC.Server.Domain.Queries.Partners
{
    public interface IBranchToolTipQuery
    {
        Task<Branch> Run(string partnerId, string branchId);
    }
}
