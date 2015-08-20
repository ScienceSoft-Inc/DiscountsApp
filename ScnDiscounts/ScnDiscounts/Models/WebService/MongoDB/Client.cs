using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ScnDiscounts.Models.Data;

namespace ScnDiscounts.Models.WebService.MongoDB
{
    public class Client : IClient
    {
        private const string RequestDiscounts = @"discounts/";
        private const string RequestSpatial = @"spatial/discounts/";
        private const string RequestPartnerDetail = @"partners/{0}/details";
        private const string RequestPartnerTooltip = @"partners/{0}/branches/{1}/tooltip";
        private const string RequestModificationHash = @"modificationhash/";

        async public Task<bool> CheckConnection()
        {
            return true;
        }

        async private Task<string> Get(string request)
        {
            #if DEBUG
            var client = new HttpClient { BaseAddress = new Uri(Config.ServerAddress)};
            #else
            var client = new HttpClient { BaseAddress = new Uri(Config.ServerAddress), Timeout = new TimeSpan(0, 0, 10)};
            #endif

            var response = await client.GetAsync(request);

            return response.Content.ReadAsStringAsync().Result;
        }

        async public Task<bool> GetDiscounts()
        {
            bool isSuccess = false;
            
            try
            {
                AppData.Discount.DiscountCollection.Clear();

                var token = await Get(RequestDiscounts);
                var branchList = JsonConvert.DeserializeObject<List<Object>>(token);

                foreach (var item in branchList)
                {
                    var deserializeBranch = JsonConvert.DeserializeObject<DeserializeBranchItem>(item.ToString());
                    DiscountData discountData = new DiscountData(deserializeBranch);
                    //AppData.Discount.DiscountCollection.Add(discountData);

                    await AppData.Discount.DB.SaveDiscount(discountData);
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        async public Task<bool> GetSpatial()
        {
            bool isSuccess = false;

            try
            {
                var token = await Get(RequestSpatial);
                var spatialList = JsonConvert.DeserializeObject<List<Object>>(token);
                
                foreach (var item in spatialList)
                {
                    var deserializeBranch = JsonConvert.DeserializeObject<DeserializeBranchItem>(item.ToString());
                    //AppData.Discount.MapPinCollection.Add(new MapPinData(deserializeBranch));

                    await AppData.Discount.DB.UpdateDiscount(deserializeBranch);
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        async public Task<bool> GetPartnerDetail(DiscountData discountData)
        {
            bool isSuccess = false;

            try
            {
                var token = await Get(String.Format(RequestPartnerDetail, discountData.DocumentId));
                var branchList = JsonConvert.DeserializeObject<List<Object>>(token);

                foreach (var item in branchList)
                {
                    var deserializeBranch = JsonConvert.DeserializeObject<DeserializeBranchItem>(item.ToString());
                    await AppData.Discount.DB.UpdateDiscount(deserializeBranch);
                }

                isSuccess = true;
            }
            catch (Exception)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        async public Task<bool> GetPartnerTooltip(string partnerId, string branchId)
        {
            bool isSuccess = false;

            try
            {
                var res = await Get(String.Format(RequestPartnerTooltip, partnerId, branchId));

                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        async public Task<ParameterData> GetModificationHash()
        {
            ParameterData parameter = null;
            
            try
            {
                var token = await Get(RequestModificationHash);
                parameter = JsonConvert.DeserializeObject<ParameterData>(token);
            }
            catch (Exception ex)
            {
                parameter = null;
            }

            return parameter;
        }
    }
}
