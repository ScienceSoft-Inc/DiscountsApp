using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Partners;

namespace SCNDISC.Server.Domain.Queries.Discounts
{
    public interface IDiscountListQuery
    {
        Task<IEnumerable<Branch>> GetPartnersDiscountListAsync(DateTime? syncdate = null);
    }
}
