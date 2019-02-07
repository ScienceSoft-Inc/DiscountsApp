using System.Threading.Tasks;

namespace SCNDISC.Server.Domain.Commands.Partners
{
    public interface IDeleteBranchCommand
    {
        Task DeleteBranchAsync(string branchId);
    }
}
