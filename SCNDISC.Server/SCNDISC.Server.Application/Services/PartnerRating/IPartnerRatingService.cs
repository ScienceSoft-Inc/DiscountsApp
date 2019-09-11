using SCNDISC.Server.Domain.Aggregates.Partners;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCNDISC.Server.Application.Services.PartnerRating
{
    public interface IPartnerRatingService
    {
        Task<Rating> UpsertRatingAsync(Rating category);
        Task<IEnumerable<Rating>> GetPartnerRatingAsync(string partnerId);
        Task<Rating> GetRatingByIdAsync(string id);
        Task<IEnumerable<Rating>> GetRatingsByDeviceIdAsync(string deviceId);
        Task<bool> IsValidIdDeviceIdPartnerIdFields(Rating updatingRating);
    }
}
