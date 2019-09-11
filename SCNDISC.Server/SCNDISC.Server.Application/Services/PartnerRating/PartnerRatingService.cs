using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Commands.Ratings;
using SCNDISC.Server.Domain.Queries.PartnerRating;

namespace SCNDISC.Server.Application.Services.PartnerRating
{
    public class PartnerRatingService : IPartnerRatingService
    {
        private readonly IPartnerRatingQuery _partnerRatingQuery;
        private readonly IDeviceRatingsQuery _deviceRatingsQuery;
        private readonly IRatingByIdQuery _ratingByIdQuery;
        private readonly IUpsertRatingCommand _upsertRatingCommand;
        public PartnerRatingService(
            IPartnerRatingQuery partnerRatingQuery, 
            IRatingByIdQuery ratingByIdQuery,
            IDeviceRatingsQuery deviceRatingsQuery,
        IUpsertRatingCommand upsertRatingCommand)
        {
            _partnerRatingQuery = partnerRatingQuery;
            _ratingByIdQuery = ratingByIdQuery;
            _deviceRatingsQuery = deviceRatingsQuery;
            _upsertRatingCommand = upsertRatingCommand;
        }

        public async Task<IEnumerable<Rating>> GetPartnerRatingAsync(string partnerId)
        {
            return await _partnerRatingQuery.RunAsync(partnerId);
        }

        public async Task<IEnumerable<Rating>> GetRatingsByDeviceIdAsync(string deviceId)
        {
            return await _deviceRatingsQuery.RunAsync(deviceId);
        }

        public async Task<Rating> UpsertRatingAsync(Rating rating)
        {
            var ratingToUpsert = await _upsertRatingCommand.UpsertRatingAsync(rating);

            return await _ratingByIdQuery.RunAsync(ratingToUpsert.Id);
        }

        public async Task<Rating> GetRatingByIdAsync(string id)
        {
            return await _ratingByIdQuery.RunAsync(id);
        }

        public async Task<bool> IsValidIdDeviceIdPartnerIdFields(Rating updatingRating)
        {
            var currentRatingInDb =  await GetRatingByIdAsync(updatingRating?.Id);
            if (currentRatingInDb != null)
            {
                if (updatingRating.DeviceId == currentRatingInDb.DeviceId && updatingRating.PartnerId == currentRatingInDb.PartnerId)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
