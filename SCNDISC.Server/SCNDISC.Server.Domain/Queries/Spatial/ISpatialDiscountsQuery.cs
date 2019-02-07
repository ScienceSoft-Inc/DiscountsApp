using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Partners;

namespace SCNDISC.Server.Domain.Queries.Spatial
{
    public interface ISpatialDiscountsQuery
    {
        Task<IEnumerable<Branch>> Run(DateTime? syncdate = null);
    }
}
