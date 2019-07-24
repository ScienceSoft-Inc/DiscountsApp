using Refit;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models.WebService.MongoDB;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ScnDiscounts.Models.WebService
{
    public class Client
    {
        protected readonly IApiService ApiService; 

        public Client()
        {
            var httpClient = new HttpClient(new LoggingHandler(), false)
            {
                BaseAddress = new Uri(WebService.ApiService.ServerAddress)
            };

            if (!Functions.IsDebug)
                httpClient.Timeout = new TimeSpan(0, 0, 10);

            var refitSettings = new RefitSettings
            {
                UrlParameterFormatter = new CustomUrlParameterFormatter()
            };

            ApiService = RestService.For<IApiService>(httpClient, refitSettings);
        }

        public async Task<bool> GetCategories(DateTime? syncDateUtc = null)
        {
            bool isSuccess;

            try
            {
                var syncParams = new SyncParams(syncDateUtc);
                var categories = await ApiService.GetCategories(syncParams);

                foreach (var category in categories)
                {
                    await AppData.Discount.Db.SyncCategory(category);
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
                var syncParams = new SyncParams(syncDateUtc);
                var discounts = await ApiService.GetDiscounts(syncParams);

                foreach (var discount in discounts)
                {
                    await AppData.Discount.Db.SyncDiscount(discount);
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
                var syncParams = new SyncParams(syncDateUtc);
                var spatials = await ApiService.GetSpatials(syncParams);

                foreach (var spatial in spatials)
                {
                    await AppData.Discount.Db.SyncContact(spatial);
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
                var response = await ApiService.GetDiscountImage(documentId);
                var content = await response.ReadAsByteArrayAsync();

                if (content != null)
                {
                    using (var stream = new MemoryStream(content))
                    {
                        await AppData.Discount.Db.UpdateDiscountImage(documentId, stream);
                    }
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

        public async Task<bool> GetDiscountGallery(string documentId)
        {
            bool isSuccess;

            try
            {
                var images = await ApiService.GetDiscountGalleryImages(documentId);

                foreach (var imageId in images)
                {
                    var response = await ApiService.GetDiscountGalleryImage(imageId);
                    var content = await response.ReadAsByteArrayAsync();

                    if (content != null)
                    {
                        using (var stream = new MemoryStream(content))
                        {
                            await AppData.Discount.Db.UpdateDiscountGalleryImage(documentId, imageId, stream);
                        }
                    }
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
                var feedback = new DeserializeFeedback
                {
                    Name = name,
                    Message = comment
                };

                await ApiService.PostFeedback(feedback);

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
