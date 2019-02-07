using ScnDiscounts.Helpers;
using ScnDiscounts.Models.Data;
using ScnDiscounts.Models.Database;
using ScnDiscounts.Models.WebService.MongoDB;
using ScnDiscounts.Views.ContentUI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScnDiscounts.Models
{
    public static class AppData
    {
        public class DiscountContainer
        {
            public DiscountContainer()
            {
                CategoryCollection = new List<CategoryData>();
                DiscountCollection = new List<DiscountData>();
                MapPinCollection = new List<MapPinData>();

                Db = new LocalDb();
            }

            private Client ServiceProvider { get; } = new Client();

            public LocalDb Db { get; }

            public List<CategoryData> CategoryCollection { get; set; }
            public List<DiscountData> DiscountCollection { get; set; }
            public List<MapPinData> MapPinCollection { get; set; }

            public string ActiveMapPinId { get; set; }

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

            public async Task<bool> SyncImages()
            {
                var isSuccess = true;

                try
                {
                    var discountIds = Db.GetDiscountsWithoutImages();

                    foreach (var discountId in discountIds)
                    {
                        if (isSuccess)
                            isSuccess = await ServiceProvider.GetDiscountImage(discountId);
                    }
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

                actionProcess(content.TxtProcessLoadMapData);
                if (result)
                    result = await SyncSpatial();

                if (result)
                    SyncImages();

                return result;
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
