using System;
using ScnDiscounts.Control;
using ScnDiscounts.ViewModels;
using ScnDiscounts.Views.ContentUI;
using ScnDiscounts.Views.Styles;
using Xamarin.Forms;
using ScnPage.Plugin.Forms;
using ScnTitleBar.Forms;
using ScnViewGestures.Plugin.Forms;

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

            var appBar = new TitleBar(this, TitleBar.BarBtnEnum.bbBack)
            {
                BarColor = (Color)App.Current.Resources[MainStyles.StatusBarColor]
            };
            appBar.BtnBack.BackgroundColor = Color.Transparent;
            appBar.BtnBack.Source = contentUI.IconBack;

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
                Style = (Style)App.Current.Resources[LabelStyles.DescriptionLightStyle],
            };

            var txtDescriptionBullet1 = new Label
            {
                Text = contentUI.TxtDescriptionBullet1,
                Style = (Style)App.Current.Resources[LabelStyles.DescriptionLightStyle],
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
                        Style = (Style)App.Current.Resources[LabelStyles.DescriptionLightStyle],
                    },
                    new Label 
                    {
                        Text = contentUI.TxtDescriptionBullet1,
                        Style = (Style)App.Current.Resources[LabelStyles.DescriptionLightStyle],
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
                        Style = (Style)App.Current.Resources[LabelStyles.DescriptionLightStyle],
                    },
                    new Label 
                    {
                        Text = contentUI.TxtDescriptionBullet2,
                        Style = (Style)App.Current.Resources[LabelStyles.DescriptionLightStyle],
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
                        Style = (Style)App.Current.Resources[LabelStyles.DescriptionLightStyle],
                    },
                    new Label 
                    {
                        Text = contentUI.TxtDescriptionBullet3,
                        Style = (Style)App.Current.Resources[LabelStyles.DescriptionLightStyle],
                    },
                }
            };

            var stackDescription = new StackLayout
            {
                Children = 
                {
                    txtDescription,
                    stackDescriptionBullet1,
                    stackDescriptionBullet2,
                    stackDescriptionBullet3
                }
            };

            //because apple might reject application
            if (Device.OS != TargetPlatform.iOS)
            {
                var txtDescriptionLink = new LabelExtended
                {
                    Text = contentUI.TxtDescriptionLink,
                    Style = (Style)App.Current.Resources[LabelStyles.LinkStyle],
                    TextColor = (Color)App.Current.Resources[MainStyles.LightTextColor],
                    LineBreakMode = LineBreakMode.WordWrap,
                    IsUnderline = true
                };

                var viewGesturesLink = new ViewGestures
                {
                    Content = txtDescriptionLink,
                    DeformationValue = -5,
                };
                viewGesturesLink.BackgroundColor = this.BackgroundColor;
                viewGesturesLink.Tap += viewModel.txtLink_Click;
                stackDescription.Children.Add(viewGesturesLink);
            }
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
                Style = (Style)App.Current.Resources[LabelStyles.DescriptionLightStyle],
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
            var stackDeveloper = new StackLayout();

            var txtTitleDeveloper = new Label
            {
                Text = contentUI.TitleDeveloper,
                Style = (Style)App.Current.Resources[LabelStyles.DescriptionStyle],
                TextColor = (Color)App.Current.Resources[MainStyles.LightTextColor],
                Opacity = 0.5
            };
            stackDeveloper.Children.Add(txtTitleDeveloper);

            var txtPhone = new LabelExtended
            {
                Text = String.Format("{0}: {1}", contentUI.TxtPhone, contentUI.TxtPhoneValue),
                Style = (Style)App.Current.Resources[LabelStyles.DescriptionStyle],
                TextColor = (Color)App.Current.Resources[MainStyles.LightTextColor]
            };
            
            var viewGesturesPhone = new ViewGestures
            {
                Content = txtPhone,
                DeformationValue = -5,
            };
            viewGesturesPhone.BackgroundColor = this.BackgroundColor;
            viewGesturesPhone.Tap += viewModel.txtPhone_Click;
            stackDeveloper.Children.Add(viewGesturesPhone);

            var txtEmail = new Label
            {
                Text = String.Format("{0}: {1}", contentUI.TxtEmail, contentUI.TxtEmailValue),
                Style = (Style)App.Current.Resources[LabelStyles.DescriptionStyle],
                TextColor = (Color)App.Current.Resources[MainStyles.LightTextColor]
            };
            stackDeveloper.Children.Add(txtEmail);

            var txtHttp = new LabelExtended
            {
                Text = contentUI.TxtHttpValue,
                Style = (Style)App.Current.Resources[LabelStyles.LinkStyle],
                TextColor = (Color)App.Current.Resources[MainStyles.LightTextColor],
                IsUnderline = true
            };

            var viewGesturesHTTP = new ViewGestures
            {
                Content = txtHttp,
                DeformationValue = -5,
            };
            viewGesturesHTTP.BackgroundColor = this.BackgroundColor;
            viewGesturesHTTP.Tap += viewModel.txtLink_Click;
            stackDeveloper.Children.Add(viewGesturesHTTP);

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
