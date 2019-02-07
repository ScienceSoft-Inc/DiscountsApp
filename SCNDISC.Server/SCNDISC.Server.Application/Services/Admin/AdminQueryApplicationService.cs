using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Queries.Partners;

namespace SCNDISC.Server.Application.Services.Admin
{
    public class AdminQueryApplicationService : IAdminQueryApplicationService
    {
        private readonly IPartnersOnlyQuery _partnersOnlyQuery;

        public AdminQueryApplicationService(IPartnersOnlyQuery partnersOnlyQuery)
        {
            _partnersOnlyQuery = partnersOnlyQuery;
        }

        public async Task<IEnumerable<Branch>> GetPartnersOnly(string[] selectedCategories = null, string partnerName = null)
        {
            return await _partnersOnlyQuery.Run(selectedCategories, partnerName);
        }
    }
}
