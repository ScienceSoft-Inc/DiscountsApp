using System;
using ScnDiscounts.Control;
using ScnDiscounts.Control.Pages;
using ScnDiscounts.ViewModels;
using ScnDiscounts.Views.ContentUI;
using ScnDiscounts.Views.Styles;
using Xamarin.Forms;

namespace ScnDiscounts.Views
{
    class AboutPage : BaseContentPage 
    {
        private AboutViewModel viewModel
        {
            get { return (AboutViewModel)BindingContext; }
        }

        private AboutContentUI contentUI
        {
            get { return (AboutContentUI)ContentUI; }
        }

        public AboutPage()
            : base(typeof(AboutViewModel), typeof(AboutContentUI))
        {
            BackgroundColor = (Color)App.Current.Resources[MainStyles.MainBackgroundColor];

            var appBar = new CustomAppBar(this, CustomAppBar.CustomBarBtnEnum.cbBack)
            {
                BarColor = (Color)App.Current.Resources[MainStyles.MainBackgroundColor]
            };
            appBar.BtnBack.BackgroundColor = Color.Transparent;
            appBar.BtnBack.Source = contentUI.IconBack;
            appBar.BtnBack.WidthRequest = appBar.HeightRequest;
            appBar.BtnBack.HeightRequest = appBar.HeightRequest;

            ContentLayout.Children.Add(appBar);

            #region Logo
            var imgLogo = new Image
            {
                Source = contentUI.ImgLogo,
                HeightRequest = Device.OnPlatform(-1, -1, 150),
            };
            #endregion

            #region Description info
            var txtDescription = new Label
            {
                Text = contentUI.TxtDescription,
                Style = (Style)App.Current.Resources[LabelStyles.AboutDescriptionStyle],
            };

            var txtDescriptionBullet1 = new Label
            {
                Text = contentUI.TxtDescriptionBullet1,
                Style = (Style)App.Current.Resources[LabelStyles.AboutDescriptionStyle],
            };

            var stackDescriptionBullet1 = new StackLayout
            {
                Padding = new Thickness(12, 0, 0, 0),
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    new Label 
                    {
                        VerticalOptions = LayoutOptions.Start,
                        Text = contentUI.TxtDescriptionBulletSymbol,
                        Style = (Style)App.Current.Resources[LabelStyles.AboutDescriptionStyle],
                    },
                    new Label 
                    {
                        Text = contentUI.TxtDescriptionBullet1,
                        Style = (Style)App.Current.Resources[LabelStyles.AboutDescriptionStyle],
                    },
                }
            };

            var stackDescriptionBullet2 = new StackLayout
            {
                Padding = new Thickness(12, 0, 0, 0),
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    new Label 
                    {
                        VerticalOptions = LayoutOptions.Start,
                        Text = contentUI.TxtDescriptionBulletSymbol,
                        Style = (Style)App.Current.Resources[LabelStyles.AboutDescriptionStyle],
                    },
                    new Label 
                    {
                        Text = contentUI.TxtDescriptionBullet2,
                        Style = (Style)App.Current.Resources[LabelStyles.AboutDescriptionStyle],
                    },
                }
            };

            var stackDescriptionBullet3 = new StackLayout
            {
                Padding = new Thickness(12, 0, 0, 0),
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    new Label 
                    {
                        VerticalOptions = LayoutOptions.Start,
                        Text = contentUI.TxtDescriptionBulletSymbol,
                        Style = (Style)App.Current.Resources[LabelStyles.AboutDescriptionStyle],
                    },
                    new Label 
                    {
                        Text = contentUI.TxtDescriptionBullet3,
                        Style = (Style)App.Current.Resources[LabelStyles.AboutDescriptionStyle],
                    },
                }
            };

            var txtDescriptionLink = new LabelExtended
            {
                Text = contentUI.TxtDescriptionLink,
                Style = (Style)App.Current.Resources[LabelStyles.LinkStyle],
                TextColor = (Color)App.Current.Resources[MainStyles.LightTextColor],
                LineBreakMode = LineBreakMode.WordWrap,
                IsUnderline = true
            };
            txtDescriptionLink.Click += viewModel.txtLink_Click;

            var stackDescription = new StackLayout
            {
                Children = 
                {
                    txtDescription,
                    stackDescriptionBullet1,
                    stackDescriptionBullet2,
                    stackDescriptionBullet3,
                    txtDescriptionLink
                }
            };

            #endregion

            #region Version info
            var txtTitleVersion = new Label 
            {
                Text = contentUI.TitleVersion,
                Style = (Style)App.Current.Resources[LabelStyles.DescriptionStyle],
                TextColor = (Color)App.Current.Resources[MainStyles.LightTextColor],
                Opacity = 0.5
            };

            var txtNumberVersion = new Label 
            {
                Text = contentUI.TxtVersionValue,
                Style = (Style)App.Current.Resources[LabelStyles.AboutVersionStyle],
            };

            var stackVersion = new StackLayout
            {
                Children =
                {
                    txtTitleVersion,
                    txtNumberVersion
                }
            };
            #endregion

            #region Developer info
            var txtTitleDeveloper = new Label
            {
                Text = contentUI.TitleDeveloper,
                Style = (Style)App.Current.Resources[LabelStyles.DescriptionStyle],
                TextColor = (Color)App.Current.Resources[MainStyles.LightTextColor],
                Opacity = 0.5
            };

            var txtPhone = new LabelExtended
            {
                Text = String.Format("{0}: {1}", contentUI.TxtPhone, contentUI.TxtPhoneValue),
                Style = (Style)App.Current.Resources[LabelStyles.DescriptionStyle],
                TextColor = (Color)App.Current.Resources[MainStyles.LightTextColor]
            };
            txtPhone.Click += viewModel.txtPhone_Click;

            var txtEmail = new Label
            {
                Text = String.Format("{0}: {1}", contentUI.TxtEmail, contentUI.TxtEmailValue),
                Style = (Style)App.Current.Resources[LabelStyles.DescriptionStyle],
                TextColor = (Color)App.Current.Resources[MainStyles.LightTextColor]
            };

            var txtHttp = new LabelExtended
            {
                Text = contentUI.TxtHttpValue,
                Style = (Style)App.Current.Resources[LabelStyles.LinkStyle],
                TextColor = (Color)App.Current.Resources[MainStyles.LightTextColor],
                IsUnderline = true
            };
            txtHttp.Click += viewModel.txtLink_Click;

            var stackDeveloper = new StackLayout
            {
                Children =
                {
                    txtTitleDeveloper,
                    txtPhone,
                    txtEmail,
                    txtHttp
                }
            };
            #endregion

            var stackAbout = new StackLayout
            {
                Padding = new Thickness(24),
                Spacing = 20,

                Children =
                {
                    imgLogo,
                    stackDescription,
                    stackVersion,
                    stackDeveloper
                }
            };

            var scrollAbout = new ScrollView
            {
                Content = stackAbout
            };

            ContentLayout.Children.Add(scrollAbout);
        }
    }
}
