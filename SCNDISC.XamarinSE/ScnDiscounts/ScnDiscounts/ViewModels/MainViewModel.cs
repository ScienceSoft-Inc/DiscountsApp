using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using ScnDiscounts.Control;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models;
using ScnDiscounts.Models.Data;
using ScnDiscounts.Views;
using ScnDiscounts.Views.ContentUI;
using ScnPage.Plugin.Forms;
using ScnSideMenu.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace ScnDiscounts.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private MainContentUI contentUI => (MainContentUI) ContentUI;

        private MainPage MainPage => (MainPage) ViewPage;

        protected override void InitProperty()
        {
            ViewPage.Appearing += ViewPage_InitAfterAppearing;
            ViewPage.Disappearing += ViewPage_Disappearing;

            InitMenuList();
            InitFilterList();
        }

        private async void ViewPage_InitAfterAppearing(object sender, EventArgs e)
        {
            ViewPage.Appearing -= ViewPage_InitAfterAppearing;

            await DelayInit();
        }

        private async Task DelayInit()
        {
            await Task.Yield(); //wait full appearing

            RefreshMap();

            FilterCategorySwitched += SwitchFilter_Toggled;
            DataRefreshing += Filter_DataRefreshing;

            ViewPage.Appearing += ViewPage_Appearing;

            MainPage.IsShowRightPanel = App.PushNotificationData == null;

            AppMobileService.Locaion.PositionUpdated += MapLocation_Position;
            AppMobileService.Locaion.HeadingUpdated += MapLocation_Heading;
            AppMobileService.Locaion.StartListening();

            PushNotificationHelper.EnablePushNotifications(PushNotificationHelper.IsEnabled);

            await App.ProcessPushNotification();
        }

        private void Filter_DataRefreshing(object sender, EventArgs e)
        {
            RefreshMap();
        }

        private void ViewPage_Appearing(object sender, EventArgs e)
        {
            RefreshMap();

            FilterCategorySwitched += SwitchFilter_Toggled;
            DataRefreshing += Filter_DataRefreshing;

            CurrentLang = AppParameters.Config.SystemLang;

            if (!string.IsNullOrEmpty(AppData.Discount.ActiveMapPinId))
                ActivateMapPin();
        }

        private void ViewPage_Disappearing(object sender, EventArgs e)
        {
            FilterCategorySwitched -= SwitchFilter_Toggled;
            DataRefreshing -= Filter_DataRefreshing;

            MainPage.ClosePanel();
        }

        private LanguageHelper.LangTypeEnum _currentLang = AppParameters.Config.SystemLang;

        public LanguageHelper.LangTypeEnum CurrentLang
        {
            get => _currentLang;
            set
            {
                if (_currentLang != value)
                {
                    _currentLang = value;
                    OnPropertyChanged();

                    OnPropertyChanged(nameof(CategoriesTitle));

                    UpdateMenuList();
                    UpdateFilterList();
                }
            }
        }

        #region Menu & Filter list

        private void InitMenuList()
        {
            var discountContent = new DiscountContentUI();
            //var healthContent = new HealthContentUI();
            var settingsContent = new SettingsContentUI();
            var feedbackContent = new FeedbackContentUI();
            var aboutContent = new AboutContentUI();

            MenuItemList = new List<MenuItemData>
            {
                new MenuItemData
                {
                    Icon = contentUI.IconMap,
                    Title = contentUI.Title,
                    TypePage = typeof(MainPage)
                },
                new MenuItemData
                {
                    Icon = discountContent.Icon,
                    Title = discountContent.Title,
                    TypePage = typeof(DiscountPage)
                },
                //new MenuItemData
                //{
                //    Icon = healthContent.Icon,
                //    Title = healthContent.Title,
                //    TypePage = typeof(HealthPage)
                //},
                new MenuItemData
                {
                    Icon = settingsContent.Icon,
                    Title = settingsContent.Title,
                    TypePage = typeof(SettingsPage)
                },
                new MenuItemData
                {
                    Icon = feedbackContent.Icon,
                    Title = feedbackContent.Title,
                    TypePage = typeof(FeedbackPage)
                },
                new MenuItemData
                {
                    Icon = aboutContent.Icon,
                    Title = aboutContent.Title,
                    TypePage = typeof(AboutPage)
                }
            };
        }

        private void UpdateMenuList()
        {
            var discountContent = new DiscountContentUI();
            //var healthContent = new HealthContentUI();
            var settingsContent = new SettingsContentUI();
            var feedbackContent = new FeedbackContentUI();
            var aboutContent = new AboutContentUI();

            var mapItem = MenuItemList.First(i => i.TypePage == typeof(MainPage));
            mapItem.Title = contentUI.Title;

            var discountItem = MenuItemList.First(i => i.TypePage == typeof(DiscountPage));
            discountItem.Title = discountContent.Title;

            //var healthItem = MenuItemList.First(i => i.TypePage == typeof(HealthPage));
            //healthItem.Title = healthContent.Title;

            var settingsItem = MenuItemList.First(i => i.TypePage == typeof(SettingsPage));
            settingsItem.Title = settingsContent.Title;

            var feedbackItem = MenuItemList.First(i => i.TypePage == typeof(FeedbackPage));
            feedbackItem.Title = feedbackContent.Title;

            var aboutItem = MenuItemList.First(i => i.TypePage == typeof(AboutPage));
            aboutItem.Title = aboutContent.Title;
        }

        public void InitFilterList()
        {
            AppParameters.Config.LoadCategoryFilter(AppData.Discount.CategoryCollection);

            FilterCategoryList = AppParameters.Config.FilterCategoryList;
        }

        private void UpdateFilterList()
        {
            foreach (var filterCategoryItem in FilterCategoryList)
            {
                filterCategoryItem.RefreshName();
            }
        }

        private List<MenuItemData> _menuItemList;
        public List<MenuItemData> MenuItemList
        {
            get => _menuItemList;
            set
            {
                _menuItemList = value;
                OnPropertyChanged();
            }
        }

        private List<FilterCategoryItem> _filterCategoryList;
        public List<FilterCategoryItem> FilterCategoryList
        {
            get => _filterCategoryList;
            set
            {
                _filterCategoryList = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public string CategoriesTitle => contentUI.TxtCategories;

        private bool _isAllFiltersSelected;

        public bool IsAllFiltersSelected
        {
            get
            {
                _isAllFiltersSelected = FilterCategoryList.All(i => i.IsToggle);

                return _isAllFiltersSelected;
            }
            set
            {
                if (_isAllFiltersSelected != value)
                {
                    _isAllFiltersSelected = value;
                    OnPropertyChanged();

                    FilterCategoryList.ForEach(i =>
                    {
                        i.SkipRefreshing = i.IsToggle != value;
                        i.IsToggle = value;
                    });

                    OnDataRefreshing();
                }
            }
        }

        private void OpenPage(Type typePage)
        {
            switch (typePage.Name)
            {
                case nameof(Views.MainPage):
                    MainPage.ClosePanel();
                    break;
                case nameof(HealthPage):
                    const string appUrl = "sciencesofthealth://";
                    var appId = Functions.OnPlatform("id1434682692", "com.scnsoft.task_insurance");

                    var marketUrl =
                        Functions.OnPlatform(
                            $"itms-apps://itunes.apple.com/app/sciencesoft-health/{appId}?mt=8",
                            $"market://details?id={appId}");

                    var webUrl =
                        Functions.OnPlatform(
                            $"https://itunes.apple.com/app/sciencesoft-health/{appId}?ls=1&mt=8",
                            $"https://play.google.com/store/apps/details?id={appId}");

                    DependencyService.Get<IPhoneService>().LaunchApp(appId, appUrl, marketUrl, webUrl);
                    break;
                default:
                    ViewPage.OpenPage((BaseContentPage) Activator.CreateInstance(typePage));
                    break;
            }
        }

        public async void MapLocation_ClickPinDetail(object sender, MapPinDataEventArgs e)
        {
            await ViewPage.OpenDetailPage(e.PinData.PartnerId);
        }

        private void MapLocation_Position(object sender, EventArgs e)
        {
            MainPage.MapLocation.LocationUpdate();

            var mapPinCollection = AppData.Discount.MapPinCollection.ToList();
            mapPinCollection.ForEach(i => i.CalculateDistance());
        }

        private void MapLocation_Heading(object sender, EventArgs e)
        {
            MainPage.MapLocation.HeadingUpdate();
        }

        private void ActivateMapPin()
        {
            var pinId = AppData.Discount.ActiveMapPinId;
            var pinData = AppData.Discount.MapPinCollection.FirstOrDefault(i => i.Id == pinId);
            AppData.Discount.ActiveMapPinId = string.Empty;

            MainPage.MapLocation.ActivatePin(pinData);
        }

        private void RefreshMap()
        {
            var pinList = new List<MapPinData>();

            var filteredCategories = FilterCategoryList.Where(i => i.IsToggle).Select(i => i.CategoryData.Id).ToArray();
            var items = AppData.Discount.MapPinCollection.ToList();

            foreach (var item in items)
            {
                item.PrimaryCategory = item.CategoryList.FirstOrDefault(i => filteredCategories.Contains(i.Id));

                if (item.CategoryList.Count == 0 || item.PrimaryCategory != null)
                    pinList.Add(item);
            }

            MainPage.MapLocation.PinList = pinList;
        }

        public void AppBar_BtnRightClick(object sender, EventArgs e)
        {
            MainPage.MapLocation.CloseDetailInfo();
            MainPage.IsShowRightPanel = !MainPage.IsShowRightPanel;
        }

        public void AppBar_BtnLeftPhoneClick(object sender, EventArgs e)
        {
            MainPage.MapLocation.CloseDetailInfo();
            MainPage.IsShowLeftPanel = !MainPage.IsShowLeftPanel;
        }

        public void AppBar_BtnLeftTabletClick(object sender, EventArgs e)
        {
            MainPage.ClosePanel();
            BtnLocation_Click(sender, e);
        }

        private static async Task<PermissionStatus?> AskForLocationPermission()
        {
            PermissionStatus? result;

            try
            {
                result = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);

                if (result != PermissionStatus.Granted)
                {
                    var permissions = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    if (permissions.ContainsKey(Permission.Location))
                        result = permissions[Permission.Location];
                }
            }
            catch
            {
                result = null;
            }    

            return result;
        }

        public async void BtnLocation_Click(object sender, EventArgs e)
        {
            MainPage.MapLocation.CloseDetailInfo();

            var result = await AskForLocationPermission();

            if (result.HasValue)
            {
                if (result == PermissionStatus.Granted && LocationHelper.IsGeoServiceEnabled)
                {
                    if (LocationHelper.IsGeoServiceAvailable)
                    {
                        await AppMobileService.Locaion.UpdateCurrentLocation();

                        var position = AppMobileService.Locaion.CurrentLocation;
                        if (position != null)
                        {
                            var mapPosition = new Position(position.Latitude, position.Longitude);
                            var mapSpan = MapSpan.FromCenterAndRadius(mapPosition, Distance.FromMeters(250));
                            MainPage.MapLocation.MoveToRegion(mapSpan);
                        }
                    }
                }
                else
                {
                    var isLocationDenied = result == PermissionStatus.Denied;
                    var message = isLocationDenied ? contentUI.MsgTxtDeniedGps : contentUI.MsgTxtNoGps;
                    var successText = isLocationDenied ? contentUI.TxtAppSettings : contentUI.TxtPhoneSettings;

                    var isSuccess = await ViewPage.DisplayAlert(contentUI.MsgTitleNoGps, message, successText.ToUpper(),
                        contentUI.TxtCancel.ToUpper());

                    if (isSuccess)
                    {
                        if (isLocationDenied)
                            CrossPermissions.Current.OpenAppSettings();
                        else
                            DependencyService.Get<IPhoneService>().OpenGpsSettings();
                    }
                }
            }
        }

        public void SwitchFilter_Toggled(object sender, ToggledEventArgs e)
        {
            var bindableObject = (BindableObject) sender;
            var filterItem = (FilterCategoryItem) bindableObject?.BindingContext;

            if (filterItem != null)
            {
                if (!ViewPage.IsOpenning)
                {
                    OnPropertyChanged(nameof(IsAllFiltersSelected));

                    if (filterItem.SkipRefreshing)
                        filterItem.SkipRefreshing = false;
                    else
                        OnDataRefreshing();
                }

                AppParameters.Config.SaveCategoryFilter(filterItem);
            }
        }

        public void FilterGestures_Tap(object sender, EventArgs e)
        {
            var bindableObject = (BindableObject) sender;
            var filterItem = (FilterCategoryItem) bindableObject?.BindingContext;

            if (filterItem != null)
                filterItem.IsToggle = !filterItem.IsToggle;
        }

        public void AllFiltersGestures_Tap(object sender, EventArgs e)
        {
            IsAllFiltersSelected = !IsAllFiltersSelected;
        }

        public void MenuItem_Tap(object sender, EventArgs e)
        {
            if (ViewPage.IsOpenning)
                return;

            var view = (View) sender;
            view.ClickAnimation(() =>
            {
                var args = e as TappedEventArgs;
                var tag = args?.Parameter?.ToString();
                if (!string.IsNullOrEmpty(tag))
                {
                    var type = Type.GetType(tag);
                    OpenPage(type);
                }
            });
        }

        public event EventHandler<ToggledEventArgs> FilterCategorySwitched;

        public void OnFilterCategorySwitched(object sender, ToggledEventArgs e)
        {
            FilterCategorySwitched?.Invoke(sender, e);
        }

        public event EventHandler DataRefreshing;

        public void OnDataRefreshing()
        {
            DataRefreshing?.Invoke(this, EventArgs.Empty);
        }
    }
}
