using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.Domain.Queries.Discounts;

namespace SCNDISC.Server.Application.Services.Discounts
{
    public class DiscountApplicationService : IDiscountApplicationService
    {
        private readonly IDiscountListQuery _discountListQuery;
	    private readonly IDiscountImageQuery _discountImageQuery;

	    public DiscountApplicationService(IDiscountListQuery discountListQuery, IDiscountImageQuery discountImageQuery)
	    {
		    _discountListQuery = discountListQuery;
		    _discountImageQuery = discountImageQuery;
	    }

        public async Task<IEnumerable<Branch>> GetPartnersDiscountListAsync(DateTime? last = null)
        {
            return await _discountListQuery.GetPartnersDiscountListAsync(last);
        }

	    public async Task<byte[]> GetImageByIdAsync(string id)
	    {
		    var imageBase64 = await _discountImageQuery.GetImageBase64Async(id);
		    if (string.IsNullOrEmpty(imageBase64))
		    {
				return new byte[0];
		    }

		    return Convert.FromBase64String(imageBase64);
	    }
    }
}
