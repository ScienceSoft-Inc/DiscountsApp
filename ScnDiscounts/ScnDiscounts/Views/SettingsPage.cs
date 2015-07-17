using ScnDiscounts.Control;
using ScnDiscounts.ViewModels;
using ScnDiscounts.Views.ContentUI;
using ScnDiscounts.Views.Styles;
using ScnPage.Plugin.Forms;
using ScnTitleBar.Forms;
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
                Title = contentUI.Title,
                TitleStyle = (Style)App.Current.Resources[LabelStyles.PageTitleStyle]
            };
            appBar.BtnBack.BackgroundColor = Color.Transparent;
            appBar.BtnBack.Source = contentUI.IconBack;

            ContentLayout.Children.Add(appBar);

            var stackSettings = new StackLayout
            {
                Spacing = Device.OnPlatform(6, 6, 32),
                Padding = new Thickness(32)
            };

            #region Language setting
            var stackLang = new StackLayout();

            var txtLangTitle = new LabelExtended
            {
                Style = (Style)App.Current.Resources[LabelStyles.SettingStyle]
            };
            txtLangTitle.SetBinding(Label.TextProperty, "CurrLanguageTitle");

            if (Device.OS != TargetPlatform.WinPhone)
            {
                var viewGesture = new ViewGesture
                {
                    Content = txtLangTitle,
                    DeformationValue = -5,
                };
                viewGesture.Gesture.Tap += viewModel.LangSetting_Click;
                stackLang.Children.Add(viewGesture);
            }
            else
            {
                stackLang.Children.Add(txtLangTitle);
                txtLangTitle.Click += viewModel.LangSetting_Click;
            }

            var txtLangValue = new LabelExtended
            {
                Style = (Style)App.Current.Resources[LabelStyles.SettingHintStyle]
            };
            txtLangValue.SetBinding(Label.TextProperty, "CurrLanguageName");
            if (Device.OS != TargetPlatform.WinPhone)
            {
                var viewGesture = new ViewGesture
                {
                    Content = txtLangValue,
                    DeformationValue = -5,
                };
                viewGesture.Gesture.Tap += viewModel.LangSetting_Click;
                stackLang.Children.Add(viewGesture);
            }
            else
            {
                stackLang.Children.Add(txtLangValue);
                txtLangValue.Click += viewModel.LangSetting_Click;
            }

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

       /* private class SettingParam : AbsoluteLayout
        {
            private SettingParam ()
            {
                image = new Image();
                SetLayoutFlags(image, AbsoluteLayoutFlags.PositionProportional);
                SetLayoutBounds(image,
                    new Rectangle(0.5, 0.5, image.Width, image.Height)
                );
                Children.Add(image);

                var boxGesture = new BoxViewGesture(this);
                SetLayoutFlags(boxGesture, AbsoluteLayoutFlags.PositionProportional);
                SetLayoutBounds(boxGesture,
                    new Rectangle(0.5, 0.5, this.Width, this.Height)
                );

                boxGesture.Tap += (s, e) => { OnClick(); };
                boxGesture.TapBegan += boxGesture_PressBegan;
                boxGesture.TapEnded += boxGesture_PressEnded;
                boxGesture.TapMoved += boxGesture_PressEnded;

                boxGesture.LongTapEnded += boxGesture_PressEnded;
                boxGesture.LongTapMoved += boxGesture_PressEnded;

                boxGesture.SwipeEnded += boxGesture_PressEnded;

                Children.Add(boxGesture);

                if (Device.OS == TargetPlatform.WinPhone)
                {
                    var tapGesture = new TapGestureRecognizer();
                    tapGesture.Tapped += (sender, e) =>
                    {
                        OnClick();
                    };
                    GestureRecognizers.Add(tapGesture);
                    image.GestureRecognizers.Add(tapGesture);
                }

            }

        }*/

    }
}
