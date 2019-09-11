using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SCNDISC.Server.Application.Services.Admin;
using SCNDISC.Server.Application.Services.Categories;
using SCNDISC.Server.Application.Services.Partners;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Core.Models.Partner;
using SCNDISC.Server.Core.Mapper;
using Microsoft.AspNetCore.Mvc;
using SCNDISC.Server.Core.Models;
using SCNDISC.Server.Application.Services.Gallery;
using SCNDISC.Server.Infrastructure.Imaging;
using SCNDISC.Server.Application.Services.PartnerRating;

namespace SCNDISC.Server.Core.Controllers.Partners
{
    public class PartnersController : ControllerBase
    {
        private readonly IPartnerApplicationService _partnerApplicationService;
        private readonly IAdminQueryApplicationService _adminQueryApplicationService;
        private readonly ICategoryApplicationService _categoryApplicationService;
        private readonly IGalleryApplicationService _galleryService;
        private readonly IPartnerRatingService _ratingService;
        private readonly IImageConverter _imageConverter;

        public PartnersController(IPartnerApplicationService partnerApplicationService,
            IAdminQueryApplicationService adminQueryApplicationService,
            ICategoryApplicationService сategoryApplicationService,
            IGalleryApplicationService galleryService,
            IPartnerRatingService ratingService,
            IImageConverter imageConverter)
        {
            _partnerApplicationService = partnerApplicationService;
            _adminQueryApplicationService = adminQueryApplicationService;
            _categoryApplicationService = сategoryApplicationService;
            _galleryService = galleryService;
            _ratingService = ratingService;
            _imageConverter = imageConverter;
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
            IEnumerable<CategoryModel> categoriesModels = CategoryMapper.MapToCategoryModels(categories);

            var partners = await _adminQueryApplicationService.GetPartnersOnly(selectedCategories, discountName);
            IEnumerable<DiscountModel> discounts = DiscountMapper.MapToDiscountModel(partners, categoriesModels);
            return discounts;
        }

        [HttpGet]
        [Route("partners/{partnerId}/details")]
        public async Task<PartnerModel> GetPartnerDetailsAsync(string partnerId)
        {
            var categories = await _categoryApplicationService.GetCategoryListAsync();
            var categoriesModels = CategoryMapper.MapToCategoryModels(categories);
            var partnerBranches = await _partnerApplicationService.GetPartnerDetailsAsync(partnerId);
            var partnerGalleryImages = await _galleryService.GetGalleryImageIdsForPartner(partnerId);
            var rating = (await _ratingService.GetPartnerRatingAsync(partnerId)).MapToPartnerStatisticsModel();
            var partner = PartnerMapper.Map(
                partnerBranches?.ToList(), 
                categoriesModels?.ToArray(), 
                partnerGalleryImages?.ToList(),
                rating);
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
            var branches = PartnerMapper.MapToBranches(partner, _imageConverter);
            var partnerBranches = await _partnerApplicationService.GetPartnerDetailsAsync(partner.Id);
            var newPartner = new Branch();
            newPartner = branches.ElementAt(0).PartnerId != null ? branches.First(x => x.Id == x.PartnerId) : branches.ElementAt(0);
            var partnerFirst = await _partnerApplicationService.UpsertPartnerAsync(newPartner);
            foreach (var branch in branches.Where(b => b.Id != b.PartnerId || (b.Id == null && b.PartnerId == null)))
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

        [HttpPost]
        [Authorize(Roles = Roles.AdminRole)]
        [Route("galleryimage")]
        public async Task<ActionResult> PostAsync([FromBody]GalleryImageModel model)
        {
            string result;
            if (ModelState.IsValid)
            {
                var galleryImage = PartnerMapper.MapToGalleryImage(model, _imageConverter);
                result = await _galleryService.AddGalleryImageToPartnerAsync(galleryImage);
            }
            else return BadRequest();

            return new JsonResult(new { id = result }); 
        }

        [Route("galleryimagebase64/{id}")]
        [HttpGet]
        public async Task<string> GetGalleryImageBase64Async(string id)
        {
            return await _galleryService.GetGalleryImageById(id);
        }

        [Route("galleryimage/{id}")]
        [HttpGet]
        public async Task<ActionResult> GetGalleryImageAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            return File(await _galleryService.GetGalleryImage(id), "application/octet-stream");
        }

        [HttpDelete]
        [Authorize(Roles = Roles.AdminRole)]
        [Route("galleryimage/{id}")]
        public async Task<bool> DeleteGalleryImageAsync(string id)
        {
            return await _galleryService.RemoveGalleryImage(id);
        }

        [Route("partners/{id}/galleryimages")]
        [HttpGet]
        public async Task<IEnumerable<string>> GetGalleryImageIdsAsync(string id)
        {
            return await _galleryService.GetGalleryImageIdsForPartner(id);
        }
    }
}