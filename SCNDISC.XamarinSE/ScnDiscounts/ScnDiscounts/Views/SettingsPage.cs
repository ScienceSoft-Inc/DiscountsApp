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

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandParameterProperty, "TypeName");
            tapGestureRecognizer.Tapped += viewModel.LangSetting_Click;
            stackLang.GestureRecognizers.Add(tapGestureRecognizer);

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
            activityIndicator.SetBinding(IsVisibleProperty, "IsLoading");
            activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");

            var txtProgress = new Label
            {
                Style = LabelStyles.DescriptionStyle.FromResources<Style>(),
                LineBreakMode = LineBreakMode.WordWrap,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.White
            };
            txtProgress.SetBinding(Label.TextProperty, "ProcessMessage");

            var layoutSettings = new StackLayout
            {
                Spacing = 20,
                Padding = 24,
                Children =
                {
                    stackLang,
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
