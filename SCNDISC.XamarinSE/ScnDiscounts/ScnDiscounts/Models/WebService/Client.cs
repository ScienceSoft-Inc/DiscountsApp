using Refit;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models.Data;
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
            var httpClient = Functions.IsDebug ? new HttpClient(new LoggingHandler(), false) : new HttpClient();
            httpClient.BaseAddress = new Uri(WebService.ApiService.ServerAddress);

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
                var categories = await ApiService.GetCategories(syncParams).RequestAsync();

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
                var discounts = await ApiService.GetDiscounts(syncParams).RequestAsync();

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
                var spatials = await ApiService.GetSpatials(syncParams).RequestAsync();

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

        public async Task<bool> GetPersonalRatings(string deviceId)
        {
            bool isSuccess;

            try
            {
                var personalRatings = await ApiService.GetPersonalRatings(deviceId).RequestAsync();

                foreach (var personalRating in personalRatings)
                {
                    await AppData.Discount.Db.SyncPersonalRating(personalRating);
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
                var content = await ApiService.GetDiscountImage(documentId).BytesAsync();
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
                var images = await ApiService.GetDiscountGalleryImages(documentId).RequestAsync();

                foreach (var imageId in images)
                {
                    var content = await ApiService.GetDiscountGalleryImage(imageId).BytesAsync();
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

        public async Task<bool> GetDiscountRating(string documentId)
        {
            bool isSuccess;

            try
            {
                var rating = await ApiService.GetDiscountRating(documentId).RequestAsync();
                rating.Id = documentId;

                await AppData.Discount.Db.SyncDiscountRating(rating);

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

        public async Task<bool> PostPersonalRating(PersonalRatingData personalRating)
        {
            bool isSuccess;

            try
            {
                var rating = new DeserializePersonalRating
                {
                    Id = personalRating.DocumentId,
                    DeviceId = personalRating.DeviceId,
                    PartnerId = personalRating.PartnerId,
                    Mark = personalRating.Mark
                };

                rating = await ApiService.PostPersonalRating(rating).RequestAsync();

                await AppData.Discount.Db.SyncPersonalRating(rating);

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

                await ApiService.PostFeedback(feedback).RequestAsync();

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
