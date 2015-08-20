using ScnDiscounts.Control;
using ScnDiscounts.ViewModels;
using ScnDiscounts.Views.ContentUI;
using ScnDiscounts.Views.Styles;
using ScnPage.Plugin.Forms;
using ScnTitleBar.Forms;
using ScnViewGestures.Plugin.Forms;
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

            var appBar = new TitleBar(this, TitleBar.BarBtnEnum.bbBack)
            {
                BarColor = (Color)App.Current.Resources[MainStyles.MainBackgroundColor],
                TitleStyle = (Style)App.Current.Resources[LabelStyles.PageTitleStyle]
            };
            appBar.SetBinding(TitleBar.TitleProperty, "Title");
            appBar.BtnBack.BackgroundColor = Color.Transparent;
            appBar.BtnBack.Source = contentUI.IconBack;

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

            var txtLangValue = new LabelExtended
            {
                Style = (Style)App.Current.Resources[LabelStyles.SettingHintStyle]
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

            var viewGesturesLang = new ViewGestures
            {
                Content = stackLang,
                DeformationValue = -5,
            };
            viewGesturesLang.BackgroundColor = this.BackgroundColor;
            viewGesturesLang.Tap += viewModel.LangSetting_Click;

            stackSettings.Children.Add(viewGesturesLang);
            #endregion

            if (Device.OS == TargetPlatform.WinPhone)
            {
                #region Map setting
                var txtMapTitle = new LabelExtended
                {
                    Style = (Style)App.Current.Resources[LabelStyles.SettingStyle]
                };
                txtMapTitle.SetBinding(Label.TextProperty, "MapTitle");

                var txtMapValue = new LabelExtended
                {
                    Style = (Style)App.Current.Resources[LabelStyles.SettingHintStyle]
                };
                txtMapValue.SetBinding(Label.TextProperty, "MapName");

                var stackMap = new StackLayout
                {
                    Children = 
                    {
                        txtMapTitle,
                        txtMapValue
                    }
                };

                var viewGesturesMap = new ViewGestures
                {
                    Content = stackMap,
                    DeformationValue = -5,
                };
                viewGesturesMap.BackgroundColor = this.BackgroundColor;
                viewGesturesMap.Tap += viewModel.MapSetting_Click;

                stackSettings.Children.Add(viewGesturesMap);
                #endregion
            }

            ContentLayout.Children.Add(stackSettings);
        }
    }
}
