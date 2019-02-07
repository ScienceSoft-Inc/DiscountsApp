using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Partners;

namespace SCNDISC.Server.Application.Services.Discounts
{
    public interface IDiscountApplicationService
    {
        Task<IEnumerable<Branch>> GetPartnersDiscountListAsync(DateTime? last = null);
        Task<byte[]> GetImageByIdAsync(string id);
    }
}
