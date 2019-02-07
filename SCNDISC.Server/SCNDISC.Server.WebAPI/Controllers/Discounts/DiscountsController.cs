using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using SCNDISC.Server.Application.Services.Discounts;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.WebAPI.Infrastructure.Http.Results;
using System.Web.Http.Cors;

namespace SCNDISC.Server.WebAPI.Controllers.Discounts
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DiscountsController : ApiController
    {
        private readonly IDiscountApplicationService _discountApplicationService;

        public DiscountsController(IDiscountApplicationService discountApplicationService)
        {
            _discountApplicationService = discountApplicationService;
        }

        [Route("discounts/{syncdate?}")]
        [HttpGet]
        public async Task<IEnumerable<Branch>> GetAllDiscountsAsync([FromUri]DateTime? syncdate = null)
        {
            return await _discountApplicationService.GetPartnersDiscountListAsync(syncdate);
        }

	    [Route("discounts/{id}/image")]
	    [HttpGet]
	    public async Task<IHttpActionResult> GetImageAsync(string id)
	    {
		    if (string.IsNullOrEmpty(id))
		    {
			    return BadRequest();
		    }

		    return this.File(await _discountApplicationService.GetImageByIdAsync(id));
	    }
	}
}