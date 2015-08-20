using System;
using System.Collections.ObjectModel;
using System.Linq;
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
            InitMenuList();

            _filterCategoryList = new ObservableCollection<FilterCategoryItem>();
            InitFilterList();
        }

        private void ViewPage_InitAfterAppearing(object sender, EventArgs e)
        {
            ViewPage.Appearing -= ViewPage_InitAfterAppearing;
            InitMapPinCollection();
            (ViewPage as MainPage).MapLocation.OnPinUpdate();

            if (ViewPage.Navigation.NavigationStack.Count == 1)
            {
                double minskLat = 53.904841;
                double minskLong = 27.55327;
                (ViewPage as MainPage).MapLocation.MoveToRegion(MapSpan.FromCenterAndRadius(
                                                           new Position(minskLat, minskLong),
                                                           Distance.FromKilometers(5)));
                DelayInit();
            }
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
            {
                InitMenuList();
                InitFilterList();
            }

            if (!String.IsNullOrWhiteSpace(AppData.Discount.ActiveMapPinId))
                ActivateMapPin();

            (ViewPage as MainPage).MapLocation.MapTilesSource = AppParameters.Config.MapSource;
        }

        private LanguageHelper.LangTypeEnum langInit;

        private void InitMenuList()
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

        private void InitFilterList()
        {
            FilterCategoryList = new ObservableCollection<FilterCategoryItem>(AppParameters.Config.FilterCategoryList);
        }

        //------------------
        // Property
        //------------------
        private ObservableCollection<MenuSideBarItem> _menuItemList;
        public ObservableCollection<MenuSideBarItem> MenuItemList
        {
            get { return _menuItemList; }
        }

        private ObservableCollection<FilterCategoryItem> _filterCategoryList;
        public ObservableCollection<FilterCategoryItem> FilterCategoryList
        {
            get { return _filterCategoryList; }
            set 
            {
                _filterCategoryList = value;
                OnPropertyChanged();
            }
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
            DiscountData discountData = AppData.Discount.DiscountCollection.Find(item => { return item.DocumentId == pinData.PartnerId; });

            if (discountData != null)
            {
                try
                {
                    IsLoadActivity = true;
                    await AppData.Discount.LoadFullDescription(discountData);
                }
                finally
                {
                    IsLoadActivity = false;
                }

                await ViewPage.Navigation.PushAsync(new DiscountDetailPage(discountData.DocumentId), true);
            }
        }

        public void MapLocation_Position(object sender, EventArgs e)
        {
            (ViewPage as MainPage).MapLocation.LocationUpdate();

            foreach (var item in AppData.Discount.MapPinCollection)
                item.CalculateDistance();
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

        public void InitMapPinCollection()
        {
            (ViewPage as MainPage).MapLocation.PinList.Clear();

            foreach (var item in AppData.Discount.MapPinCollection)
            {
                var primaryCategoryShown = item.CategorieList.FirstOrDefault(c => 
                    AppParameters.Config.FilterCategoryList
                        .Where<FilterCategoryItem>(f => f.IsToggle)
                        .FirstOrDefault<FilterCategoryItem>(f => f.Id == c.TypeCode)
                    != null);
                
                if (primaryCategoryShown != null)
                {
                    item.PrimaryCategory = primaryCategoryShown;
                    (ViewPage as MainPage).MapLocation.PinList.Add(item);
                }
            }
        }

        internal void AppBar_BtnRightClick(object sender, EventArgs e)
        {
            (ViewPage as MainPage).MapLocation.CloseDetailInfo();
            (ViewPage as MainPage).IsShowRightPanel = !(ViewPage as MainPage).IsShowRightPanel;
        }

        internal void AppBar_BtnLeftClick(object sender, EventArgs e)
        {
            (ViewPage as MainPage).MapLocation.CloseDetailInfo();
            (ViewPage as MainPage).IsShowLeftPanel = !(ViewPage as MainPage).IsShowLeftPanel;
        }

        internal void BtnLocation_Click(object sender, EventArgs e)
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

        internal void SwitchFilter_Toggled(object sender, ToggledEventArgs e)
        {
            InitMapPinCollection();
            (ViewPage as MainPage).MapLocation.OnPinUpdate();
        }

        internal void FilterGestures_Tap(object sender, EventArgs e)
        {
            (ViewPage as MainPage).IsShowLeftPanel = false;
        }
    }

    public class MenuSideBarItem
    {
        public string Icon { get; set; }
        public string Name { get; set; }
        public Type MenuPage { get; set; }
    }
}
