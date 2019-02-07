using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Partners;

namespace SCNDISC.Server.Domain.Commands.Partners
{
    public interface IUpsertPartnerCommand
    {
        Task<Branch> UpsertPartnerAsync(Branch partner);
    }
}
