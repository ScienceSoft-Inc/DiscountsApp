using System;
using System.Collections.ObjectModel;
using ScnDiscounts.Control;
using ScnDiscounts.Control.Pages;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models;
using ScnDiscounts.Models.Data;
using ScnDiscounts.Views;
using ScnDiscounts.Views.ContentUI;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

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

            AppMobileService.Locaion.StartListening();
            AppMobileService.Locaion.PositionUpdated += MapLocation_Position;
        }

        private void ViewPage_InitAfterAppearing(object sender, EventArgs e)
        {
            ViewPage.Appearing -= ViewPage_InitAfterAppearing;

            double minskLat = 53.904841;
            double minskLong = 27.55327;
            (ViewPage as MainPage).MapLocation.MoveToRegion(MapSpan.FromCenterAndRadius(
                                                       new Position(minskLat, minskLong),
                                                       Distance.FromMiles(4)));
            (ViewPage as MainPage).IsShowRightPanel = true;
            (ViewPage as MainPage).MapLocation.OnPinUpdate();
        }
      
        private void ViewPage_Appearing(object sender, EventArgs e)
        {
            if (langInit != AppParameters.Config.SystemLang)
                InitMenuItem();

            if (!String.IsNullOrWhiteSpace(AppData.Discount.ActiveMapPinId))
                ActivateMapPin();
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

            if (menuItem != null)
            {
                OpenPage(menuItem.MenuPage);
                ((ListView)sender).SelectedItem = null;
            }

            ((MainPage)ViewPage).ClosePanel();
        }

        public void OpenPage(Type typePage)
        {
            //Map is always opened on MainPage therefore haven't to open it again. GTFO!
            if (typePage == typeof(MainPage))
            {
                //((MainPage)ViewPage).MapLocation.OnPinUpdate();
                return;
            }

            var page = (Page)Activator.CreateInstance(typePage);
            ViewPage.Navigation.PushAsync((BaseContentPage)page, true);
        }

        async public void MapLocation_ClickPinDetail(object sender, MapPinDataEventArgs e)
        {
            MapPinData pinData = e.PinData;
            DiscountData discountData = AppData.Discount.DiscountCollection.Find(item => { return item.Id == pinData.PartnerId; });

            if (discountData != null)
            {
                BaseContentPage page = null;

                IsLoadActivity = true;
                try
                {
                    //Load branch
                    await AppData.Discount.LoadBranchList(discountData);
                    page = new DiscountDetailPage(discountData);
                }
                finally
                {
                    IsLoadActivity = false;
                }

                if (page != null)
                    await ViewPage.Navigation.PushAsync(page, true);
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
                                                       Distance.FromMiles(1)));
            (ViewPage as MainPage).MapLocation.ShowDetailInfo(pinData.Id);
        }

        internal void AppBar_BtnLeftClick(object sender, EventArgs e)
        {
            if (AppMobileService.Locaion.IsAvailable())
                (ViewPage as MainPage).MapLocation.MoveToRegion(MapSpan.FromCenterAndRadius(
                                                           new Position(AppMobileService.Locaion.CurrentLocation.Latitude, AppMobileService.Locaion.CurrentLocation.Longitude),
                                                           Distance.FromMiles(2)));
        }
    }

    public class MenuSideBarItem
    {
        public string Icon { get; set; }
        public string Name { get; set; }
        public Type MenuPage { get; set; }
    }
}
