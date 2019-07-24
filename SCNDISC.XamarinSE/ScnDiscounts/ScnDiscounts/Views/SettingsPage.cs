using ScnDiscounts.Control;
using ScnDiscounts.Helpers;
using ScnDiscounts.ViewModels;
using ScnDiscounts.Views.ContentUI;
using ScnDiscounts.Views.Styles;
using ScnPage.Plugin.Forms;
using ScnTitleBar.Forms;
using Xamarin.Forms;

namespace ScnDiscounts.Views
{
    public class SettingsPage : BaseContentPage
    {
        private SettingsViewModel viewModel => (SettingsViewModel) ViewModel;

        private SettingsContentUI contentUI => (SettingsContentUI) ContentUI;

        public SettingsPage()
            : base(typeof(SettingsViewModel), typeof(SettingsContentUI))
        {
            BackgroundColor = MainStyles.StatusBarColor.FromResources<Color>();
            Content.BackgroundColor = MainStyles.MainBackgroundColor.FromResources<Color>();

            var loadingColor = MainStyles.LoadingColor.FromResources<Color>();
            LoadingActivityIndicator.Color = loadingColor;
            LoadingActivityText.TextColor = loadingColor;

            var appBar = new TitleBar(this, TitleBar.BarBtnEnum.bbBack)
            {
                BarColor = MainStyles.StatusBarColor.FromResources<Color>(),
                TitleStyle = LabelStyles.PageTitleStyle.FromResources<Style>(),
                BtnBack =
                {
                    Source = contentUI.IconBack
                }
            };
            appBar.SetBinding(TitleBar.TitleProperty, "Title");

            #region Language setting

            var txtLangTitle = new Label
            {
                Style = LabelStyles.SettingStyle.FromResources<Style>()
            };
            txtLangTitle.SetBinding(Label.TextProperty, "CurrentLanguageTitle");

            var txtLangValue = new Label
            {
                Style = LabelStyles.SettingHintStyle.FromResources<Style>()
            };
            txtLangValue.SetBinding(Label.TextProperty, "CurrLanguageName");
            
            var stackLang = new StackLayout
            {
                Children =
                {
                    txtLangTitle,
                    txtLangValue
                }
            };

            var tapLang = new TapGestureRecognizer();
            tapLang.Tapped += viewModel.LangSetting_Click;
            stackLang.GestureRecognizers.Add(tapLang);

            #endregion

            #region Push notifications setting

            var txtPushNotifications = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Style = LabelStyles.SettingStyle.FromResources<Style>()
            };
            txtPushNotifications.SetBinding(Label.TextProperty, "PushNotificationsTitle");

            var switchPushNotifications = new Switch
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                OnColor = MainStyles.SwitchColor.FromResources<Color>()
            };
            switchPushNotifications.SetBinding(Switch.IsToggledProperty, "IsPushEnabled");

            var stackPushNotifications = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Spacing = 0,
                Children =
                {
                    txtPushNotifications,
                    switchPushNotifications
                }
            };

            var tapPushNotifications = new TapGestureRecognizer();
            tapPushNotifications.Tapped += viewModel.SwitchPushNotifications_Toggled;
            stackPushNotifications.GestureRecognizers.Add(tapPushNotifications);

            #endregion

            var btnUpdateDb = new ButtonExtended
            {
                Style = LabelStyles.ButtonStyle.FromResources<Style>()
            };
            btnUpdateDb.SetBinding(Button.TextProperty, "UpdateDbTitle");
            btnUpdateDb.SetBinding(IsEnabledProperty, "IsNotLoading");
            btnUpdateDb.Clicked += viewModel.BtnUpdateDb_Clicked;

            var activityIndicator = new ActivityIndicator
            {
                Color = Color.White,
                HorizontalOptions = LayoutOptions.Center
            };
            activityIndicator.SetBinding(IsVisibleProperty, "IsUpdating");
            activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsUpdating");

            var txtProgress = new Label
            {
                Style = LabelStyles.DescriptionLightStyle.FromResources<Style>(),
                HorizontalTextAlignment = TextAlignment.Center
            };
            txtProgress.SetBinding(Label.TextProperty, "ProcessMessage");

            var layoutSettings = new StackLayout
            {
                Spacing = 20,
                Padding = 24,
                Children =
                {
                    stackLang,
                    stackPushNotifications,
                    btnUpdateDb,
                    activityIndicator,
                    txtProgress
                }
            };

            var safeAreaHelper = new SafeAreaHelper();
            safeAreaHelper.UseSafeArea(this, SafeAreaHelper.CustomSafeAreaFlags.Top);
            safeAreaHelper.UseSafeArea(appBar.BtnBack, SafeAreaHelper.CustomSafeAreaFlags.Left);
            safeAreaHelper.UseSafeArea(layoutSettings, SafeAreaHelper.CustomSafeAreaFlags.Horizontal);

            ContentLayout.Children.Add(appBar);
            ContentLayout.Children.Add(layoutSettings);
        }
    }
}
