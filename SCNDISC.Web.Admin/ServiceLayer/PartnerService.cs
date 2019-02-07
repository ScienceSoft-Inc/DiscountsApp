using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SCNDISC.Server.Application.Services.Admin;
using SCNDISC.Web.Admin.ServiceLayer.Extensions;

namespace SCNDISC.Web.Admin.ServiceLayer
{
    public class PartnerService : IPartnersService
    {
        private readonly IAdminQueryApplicationService _adminQueryApplicationService;

        public PartnerService(IAdminQueryApplicationService adminQueryApplicationService)
        {
            _adminQueryApplicationService = adminQueryApplicationService;
        }

        public IEnumerable<TipForm> GetAll()
        {
            var partners = new List<TipForm> {new TipForm {Id = string.Empty, Name_RU = string.Empty}};
            partners.AddRange(Task.Factory.StartNew(() => _adminQueryApplicationService.GetPartnersOnly())
                .Unwrap()
                .GetAwaiter()
                .GetResult().Select(b => b.ToTipForm()));
            return partners;
        }
    }
}