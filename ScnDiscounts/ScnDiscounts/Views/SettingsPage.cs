using ScnDiscounts.Control;
using ScnDiscounts.Control.Pages;
using ScnDiscounts.ViewModels;
using ScnDiscounts.Views.ContentUI;
using ScnDiscounts.Views.Styles;
using Xamarin.Forms;

namespace ScnDiscounts.Views
{
    class SettingsPage : BaseContentPage 
    {
        private SettingsViewModel viewModel
        {
            get { return (SettingsViewModel)BindingContext; }
        }

        private SettingsContentUI contentUI
        {
            get { return (SettingsContentUI)ContentUI; }
        }

        public SettingsPage()
            : base(typeof(SettingsViewModel), typeof(SettingsContentUI))
        {
            BackgroundColor = (Color)App.Current.Resources[MainStyles.MainBackgroundColor];

            var appBar = new CustomAppBar(this, CustomAppBar.CustomBarBtnEnum.cbBack)
            {
                BarColor = (Color)App.Current.Resources[MainStyles.MainBackgroundColor],
                Title = contentUI.Title,
                TitleStyle = (Style)App.Current.Resources[LabelStyles.PageTitleStyle]
            };
            appBar.BtnBack.BackgroundColor = Color.Transparent;
            appBar.BtnBack.Source = contentUI.IconBack;
            appBar.BtnBack.WidthRequest = appBar.HeightRequest;
            appBar.BtnBack.HeightRequest = appBar.HeightRequest;

            ContentLayout.Children.Add(appBar);

            var stackSettings = new StackLayout
            {
                Spacing = Device.OnPlatform(6, 6, 32),
                Padding = new Thickness(32)
            };

            #region Language setting
            var txtLangTitle = new LabelExtended
            {
                Style = (Style)App.Current.Resources[LabelStyles.SettingStyle]
            };
            txtLangTitle.SetBinding(Label.TextProperty, "CurrLanguageTitle");
            txtLangTitle.Click += viewModel.LangSetting_Click;

            var txtLangValue = new LabelExtended
            {
                Style = (Style)App.Current.Resources[LabelStyles.SettingHintStyle]
            };
            txtLangValue.SetBinding(Label.TextProperty, "CurrLanguageName");
            txtLangValue.Click += viewModel.LangSetting_Click;

            var stackLang = new StackLayout
            {
                Children = 
                {
                    txtLangTitle,
                    txtLangValue
                }
            };
            stackSettings.Children.Add(stackLang);
            #endregion

            #region Map setting
            var txtMapTitle = new LabelExtended
            {
                Style = (Style)App.Current.Resources[LabelStyles.SettingStyle]
            };
            txtMapTitle.SetBinding(Label.TextProperty, "MapTitle");
            txtMapTitle.Click += viewModel.MapSetting_Click;

            var txtMapValue = new LabelExtended
            {
                Style = (Style)App.Current.Resources[LabelStyles.SettingHintStyle]
            };
            txtMapValue.SetBinding(Label.TextProperty, "MapName");
            txtMapValue.Click += viewModel.MapSetting_Click;

            var stackMap = new StackLayout
            {
                Children = 
                {
                    txtMapTitle,
                    txtMapValue
                }
            };

            stackSettings.Children.Add(stackLang);
            if (Device.OS == TargetPlatform.WinPhone)
                stackSettings.Children.Add(stackMap);
            #endregion

            ContentLayout.Children.Add(stackSettings);
        }
    }
}
