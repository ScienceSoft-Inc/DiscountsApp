using ScnDiscounts.Helpers;
using ScnDiscounts.Models.Database.Tables;

namespace ScnDiscounts.Models.Data
{
    public class WebAddressData
    {
        public string Url { get; set; }

        public WebAddressTypeEnum Type { get; set; }

        public WebAddressData(WebAddress webAddress)
        {
            Url = webAddress.Url;
            Type = webAddress.Category.GetWebAddressType();
        }
    }
}

