using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Partners;

namespace SCNDISC.Server.Application.Services.Spatial
{
    public interface ISpatialApplicationService
    {
        Task<IEnumerable<Branch>> GetSpatialDiscountsDataAsync(DateTime? syncdate = null);
        Task<IEnumerable<Branch>> GetAllDiscountsInProximityAsync(double longitude, double latitude, int proximity);
    }
}
