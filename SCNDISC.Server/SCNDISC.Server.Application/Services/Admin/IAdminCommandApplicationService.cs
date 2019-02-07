using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Categories;
using SCNDISC.Server.Domain.Aggregates.Partners;

namespace SCNDISC.Server.Application.Services.Admin
{
    public interface IAdminCommandApplicationService
    {
        Task<Branch> UpsertPartnerAsync(Branch partner);
        Task<Branch> UpsertBranchAsync(Branch branch);
        Task<Category> UpsertCategoryAsync(Category category);
        Task DeleteBranchAsync(string branchId);
        Task DeletePartnerAsync(string partnerId);
        Task DeleteCategoryAsync(string categoryId);
    }
}
