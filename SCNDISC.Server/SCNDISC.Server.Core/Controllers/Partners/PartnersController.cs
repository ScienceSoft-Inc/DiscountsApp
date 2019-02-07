using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
//using System.Web.Http.Cors;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SCNDISC.Server.Application.Services.Admin;
using SCNDISC.Server.Application.Services.Categories;
using SCNDISC.Server.Application.Services.Partners;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Core.Models.Partner;
using SCNDISC.Server.Core.Mapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using SCNDISC.Server.Core.Models;

namespace SCNDISC.Server.Core.Controllers.Partners
{
    public class PartnersController : ControllerBase
    {
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
        public async Task<IEnumerable<DiscountModel>> GetAll([FromQuery] string[] selectedCategories,
            string discountName = null)
        {
            if (selectedCategories.Length == 0)
            {
                selectedCategories = null;
            }
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
        [Authorize(Roles = Roles.AdminRole)]
        [Route("partners")]
        public async Task<HttpResponseMessage> SavePartnerAsync([FromBody] PartnerModel partner)
        {
            var branches = Mapper.PartnerMapper.MapToBranches(partner);
            var partnerBranches = await _partnerApplicationService.GetPartnerDetailsAsync(partner.Id);
            var newPartner = new Branch();
            newPartner = branches.ElementAt(0).PartnerId != null ? branches.First(x => x.Id == x.PartnerId) : branches.ElementAt(0);
            var partnerFirst = await _partnerApplicationService.UpsertPartnerAsync(newPartner);
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
        [Authorize(Roles = Roles.AdminRole)]
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