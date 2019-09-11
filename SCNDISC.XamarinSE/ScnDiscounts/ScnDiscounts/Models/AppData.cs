using ScnDiscounts.DependencyInterface;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models.Data;
using ScnDiscounts.Models.Database;
using ScnDiscounts.Models.WebService;
using ScnDiscounts.Views.ContentUI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ScnDiscounts.Models
{
    public static class AppData
    {
        public class DiscountContainer
        {
            private Client ServiceProvider { get; } = new Client();

            private NamedLocker Locker { get; } = new NamedLocker();

            public LocalDb Db { get; } = new LocalDb();

            public string ActiveMapPinId { get; set; }

            public List<CategoryData> CategoryCollection { get; set; }

            public List<DiscountData> DiscountCollection { get; set; }

            public List<MapPinData> MapPinCollection { get; set; }

            public bool IsSynchingImages { get; set; }

            public DiscountContainer()
            {
                CategoryCollection = new List<CategoryData>();
                DiscountCollection = new List<DiscountData>();
                MapPinCollection = new List<MapPinData>();
            }

            public async Task<bool> SyncCategories()
            {
                bool isSuccess;

                try
                {
                    var syncDateUtc = Db.GetCategoryLastSyncDate();
                    isSuccess = await ServiceProvider.GetCategories(syncDateUtc);
                }
                catch (Exception ex)
                {
                    LoggerHelper.WriteException(ex);

                    isSuccess = false;
                }

                return isSuccess;
            }

            public async Task<bool> SyncDiscounts()
            {
                bool isSuccess;

                try
                {
                    var syncDateUtc = Db.GetDiscountLastSyncDate();
                    isSuccess = await ServiceProvider.GetDiscounts(syncDateUtc);
                }
                catch (Exception ex)
                {
                    LoggerHelper.WriteException(ex);

                    isSuccess = false;
                }

                return isSuccess;
            }

            public async Task<bool> SyncSpatial()
            {
                bool isSuccess;

                try
                {
                    var syncDateUtc = Db.GetContactLastSyncDate();
                    isSuccess = await ServiceProvider.GetSpatial(syncDateUtc);
                }
                catch (Exception ex)
                {
                    LoggerHelper.WriteException(ex);

                    isSuccess = false;
                }

                return isSuccess;
            }

            private async Task<bool> SyncPersonalRatings()
            {
                bool isSuccess;

                try
                {
                    var deviceId = DependencyService.Get<IPhoneService>().DeviceId;
                    isSuccess = await ServiceProvider.GetPersonalRatings(deviceId);
                }
                catch (Exception ex)
                {
                    LoggerHelper.WriteException(ex);

                    isSuccess = false;
                }

                return isSuccess;
            }

            public async Task<bool> SyncImages()
            {
                var isSuccess = true;

                IsSynchingImages = true;

                try
                {
                    var discountsId = Db.GetDiscountsId();

                    foreach (var discountId in discountsId)
                    {
                        await SyncImagesFor(discountId);
                    }
                }
                catch (Exception ex)
                {
                    LoggerHelper.WriteException(ex);

                    isSuccess = false;
                }

                IsSynchingImages = false;

                return isSuccess;
            }

            public async Task<bool> SyncImagesFor(string discountId)
            {
                var isSuccess = true;

                var locker = Locker[discountId];
                if (await locker.WaitAsync(0))
                {
                    try
                    {
                        if (Db.TryCheckAnyImage(discountId, out var containsImage, out var containsGallery))
                        {
                            if (!containsImage)
                                await ServiceProvider.GetDiscountImage(discountId);

                            if (!containsGallery)
                                await ServiceProvider.GetDiscountGallery(discountId);
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.WriteException(ex);

                        isSuccess = false;
                    }
                    finally
                    {
                        locker.Release();
                    }
                }

                return isSuccess;
            }

            public async Task<bool> SyncRatingFor(string discountId)
            {
                var isSuccess = true;

                try
                {
                    await ServiceProvider.GetDiscountRating(discountId);
                }
                catch (Exception ex)
                {
                    LoggerHelper.WriteException(ex);

                    isSuccess = false;
                }

                return isSuccess;
            }

            public async Task<bool> SyncData(Action<string> actionProcess)
            {
                var content = new SplashContentUI();

                actionProcess(content.TxtProcessLoadDiscountsData);
                var result = await SyncCategories();
                if (result)
                    result = await SyncDiscounts();

                if (result)
                {
                    actionProcess(content.TxtProcessLoadMapData);
                    result = await SyncSpatial();
                }

                if (result)
                {
                    actionProcess(content.TxtProcessLoadRating);
                    result = await SyncPersonalRatings();
                }

                if (result)
                    SyncImages();

                return result;
            }

            public async Task<bool> SendPersonalRating(PersonalRatingData personalRating)
            {
                bool isSuccess;

                try
                {
                    isSuccess = await ServiceProvider.PostPersonalRating(personalRating);
                }
                catch (Exception ex)
                {
                    LoggerHelper.WriteException(ex);

                    isSuccess = false;
                }

                return isSuccess;
            }


            public async Task<bool> SendFeedback(string name, string comment)
            {
                bool isSuccess;

                try
                {
                    isSuccess = await ServiceProvider.PostFeedback(name, comment);
                }
                catch (Exception ex)
                {
                    LoggerHelper.WriteException(ex);

                    isSuccess = false;
                }

                return isSuccess;
            }
        }

        public static DiscountContainer Discount = new DiscountContainer();
    }
}
