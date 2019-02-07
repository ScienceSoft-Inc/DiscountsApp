using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Queries.Spatial;

namespace SCNDISC.Server.Application.Services.Spatial
{
    public class SpatialApplicationService : ISpatialApplicationService
    {
        private readonly ISpatialDiscountsQuery _discountsQuery;
        private readonly ISpatialProximityQuery _spatialProximityQuery;

        public SpatialApplicationService(ISpatialDiscountsQuery discountsQuery, ISpatialProximityQuery spatialProximityQuery)
        {
            _discountsQuery = discountsQuery;
            _spatialProximityQuery = spatialProximityQuery;
        }

        public async Task<IEnumerable<Branch>> GetSpatialDiscountsDataAsync(DateTime? syncdate = null)
        {
            return await _discountsQuery.Run(syncdate);
        }

        public async Task<IEnumerable<Branch>> GetAllDiscountsInProximityAsync(double longitude, double latitude, int proximity)
        {
            return await _spatialProximityQuery.Run(longitude, latitude, proximity);
        }
    }
}
