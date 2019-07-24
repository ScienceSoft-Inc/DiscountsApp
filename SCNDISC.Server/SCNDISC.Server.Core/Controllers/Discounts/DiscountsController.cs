using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Application.Services.Discounts;
using SCNDISC.Server.Domain.Aggregates.Partners;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using SCNDISC.Server.Application.Services.Gallery;

namespace SCNDISC.Server.Core.Controllers.Discounts
{
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountApplicationService _discountApplicationService;
        private readonly IGalleryApplicationService _galleryService;

        public DiscountsController(IDiscountApplicationService discountApplicationService, IGalleryApplicationService galleryService)
        {
            _discountApplicationService = discountApplicationService;
            _galleryService = galleryService;
        }

        [Route("discounts/{syncdate?}")]
        [HttpGet]
        public async Task<IEnumerable<Branch>> GetAllDiscountsAsync([FromQuery]DateTime? syncdate = null)
        {
            return await _discountApplicationService.GetPartnersDiscountListAsync(syncdate);
        }

	    [Route("discounts/{id}/image")]
	    [HttpGet]
	    public async Task<ActionResult> GetImageAsync(string id)
	    {
	        if (string.IsNullOrEmpty(id))
	        {
	            return BadRequest();
	        }

            return File(await _discountApplicationService.GetImageByIdAsync(id), "application/octet-stream");
	    }

        [Route("discounts/{id}/logo")]
        [HttpGet]
        public async Task<ActionResult> GetLogoAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            return File(await _discountApplicationService.GetLogoByIdAsync(id), "application/octet-stream");
        }
    }
}