using System.Collections.Generic;
using SCNDISC.Server.Domain.Aggregates.Partners;
using SCNDISC.Server.WebAPI.Models.Partner;

namespace SCNDISC.Server.WebAPI.Mapper
{
    public abstract class WebAddressMapper
    {
        public static IEnumerable<WebAddressModel> MapToWebAddressModels(IEnumerable<WebAddress> webAddresses)
        {
            List<WebAddressModel> webAddressModels = new List<WebAddressModel>();
            foreach (var web in webAddresses)
            {
                var webAdr = FromWebAddressToWebAddressModel(web);
                webAddressModels.Add(webAdr);
            }

            return webAddressModels;
        }

        public static IEnumerable<WebAddress> MapToWebAddresses(IEnumerable<WebAddressModel> webAddressModels)
        {
            List<WebAddress> webAddresses = new List<WebAddress>();
            foreach (var web in webAddressModels)
            {
                var webAdr = FromWebAddressModelToWebAddress(web);
                webAddresses.Add(webAdr);
            }

            return webAddresses;
        }

        private static WebAddressModel FromWebAddressToWebAddressModel(WebAddress webAddress)
        {
            WebAddressModel webAddressModel = new WebAddressModel();
            webAddressModel.Id = webAddress.Id;
            webAddressModel.SocialNetwork = webAddress.Category;
            webAddressModel.URL = webAddress.Url;
            return webAddressModel;
        }

        private static WebAddress FromWebAddressModelToWebAddress(WebAddressModel webAddressModel)
        {
            WebAddress webAddress = new WebAddress();
            webAddress.Id = webAddressModel.Id;
            webAddress.Category = webAddressModel.SocialNetwork;
            webAddress.Url = webAddressModel.URL;
            return webAddress;
        }


    }
}