using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Cors;
using System.Threading.Tasks;
using System.Web.Http;
using SCNDISC.Server.Application.Services.Admin;
using SCNDISC.Server.Application.Services.Categories;
using SCNDISC.Server.Application.Services.Partners;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.WebAPI.Models.Partner;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SCNDISC.Server.WebAPI.Controllers.Partners
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PartnersController : ApiController
    {
        private const string adminRole = @"MAIN\prj-SCN.SCNDISC-Admininstrators";
        private readonly IPartnerApplicationService _partnerApplicationService;
        private readonly IAdminQueryApplicationService _adminQueryApplicationService;
        private readonly ICategoryApplicationService _categoryApplicationService;

        public PartnersController(IPartnerApplicationService partnerApplicationService,
            IAdminQueryApplicationService adminQueryApplicationService,
            ICategoryApplicationService сategoryApplicationService)
        {
            _partnerApplicationService = partnerApplicationService;
            _adminQueryApplicationService = adminQueryApplicationService;
            _categoryApplicationService = сategoryApplicationService;
        }

        [HttpGet]
        [Route("partners")]
        public async Task<IEnumerable<DiscountModel>> GetAll([FromUri] string[] selectedCategories,
            string discountName = null)
        {
            var categories = await _categoryApplicationService.GetCategoryListAsync();
            IEnumerable<CategoryModel> categoriesModels = Mapper.CategoryMapper.MapToCategoryModels(categories);

            var partners = await _adminQueryApplicationService.GetPartnersOnly(selectedCategories, discountName);
            IEnumerable<DiscountModel> discounts = Mapper.DiscountMapper.MapToDiscountModel(partners, categoriesModels);
            return discounts;
        }

        [HttpGet]
        [Route("partners/{partnerId}/details")]
        public async Task<PartnerModel> GetPartnerDetailsAsync(string partnerId)
        {
            var categories = await _categoryApplicationService.GetCategoryListAsync();
            var categoriesModels = Mapper.CategoryMapper.MapToCategoryModels(categories);
            var partnerBranches = await _partnerApplicationService.GetPartnerDetailsAsync(partnerId);
            var partner = Mapper.PartnerMapper.Map(partnerBranches?.ToList(), categoriesModels?.ToArray());
            return partner;
        }

        [HttpGet]
        [Route("partners/{partnerId}/branches/{branchId}/tooltip")]
        public async Task<Branch> GetBranchToolTipAsync(string partnerId, string branchId)
        {
            return await _partnerApplicationService.GetBranchToolTipAsync(partnerId, branchId);
        }

        [HttpPost]
        [Authorize(Roles = adminRole)]
        [Route("partners")]
        public async Task<HttpResponseMessage> SavePartnerAsync(PartnerModel partner)
        {
            var branches = Mapper.PartnerMapper.MapToBranches(partner);
            var partnerBranches = await _partnerApplicationService.GetPartnerDetailsAsync(partner.Id);
            var newFirstPartner = new Branch();
            if (branches.ElementAt(0).PartnerId != null)
            {
                newFirstPartner = branches.First(x => x.Id == x.PartnerId);
            }
            else
            {
                newFirstPartner = branches.ElementAt(0);
            }
            var partnerFirst = await _partnerApplicationService.UpsertPartnerAsync(newFirstPartner);
            foreach (var branch in branches)
            {
                branch.PartnerId = partnerFirst.PartnerId;
                var qwerty = await _partnerApplicationService.UpsertBranchAsync(branch);
            }
            foreach (var branch in partnerBranches)
            {
                if (branches.FirstOrDefault(x => x?.Id == branch.Id) == null)
                {
                    await _partnerApplicationService.DeleteBranchAsync(branch.Id);
                }
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpDelete]
       // [Authorize(Roles = adminRole)]
        [Route("partners/{partnerId}")]
        public async Task<HttpResponseMessage> DeletePartnerAsync(string partnerId)
        {
            await _partnerApplicationService.DeletePartnerAsync(partnerId);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }


        //[HttpPost]
        //[Authorize(Roles = adminRole)]
        //[Route("branches/")]
        //public async Task<Branch> SaveBranchAsync(Branch branch)
        //{
        //    return await _partnerApplicationService.UpsertBranchAsync(branch);
        //}

        //[HttpDelete]
        //[Authorize(Roles = adminRole)]
        //[Route("branches/{branchId}")]
        //public async Task<HttpResponseMessage> DeleteBranchAsync(string branchId)
        //{
        //    await _partnerApplicationService.DeleteBranchAsync(branchId);
        //    return new HttpResponseMessage(HttpStatusCode.OK);
        //}

        //[HttpDelete]
        //[Authorize(Roles = adminRole)]
        //[Route("partners/{partnerId}")]
        //public async Task<HttpResponseMessage> DeletePartnerAsync(string partnerId)
        //{
        //    await _partnerApplicationService.DeletePartnerAsync(partnerId);
        //    return new HttpResponseMessage(HttpStatusCode.OK);
        //}
    }
}