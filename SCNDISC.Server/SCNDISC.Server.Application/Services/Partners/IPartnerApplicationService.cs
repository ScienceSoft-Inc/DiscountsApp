using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Partners;

namespace SCNDISC.Server.Application.Services.Partners
{
    public interface IPartnerApplicationService
    {
        Task<IEnumerable<Branch>> GetPartnerDetailsAsync(string partnerId);
        Task<Branch> GetBranchToolTipAsync(string partnerId, string branchId);
        Task<Branch> UpsertBranchAsync(Branch branch);
        Task<Branch> UpsertPartnerAsync(Branch branch);
        Task DeleteBranchAsync(string partnerId);
        Task DeletePartnerAsync(string partnerId);
    }
}
