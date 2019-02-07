using Newtonsoft.Json;
using ScnDiscounts.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ScnDiscounts.Models.WebService.MongoDB
{
    public class Client
    {
        private const string RequestCategories = "categories?syncDate={0}";
        private const string RequestDiscounts = "discounts?syncDate={0}";
        private const string RequestSpatial = "spatial/discounts?syncDate={0}";
        private const string RequestPartnerImage = "discounts/{0}/image";
        private const string RequestFeedbacks = "feedbacks";

        protected static HttpClient HttpClient
        {
            get
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(Config.ServerAddress)
                };

                if (!Functions.IsDebug)
                    client.Timeout = new TimeSpan(0, 0, 10);

                return client;
            }
        }

        private static async Task<HttpResponseMessage> Get(string request)
        {
            var result = await HttpClient.GetAsync(request);

            result.EnsureSuccessStatusCode();

            return result;
        }

        private static async Task<HttpResponseMessage> Post(string request, HttpContent content)
        {
            var result = await HttpClient.PostAsync(request, content);

            result.EnsureSuccessStatusCode();

            return result;
        }

        private static async Task<string> GetString(string request)
        {
            var response = await Get(request);
            return await response.Content.ReadAsStringAsync();
        }

        private static async Task<Stream> GetStream(string request)
        {
            var response = await Get(request);
            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<bool> GetCategories(DateTime? syncDateUtc = null)
        {
            bool isSuccess;

            try
            {
                var url = string.Format(RequestCategories, WebUtility.UrlEncode(syncDateUtc?.ToString("O")));

                var token = await GetString(url);
                var branchList = JsonConvert.DeserializeObject<List<object>>(token);

                foreach (var item in branchList)
                {
                    var deserializeBranch = JsonConvert.DeserializeObject<DeserializeCategoryItem>(item.ToString());
                    await AppData.Discount.Db.SyncCategory(deserializeBranch);
                }

                isSuccess = true;
            }
            catch (TaskCanceledException)
            {
                isSuccess = false;
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteException(ex);

                isSuccess = false;
            }

            return isSuccess;
        }

        public async Task<bool> GetDiscounts(DateTime? syncDateUtc = null)
        {
            bool isSuccess;
            
            try
            {
                var url = string.Format(RequestDiscounts, WebUtility.UrlEncode(syncDateUtc?.ToString("O")));

                var token = await GetString(url);
                var branchList = JsonConvert.DeserializeObject<List<object>>(token);

                foreach (var item in branchList)
                {
                    var deserializeBranch = JsonConvert.DeserializeObject<DeserializeBranchItem>(item.ToString());
                    await AppData.Discount.Db.SyncDiscount(deserializeBranch);
                }

                isSuccess = true;
            }
            catch (TaskCanceledException)
            {
                isSuccess = false;
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteException(ex);

                isSuccess = false;
            }

            return isSuccess;
        }

        public async Task<bool> GetSpatial(DateTime? syncDateUtc = null)
        {
            bool isSuccess;

            try
            {
                var url = string.Format(RequestSpatial, WebUtility.UrlEncode(syncDateUtc?.ToString("O")));

                var token = await GetString(url);
                var spatialList = JsonConvert.DeserializeObject<List<object>>(token);
                
                foreach (var item in spatialList)
                {
                    var deserializeBranch = JsonConvert.DeserializeObject<DeserializeBranchItem>(item.ToString());
                    await AppData.Discount.Db.SyncContact(deserializeBranch);
                }

                isSuccess = true;
            }
            catch (TaskCanceledException)
            {
                isSuccess = false;
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteException(ex);

                isSuccess = false;
            }

            return isSuccess;
        }

        public async Task<bool> GetDiscountImage(string documentId)
        {
            bool isSuccess;

            try
            {
                var url = string.Format(RequestPartnerImage, documentId);

                using (var streamImage = await GetStream(url))
                {
                    await AppData.Discount.Db.UpdateDiscountImage(documentId, streamImage);
                }

                isSuccess = true;
            }
            catch (TaskCanceledException)
            {
                isSuccess = false;
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteException(ex);

                isSuccess = false;
            }

            return isSuccess;
        }

        public async Task<bool> PostFeedback(string name, string comment)
        {
            bool isSuccess;

            try
            {
                var feedback = new
                {
                    Name = name,
                    Message = comment
                };

                var json = JsonConvert.SerializeObject(feedback);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                await Post(RequestFeedbacks, content);

                isSuccess = true;
            }
            catch (TaskCanceledException)
            {
                isSuccess = false;
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteException(ex);

                isSuccess = false;
            }

            return isSuccess;
        }
    }
}
