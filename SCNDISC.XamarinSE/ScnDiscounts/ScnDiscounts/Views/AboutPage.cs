using ScnDiscounts.Helpers;
using ScnDiscounts.ViewModels;
using ScnDiscounts.Views.ContentUI;
using ScnDiscounts.Views.Styles;
using ScnPage.Plugin.Forms;
using ScnTitleBar.Forms;
using Xamarin.Forms;

namespace ScnDiscounts.Views
{
    public class AboutPage : BaseContentPage
    {
        private AboutViewModel viewModel => (AboutViewModel) ViewModel;

        private AboutContentUI contentUI => (AboutContentUI) ContentUI;

        public AboutPage()
            : base(typeof(AboutViewModel), typeof(AboutContentUI))
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

            var imgLogo = new Image
            {
                Source = contentUI.ImgLogo
            };

            var txtTitle = new Label
            {
                Text = contentUI.TxtTitle,
                Style = LabelStyles.TitleLightStyle.FromResources<Style>()
            };

            #region Description info

            var txtDescription1 = new Label
            {
                Text = contentUI.TxtDescription1,
                Style = LabelStyles.DescriptionLightStyle.FromResources<Style>()
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
                        Style = LabelStyles.DescriptionLightStyle.FromResources<Style>()
                    },
                    new Label
                    {
                        Text = contentUI.TxtDescriptionBullet1,
                        Style = LabelStyles.DescriptionLightStyle.FromResources<Style>()
                    }
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
                        Style = LabelStyles.DescriptionLightStyle.FromResources<Style>()
                    },
                    new Label
                    {
                        Text = contentUI.TxtDescriptionBullet2,
                        Style = LabelStyles.DescriptionLightStyle.FromResources<Style>()
                    }
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
                        Style = LabelStyles.DescriptionLightStyle.FromResources<Style>()
                    },
                    new Label
                    {
                        Text = contentUI.TxtDescriptionBullet3,
                        Style = LabelStyles.DescriptionLightStyle.FromResources<Style>()
                    }
                }
            };

            var stackDescriptionBullet4 = new StackLayout
            {
                Padding = new Thickness(12, 0, 0, 0),
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    new Label
                    {
                        VerticalOptions = LayoutOptions.Start,
                        Text = contentUI.TxtDescriptionBulletSymbol,
                        Style = LabelStyles.DescriptionLightStyle.FromResources<Style>()
                    },
                    new Label
                    {
                        Text = contentUI.TxtDescriptionBullet4,
                        Style = LabelStyles.DescriptionLightStyle.FromResources<Style>()
                    }
                }
            };

            var txtDescription2 = new Label
            {
                Text = contentUI.TxtDescription2,
                Style = LabelStyles.DescriptionLightStyle.FromResources<Style>()
            };

            var tapEmailAlternate = new TapGestureRecognizer
            {
                CommandParameter = contentUI.TxtEmailAlternateValue
            };
            tapEmailAlternate.Tapped += viewModel.TxtEmail_Click;

            var txtDescription3 = new Label
            {
                FormattedText = new FormattedString
                {
                    Spans =
                    {
                        new Span
                        {
                            Text = contentUI.TxtDescription3,
                            Style = LabelStyles.DescriptionLightStyle.FromResources<Style>()
                        },
                        new Span
                        {
                            Text = contentUI.TxtEmailAlternateValue,
                            Style = LabelStyles.LinkStyle.FromResources<Style>(),
                            TextColor = MainStyles.LightTextColor.FromResources<Color>(),
                            GestureRecognizers =
                            {
                                tapEmailAlternate
                            }
                        }
                    }
                }
            };

            var stackDescription = new StackLayout
            {
                Spacing = 0,
                Children =
                {
                    txtDescription1,
                    stackDescriptionBullet1,
                    stackDescriptionBullet2,
                    stackDescriptionBullet3,
                    stackDescriptionBullet4,
                    txtDescription2,
                    txtDescription3
                }
            };

            #endregion

            #region Version info

            var txtTitleVersion = new Label
            {
                Text = contentUI.TitleVersion,
                Style = LabelStyles.DescriptionStyle.FromResources<Style>(),
                TextColor = MainStyles.LightTextColor.FromResources<Color>(),
                Opacity = 0.5
            };

            var txtNumberVersion = new Label
            {
                Text = contentUI.TxtVersionValue,
                Style = LabelStyles.DescriptionLightStyle.FromResources<Style>()
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
                Style = LabelStyles.DescriptionStyle.FromResources<Style>(),
                TextColor = MainStyles.LightTextColor.FromResources<Color>(),
                Opacity = 0.5
            };

            #region Phone

            var txtPhoneSymbol = new Label
            {
                Text = contentUI.TxtPhoneSymbol,
                Style = LabelStyles.DescriptionStyle.FromResources<Style>(),
                TextColor = MainStyles.LightTextColor.FromResources<Color>()
            };

            var txtPhone = new Label
            {
                Text = contentUI.TxtPhoneValue,
                Style = LabelStyles.DescriptionStyle.FromResources<Style>(),
                TextColor = MainStyles.LightTextColor.FromResources<Color>()
            };

            var tapPhone = new TapGestureRecognizer
            {
                CommandParameter = contentUI.TxtPhoneValue
            };
            tapPhone.Tapped += viewModel.TxtPhone_Click;

            var stackPhone = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    txtPhoneSymbol,
                    txtPhone
                }
            };

            stackPhone.GestureRecognizers.Add(tapPhone);

            #endregion

            #region Email

            var txtEmailSymbol = new Label
            {
                Text = contentUI.TxtEmailSymbol,
                Style = LabelStyles.DescriptionStyle.FromResources<Style>(),
                TextColor = MainStyles.LightTextColor.FromResources<Color>()
            };

            var txtEmail = new Label
            {
                Text = contentUI.TxtEmailValue,
                Style = LabelStyles.LinkStyle.FromResources<Style>(),
                TextColor = MainStyles.LightTextColor.FromResources<Color>()
            };

            var tapEmail = new TapGestureRecognizer
            {
                CommandParameter = contentUI.TxtEmailValue
            };
            tapEmail.Tapped += viewModel.TxtEmail_Click;

            var stackEmail = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    txtEmailSymbol,
                    txtEmail
                }
            };

            stackEmail.GestureRecognizers.Add(tapEmail);

            #endregion

            #region Http

            var txtHttpSymbol = new Label
            {
                Text = contentUI.TxtHttpSymbol,
                Style = LabelStyles.DescriptionStyle.FromResources<Style>(),
                TextColor = MainStyles.LightTextColor.FromResources<Color>()
            };

            var txtHttp = new Label
            {
                Text = contentUI.TxtHttpValue,
                Style = LabelStyles.LinkStyle.FromResources<Style>(),
                TextColor = MainStyles.LightTextColor.FromResources<Color>()
            };

            var tapHttp = new TapGestureRecognizer
            {
                CommandParameter = contentUI.TxtHttpValue
            };
            tapHttp.Tapped += viewModel.TxtLink_Click;

            var stackHttp = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    txtHttpSymbol,
                    txtHttp
                }
            };

            stackHttp.GestureRecognizers.Add(tapHttp);

            #endregion

            var stackDeveloper = new StackLayout
            {
                Children =
                {
                    txtTitleDeveloper,
                    stackPhone,
                    stackEmail,
                    stackHttp
                }
            };

            #endregion

            var stackAbout = new StackLayout
            {
                Padding = 24,
                Spacing = 20,
                Children =
                {
                    imgLogo,
                    txtTitle,
                    stackDescription,
                    stackVersion,
                    stackDeveloper
                }
            };

            var scrollView = new ScrollView
            {
                Content = stackAbout
            };

            var safeAreaHelper = new SafeAreaHelper();
            safeAreaHelper.UseSafeArea(this, SafeAreaHelper.CustomSafeAreaFlags.Top);
            safeAreaHelper.UseSafeArea(appBar.BtnBack, SafeAreaHelper.CustomSafeAreaFlags.Left);
            safeAreaHelper.UseSafeArea(scrollView, SafeAreaHelper.CustomSafeAreaFlags.Horizontal);

            ContentLayout.Children.Add(appBar);
            ContentLayout.Children.Add(scrollView);
        }
    }
}
