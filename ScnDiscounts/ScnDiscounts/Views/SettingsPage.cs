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

            ContentLayout.Children.Add(stackSettings);
        }
    }
}
