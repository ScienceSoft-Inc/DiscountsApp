using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SCNDISC.Server.Application.Services.Admin;
using SCNDISC.Server.Application.Services.Partners;
using SCNDISC.Web.Admin.ServiceLayer.Extensions;

namespace SCNDISC.Web.Admin.ServiceLayer
{
    public class TipService : ITipsService
    {
        private readonly IAdminCommandApplicationService _adminCommandApplicationService;
        private readonly IAdminQueryApplicationService _adminQueryApplicationService;
        private readonly IPartnerApplicationService _partnerApplicationService;

        public TipService(IAdminQueryApplicationService adminQueryApplicationService,
            IAdminCommandApplicationService adminCommandApplicationService,
            IPartnerApplicationService partnerApplicationService)
        {
            _adminQueryApplicationService = adminQueryApplicationService;
            _adminCommandApplicationService = adminCommandApplicationService;
            _partnerApplicationService = partnerApplicationService;
        }

        public TipForm Save(TipForm tip)
        {
            if (tip.IsPartner())
            {
                return Task.Factory.StartNew(() => _adminCommandApplicationService.UpsertPartnerAsync(tip.ToBranch()))
                    .Unwrap()
                    .GetAwaiter()
                    .GetResult().ToTipForm();
            }
            return Task.Factory.StartNew(() => _adminCommandApplicationService.UpsertBranchAsync(tip.ToBranch()))
                .Unwrap()
                .GetAwaiter()
                .GetResult().ToTipForm();
        }

        public void Delete(TipForm tip)
        {
            if (tip.IsPartner())
            {
                Task.Factory.StartNew(() => _adminCommandApplicationService.DeletePartnerAsync(tip.Id))
                    .Unwrap()
                    .GetAwaiter()
                    .GetResult();
            }
            else
            {
                Task.Factory.StartNew(() => _adminCommandApplicationService.DeleteBranchAsync(tip.Id))
                    .Unwrap()
                    .GetAwaiter()
                    .GetResult();
            }
        }

        public IEnumerable<TipForm> GetById(string id)
        {
            return Task.Factory.StartNew(() => _partnerApplicationService.GetPartnerDetailsAsync(id))
                .Unwrap()
                .GetAwaiter()
                .GetResult().Select(x => x.ToTipForm());
        }

        public IEnumerable<TipForm> GetAll(string[] selectedCategories, string partnerName)
        {
            return Task.Factory.StartNew(() => _adminQueryApplicationService.GetPartnersOnly(selectedCategories, partnerName))
                .Unwrap()
                .GetAwaiter()
                .GetResult().Select(b => b.ToTipForm());
        }

        public void RemoveAllBranches(string partnerId)
        {
            GetById(partnerId).Where(x => x.Id != partnerId).ToList().ForEach(Delete);
        }
    }
}