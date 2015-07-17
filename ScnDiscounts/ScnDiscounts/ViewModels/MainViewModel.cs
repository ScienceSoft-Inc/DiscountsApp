using System;
using System.Collections.ObjectModel;
using ScnDiscounts.Control;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models;
using ScnDiscounts.Models.Data;
using ScnDiscounts.Views;
using ScnDiscounts.Views.ContentUI;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Threading.Tasks;
using ScnPage.Plugin.Forms;

namespace ScnDiscounts.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        private MainContentUI contentUI
        {
            get { return (MainContentUI)ContentUI; }
        }

        protected override void InitProperty()
        {
            ViewPage.Appearing += ViewPage_Appearing;
            ViewPage.Appearing += ViewPage_InitAfterAppearing;
            _menuItemList = new ObservableCollection<MenuSideBarItem>();
            InitMenuItem();
        }

        private void ViewPage_InitAfterAppearing(object sender, EventArgs e)
        {
            ViewPage.Appearing -= ViewPage_InitAfterAppearing;

            double minskLat = 53.904841;
            double minskLong = 27.55327;
            (ViewPage as MainPage).MapLocation.MoveToRegion(MapSpan.FromCenterAndRadius(
                                                       new Position(minskLat, minskLong),
                                                       Distance.FromKilometers(5)));
            
            (ViewPage as MainPage).MapLocation.OnPinUpdate();
            DelayInit();
        }

        async private void DelayInit()
        {
            await Task.Delay(1000); //wait full appearing

            (ViewPage as MainPage).IsShowRightPanel = true;

            AppMobileService.Locaion.StartListening();
            AppMobileService.Locaion.PositionUpdated += MapLocation_Position;
        }

        private void ViewPage_Appearing(object sender, EventArgs e)
        {
            if (langInit != AppParameters.Config.SystemLang)
                InitMenuItem();

            if (!String.IsNullOrWhiteSpace(AppData.Discount.ActiveMapPinId))
                ActivateMapPin();

            (ViewPage as MainPage).MapLocation.MapTilesSource = AppParameters.Config.MapSource;
        }

        private LanguageHelper.LangTypeEnum langInit;

        private void InitMenuItem()
        {
            _menuItemList.Clear();

            _menuItemList.Add(new MenuSideBarItem { Icon = contentUI.IconMap, Name = contentUI.Title, MenuPage = typeof(MainPage) });

            var discountContentUI = new DiscountContentUI();
            _menuItemList.Add(new MenuSideBarItem { Icon = discountContentUI.Icon, Name = discountContentUI.Title, MenuPage = typeof(DiscountPage) });

            var settingsContentUI = new SettingsContentUI();
            _menuItemList.Add(new MenuSideBarItem { Icon = settingsContentUI.Icon, Name = settingsContentUI.Title, MenuPage = typeof(SettingsPage) });

            var aboutContentUI = new AboutContentUI();
            _menuItemList.Add(new MenuSideBarItem { Icon = aboutContentUI.Icon, Name = aboutContentUI.Title, MenuPage = typeof(AboutPage) });

            langInit = AppParameters.Config.SystemLang;
        }

        //------------------
        // Property
        //------------------
        private ObservableCollection<MenuSideBarItem> _menuItemList;
        public ObservableCollection<MenuSideBarItem> MenuItemList
        {
            get { return _menuItemList; }
        }

        private ObservableCollection<MenuSideBarItem> _pinList;
        public ObservableCollection<MenuSideBarItem> PinList
        {
            get { return _menuItemList; }
        }

        public string ActivateMapPinId { get; set; }

        public void OnMenuViewItemTapped(object sender, SelectedItemChangedEventArgs e)
        {
            MenuSideBarItem menuItem = e.SelectedItem as MenuSideBarItem;
            ((ListView)sender).SelectedItem = null;

            if ((menuItem != null) && (!IsOpenning))
            {
                ((MainPage)ViewPage).ClosePanel();
                OpenPage(menuItem.MenuPage);
            }
        }

        #region OpenningLocker
        static object OpenningLocker = new object();
        private bool _isOpenning = false;
        private bool IsOpenning
        {
            get
            {
                lock (OpenningLocker)
                    return _isOpenning;
            }
            set
            {
                lock (OpenningLocker)
                    _isOpenning = value;
            }
        }
        #endregion

        async public void OpenPage(Type typePage)
        {
            try
            {
                IsOpenning = true;

                //Map is always opened on MainPage therefore haven't to open it again.
                if (typePage == typeof(MainPage))
                    return;

                var page = (Page)Activator.CreateInstance(typePage);
                await ViewPage.Navigation.PushAsync((BaseContentPage)page);
            }
            finally
            {
                IsOpenning = false;
            }
        }

        async public void MapLocation_ClickPinDetail(object sender, MapPinDataEventArgs e)
        {
            MapPinData pinData = e.PinData;
            DiscountData discountData = AppData.Discount.DiscountCollection.Find(item => { return item.Id == pinData.PartnerId; });

            if (discountData != null)
            {
                try
                {
                    IsLoadActivity = true;
                    await AppData.Discount.LoadBranchList(discountData);
                }
                finally
                {
                    IsLoadActivity = false;
                }

                await ViewPage.Navigation.PushAsync(new DiscountDetailPage(discountData), true);
            }
        }

        public void MapLocation_Position(object sender, EventArgs e)
        {
            (ViewPage as MainPage).MapLocation.LocationUpdate();

            foreach (var item in AppData.Discount.MapPinCollection)
                item.CalculateDistance();
        }

        internal void AppBar_BtnRightClick(object sender, EventArgs e)
        {
            (ViewPage as MainPage).IsShowRightPanel = !(ViewPage as MainPage).IsShowRightPanel;
        }

        public void ActivateMapPin()
        {
            MapPinData pinData = null;

            foreach (var item in AppData.Discount.MapPinCollection)
                if (item.Id == AppData.Discount.ActiveMapPinId)
                {
                    pinData = item;
                    break;
                }

            AppData.Discount.ActiveMapPinId = "";
            
            if (pinData == null)
                return;

            (ViewPage as MainPage).MapLocation.MoveToRegion(MapSpan.FromCenterAndRadius(
                                                       new Position(pinData.Latitude, pinData.Longitude),
                                                       Distance.FromKilometers(2)));
            (ViewPage as MainPage).MapLocation.ShowPinDetailInfo(pinData.Id);
        }

        internal void AppBar_BtnLeftClick(object sender, EventArgs e)
        {
            (ViewPage as MainPage).MapLocation.CloseDetailInfo();
            if (AppMobileService.Locaion.IsAvailable())
            {
                AppMobileService.Locaion.UpdateCurrentLocation();
                if ((AppMobileService.Locaion.CurrentLocation.Latitude != 0) && (AppMobileService.Locaion.CurrentLocation.Longitude != 0))
                    (ViewPage as MainPage).MapLocation.MoveToRegion(MapSpan.FromCenterAndRadius(
                                                               new Position(AppMobileService.Locaion.CurrentLocation.Latitude, AppMobileService.Locaion.CurrentLocation.Longitude),
                                                               Distance.FromKilometers(2)));
            }
            else
                ViewPage.DisplayAlert(contentUI.MsgTitleNoGPS, contentUI.MsgTxtNoGPS, contentUI.TxtOk);
        }
    }

    public class MenuSideBarItem
    {
        public string Icon { get; set; }
        public string Name { get; set; }
        public Type MenuPage { get; set; }
    }
}
