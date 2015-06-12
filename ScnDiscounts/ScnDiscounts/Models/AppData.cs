using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ScnDiscounts.Models.Data;
using ScnDiscounts.Models.WebService.MongoDB;

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
                _branchCollection = new ObservableCollection<BranchData>();
            }

            private bool isConnected;
            private Client ServiceProvider { get; set; }

            private List<DiscountData> _discountCollection;
            public List<DiscountData> DiscountCollection { get { return _discountCollection; } }

            private ObservableCollection<BranchData> _branchCollection;
            public ObservableCollection<BranchData> BranchCollection { get { return _branchCollection; } }

            private string _previewImage = "";
            public string PreviewImage
            {
                get { return _previewImage; }
                set { _previewImage = value; }
            }

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

            async public Task<bool> LoadBranchList(DiscountData discountData)
            {
                bool isSuccess = false;
                
                try
                {
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
        }

        static public DiscountContainer Discount = new DiscountContainer();
    }
}
