using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ScnDiscounts.Models.Data;
using ScnDiscounts.Models.WebService.MongoDB;
using ScnDiscounts.Models.Database;
using ScnDiscounts.Views.ContentUI;
using Xamarin.Forms;
using ScnDiscounts.Helpers;

namespace ScnDiscounts.Models
{
    static public class AppData
    {
        public class DiscountContainer
        {
            public DiscountContainer()
            {
                _discountCollection = new List<DiscountData>();
                _mapPinCollection = new ObservableCollection<MapPinData>();

                _db = new LocalDB();
            }

            private bool isConnected;
            private Client ServiceProvider { get; set; }

            private LocalDB _db;
            public LocalDB DB { get { return _db; } }

            private List<DiscountData> _discountCollection;
            public List<DiscountData> DiscountCollection { get { return _discountCollection; } }

            private string _activeMapPinId = "";
            public string ActiveMapPinId
            {
                get { return _activeMapPinId; }
                set { _activeMapPinId = value; }
            }

            private ObservableCollection<MapPinData> _mapPinCollection;
            public ObservableCollection<MapPinData> MapPinCollection { get { return _mapPinCollection; } }

            async private Task<bool> ServiceConnected()
            {
                bool isSuccess = false;

                try
                {
                    ServiceProvider = new Client();
                    
                    isSuccess = await ServiceProvider.CheckConnection();
                }
                catch (Exception)
                {
                    //TODO: Handling exception

                    isSuccess = false;
                }

                isConnected = isSuccess;
                return isConnected;
            }

            async public Task<bool> LoadSpatial()
            {
                bool isSuccess = false;

                try
                {
                    _mapPinCollection.Clear();
                    isSuccess = await ServiceConnected();

                    if (isConnected)
                        isSuccess = await ServiceProvider.GetSpatial();
                }
                catch (Exception)
                {
                    //TODO: Handling exception

                    isSuccess = false;
                }

                return isSuccess;
            }

            async public Task<bool> LoadDiscounts()
            {
                bool isSuccess = false;

                try
                {
                    _discountCollection.Clear();
                    isSuccess = await ServiceConnected();

                    if (isConnected)
                        isSuccess = await ServiceProvider.GetDiscounts();
                }
                catch (Exception)
                {
                    //TODO: Handling exception

                    isSuccess = false;
                }

                return isSuccess;
            }

            async public Task<bool> LoadFullDescription(DiscountData discountData)
            {
                bool isSuccess = false;
                
                try
                {
                    if (discountData.IsFullDescription)
                        isSuccess = true;
                    else
                        if ((isConnected) && (discountData != null))
                            isSuccess = await ServiceProvider.GetPartnerDetail(discountData);
                }
                catch (Exception)
                {
                    //TODO: Handling exception

                    isSuccess = false;
                }

                return isSuccess;
            }

            private bool LoadLocalDB() 
            {
                bool isSuccess = false;

                return isSuccess;
            }

            private void ClearLocalDB()
            {

            }

            async public Task<bool> LoadData(Action<string> actionProcess)
            {
                bool isSuccess = false;

                try
                {
                    var content = new SplashContentUI();
                    actionProcess(content.TxtProcessLoadingData);

                    var dataMidificationHash = AppParameters.Config.DataMidificationHash;
                    if (!await GetModificationHash())
                        return false;

                    bool isNeedUpdating = dataMidificationHash != AppParameters.Config.DataMidificationHash;

                    if (isNeedUpdating)
                    {
                        _db.DiscountClean();
                        IsolatedStorageHelper.ClearStorage();

                        actionProcess(content.TxtProcessLoadMapData);
                        isSuccess = await LoadDiscounts();

                        actionProcess(content.TxtProcessLoadDiscountsData);
                        if (isSuccess)
                            isSuccess = await LoadSpatial();
                    }

                    _db.LoadMapData();
                    _db.LoadDiscount();
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    //TODO: Handling exception

                    isSuccess = false;
                }

                return isSuccess;
            }

            async private Task<bool> GetModificationHash()
            {
                bool isSuccess = false;

                try
                {
                    isSuccess = await ServiceConnected();

                    if (isConnected)
                    {
                        var param = await ServiceProvider.GetModificationHash();
                        AppParameters.Config.DataMidificationHash = param.Value;
                        AppParameters.Config.SaveValue();
                    }
                }
                catch (Exception)
                {
                    //TODO: Handling exception

                    isSuccess = false;
                }

                return isSuccess;
            }
        
        }

        static public DiscountContainer Discount = new DiscountContainer();
    }
}
