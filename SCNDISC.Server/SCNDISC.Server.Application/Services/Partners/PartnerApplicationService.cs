using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Commands.Parameters;
using SCNDISC.Server.Domain.Commands.Partners;
using SCNDISC.Server.Domain.Queries.Partners;

namespace SCNDISC.Server.Application.Services.Partners
{
    public class PartnerApplicationService : IPartnerApplicationService
    {
        private readonly IPartnerDetailsQuery _partnerDetailsQuery;
        private readonly IBranchToolTipQuery _branchToolTipQuery;
        private readonly IActivePartnerIdsQuery _activePartnerIdsQuery;
        private readonly IUpsertBranchCommand _upsertBranchCommand;
        private readonly IDeleteBranchCommand _deleteBranchCommand;
        private readonly IUpsertPartnerCommand _upsertPartnerCommand;
        private readonly IDeletePartnerCommand _deletePartnerCommand;

        public PartnerApplicationService(
            IPartnerDetailsQuery partnerDetailsQuery, 
            IBranchToolTipQuery branchToolTipQuery,
            IActivePartnerIdsQuery activePartnerIdsQuery,
            IUpsertBranchCommand upsertBranchCommand, 
            IDeleteBranchCommand deleteBranchCommand, 
            IUpsertPartnerCommand upsertPartnerCommand, 
            IDeletePartnerCommand deletePartnerCommand)
        {
            _partnerDetailsQuery = partnerDetailsQuery;
            _branchToolTipQuery = branchToolTipQuery;
            _activePartnerIdsQuery = activePartnerIdsQuery;
            _upsertBranchCommand = upsertBranchCommand;
            _deleteBranchCommand = deleteBranchCommand;
            _upsertPartnerCommand = upsertPartnerCommand;
            _deletePartnerCommand = deletePartnerCommand;
        }

        public async Task<IEnumerable<Branch>> GetPartnerDetailsAsync(string partnerId)
        {
            return await _partnerDetailsQuery.Run(partnerId);
        }

        public async Task<Branch> GetBranchToolTipAsync(string partnerId, string branchId)
        {
            return await _branchToolTipQuery.Run(partnerId, branchId);
        }

        public async Task<IList<string>> GetActivePartnerIdsAsync()
        {
            return await _activePartnerIdsQuery.RunAsync();
        }

        public async Task<Branch> UpsertBranchAsync(Branch branch)
        {
            return await _upsertBranchCommand.UpsertBranchAsync(branch);
        }

        public async Task<Branch> UpsertPartnerAsync(Branch branch)
        {
            return await _upsertPartnerCommand.UpsertPartnerAsync(branch);
        }

        public async Task DeleteBranchAsync(string partnerId)
        {
            await _deleteBranchCommand.DeleteBranchAsync(partnerId);
        }

        public async Task DeletePartnerAsync(string partnerId)
        {
            await _deletePartnerCommand.DeletePartnerAsync(partnerId);
        }
    }
}
