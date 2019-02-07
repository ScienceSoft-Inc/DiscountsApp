using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Partners;

namespace SCNDISC.Server.Application.Services.Admin
{
    public interface IAdminQueryApplicationService
    {
        Task<IEnumerable<Branch>> GetPartnersOnly(string[] selectedCategories = null, string partnerName = null);
    }
}
