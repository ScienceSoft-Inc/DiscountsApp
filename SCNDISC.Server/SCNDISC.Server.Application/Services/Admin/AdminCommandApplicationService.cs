using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Categories;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Commands.Categories;
using SCNDISC.Server.Domain.Commands.Partners;
using SCNDISC.Server.Domain.Queries.Parameters;

namespace SCNDISC.Server.Application.Services.Admin
{
    public class AdminCommandApplicationService : IAdminCommandApplicationService
    {
        private readonly IUpsertPartnerCommand _upsertPartnerCommand;
        private readonly IUpsertBranchCommand _upsertBranchCommand;
        private readonly IUpsertCategoryCommand _upsertCategoryCommand;
        private readonly IDeleteBranchCommand _deleteBranchCommand;
        private readonly IDeletePartnerCommand _deletePartnerCommand;
        private readonly IDeleteCategoryCommand _deleteCategoryCommand;
        private readonly IUpdateModificationHashQuery _updateModificationHashQuery;

        public AdminCommandApplicationService(IUpsertPartnerCommand upsertPartnerCommand,
            IUpsertBranchCommand upsertBranchCommand, IUpsertCategoryCommand upsertCategoryCommand,
            IDeleteBranchCommand deleteBranchCommand, IDeletePartnerCommand deletePartnerCommand,
            IDeleteCategoryCommand deleteCategoryCommand, IUpdateModificationHashQuery updateModificationHashQuery)
        {
            _upsertPartnerCommand = upsertPartnerCommand;
            _upsertBranchCommand = upsertBranchCommand;
            _upsertCategoryCommand = upsertCategoryCommand;
            _deleteBranchCommand = deleteBranchCommand;
            _deletePartnerCommand = deletePartnerCommand;
            _deleteCategoryCommand = deleteCategoryCommand;
            _updateModificationHashQuery = updateModificationHashQuery;
        }

        public async Task<Branch> UpsertPartnerAsync(Branch partner)
        {
            await _updateModificationHashQuery.Run();
            return await _upsertPartnerCommand.UpsertPartnerAsync(partner);
        }

        public async Task<Branch> UpsertBranchAsync(Branch branch)
        {
            await _updateModificationHashQuery.Run();
            return await _upsertBranchCommand.UpsertBranchAsync(branch);
        }

        public async Task<Category> UpsertCategoryAsync(Category category)
        {
            await _updateModificationHashQuery.Run();
            return await _upsertCategoryCommand.UpsertCategoryAsync(category);
        }

        public async Task DeleteBranchAsync(string branchId)
        {
            await _updateModificationHashQuery.Run();
            await _deleteBranchCommand.DeleteBranchAsync(branchId);
        }

        public async Task DeletePartnerAsync(string partnerId)
        {
            await _updateModificationHashQuery.Run();
            await _deletePartnerCommand.DeletePartnerAsync(partnerId);
        }

        public async Task DeleteCategoryAsync(string categoryId)
        {
            await _updateModificationHashQuery.Run();
            await _deleteCategoryCommand.DeleteCategoryAsync(categoryId);
        }
    }
}
