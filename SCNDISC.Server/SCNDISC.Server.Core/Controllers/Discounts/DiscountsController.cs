using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Application.Services.Discounts;
using SCNDISC.Server.Domain.Aggregates.Partners;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace SCNDISC.Server.Core.Controllers.Discounts
{
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountApplicationService _discountApplicationService;

        public DiscountsController(IDiscountApplicationService discountApplicationService)
        {
            _discountApplicationService = discountApplicationService;
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
	}
}