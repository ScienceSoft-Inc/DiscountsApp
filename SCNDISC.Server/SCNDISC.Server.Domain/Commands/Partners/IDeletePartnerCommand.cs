using System.Threading.Tasks;

namespace SCNDISC.Server.Domain.Commands.Partners
{
    public interface IDeletePartnerCommand
    {
        Task DeletePartnerAsync(string partnerId);
    }
}
