using Microsoft.AspNetCore.Mvc;
using SCNDISC.Server.Application.Services.PartnerRating;
using SCNDISC.Server.Application.Services.Partners;
using SCNDISC.Server.Core.Mapper;
using SCNDISC.Server.Core.Models;
using SCNDISC.Server.Core.Models.Rating;
using System;
using System.Threading.Tasks;

namespace SCNDISC.Server.Core.Controllers
{
    public class PartnerRatingController : ControllerBase
    {
        private readonly IPartnerRatingService _partnerRatingService;
        private readonly IPartnerApplicationService _partnerService;

        public PartnerRatingController(IPartnerRatingService partnerRatingService, IPartnerApplicationService partnerService)
        {
            _partnerRatingService = partnerRatingService;
            _partnerService = partnerService;
        }

        [HttpPost]
        //TODO authorization
        [Route("partner/rating")]
        public async Task<ActionResult> PostAsync([FromBody]PartnerRatingModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(model.Id))
                    {
                        // in case of updating
                        if (!await _partnerRatingService.IsValidIdDeviceIdPartnerIdFields(model.MapToPartnerRating()))
                        {
                            return BadRequest("Uncorrent all or some of this fields: Id, DeviceId, ParnterId");
                        }
                    }

                    var activeIds = await _partnerService.GetActivePartnerIdsAsync();
                    if (!activeIds.Contains(model.PartnerId))
                    {
                        return BadRequest("This ParnterId doesn't exist");
                    }

                    var result = await _partnerRatingService.UpsertRatingAsync(model.MapToPartnerRating());

                    return Ok(result.MapToPartnerRatingModel());
                }
                catch (Exception ex)
                {
                    return BadRequest(new ErrorModel { Error = ex.Message });
                }
            }

            return BadRequest(ModelState);
        }

        [HttpGet]
        [Route("partner/rating/{partnerId}")]
        public async Task<ActionResult> GetPartnerRatingStatisticsAsync(string partnerId)
        {
            if (!string.IsNullOrEmpty(partnerId))
            {
                try
                {
                    var ratingData = await _partnerRatingService.GetPartnerRatingAsync(partnerId);

                    return Ok(ratingData.MapToPartnerStatisticsModel());
                }
                catch (Exception ex)
                {
                    return BadRequest(new ErrorModel { Error = ex.Message });
                }
            }

            return BadRequest(new ErrorModel { Error = "Id should be provided" });
        }

        [HttpGet]
        [Route("device/{deviceId}/ratings")]
        public async Task<ActionResult> GetRatingsForDeviceAsync(string deviceId)
        {
            if (!string.IsNullOrEmpty(deviceId))
            {
                var deviceRatings = await _partnerRatingService.GetRatingsByDeviceIdAsync(deviceId);

                return Ok(deviceRatings.MapToPartnerRatingModels());
            }

            return BadRequest(new ErrorModel { Error = "Id should be provided" });
        }
    }
}
