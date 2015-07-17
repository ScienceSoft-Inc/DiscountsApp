using ScnDiscounts.ViewModels;
using ScnDiscounts.Views.ContentUI;
using ScnDiscounts.Views.Styles;
using ScnPage.Plugin.Forms;
using Xamarin.Forms;

namespace ScnDiscounts.Views
{
    class SplashPage : BaseContentPage 
    {
        private SplashViewModel viewModel
        {
            get { return (SplashViewModel)BindingContext; }
        }

        private SplashContentUI contentUI
        {
            get { return (SplashContentUI)ContentUI; }
        }

        public SplashPage()
            : base(typeof(SplashViewModel), typeof(SplashContentUI))
        {
            BackgroundColor = (Color)App.Current.Resources[MainStyles.MainBackgroundColor];

            var layoutSplash = new AbsoluteLayout
            { 
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            var boxBackOpacity = new BoxView
            {
                Color = (Color)App.Current.Resources[MainStyles.MainBackgroundColor]
            };
            AbsoluteLayout.SetLayoutFlags(boxBackOpacity, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(boxBackOpacity, new Rectangle(0f, 0f, 1f, 1f));
            layoutSplash.Children.Add(boxBackOpacity);

            var imgLogo = new Image
            {
                Source = contentUI.ImgLogo,
                HeightRequest = Device.OnPlatform(-1, -1, 150),
            };
            AbsoluteLayout.SetLayoutFlags(imgLogo, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(imgLogo,
                new Rectangle(0.5, 0.4, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize)
            );
            layoutSplash.Children.Add(imgLogo);

            var actIndicator = new ActivityIndicator
            {
                Color = Device.OnPlatform(Color.White, Color.White, Color.White),
                WidthRequest = Device.OnPlatform(-1, -1, 480)
            };
            actIndicator.SetBinding(IsVisibleProperty, "IsShowLoading");
            actIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsShowLoading");
            AbsoluteLayout.SetLayoutFlags(actIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(actIndicator,
                new Rectangle(0.5, 0.8, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize)
            );
            layoutSplash.Children.Add(actIndicator);

            #region Retry button
            var btnRetry = new Button
            {
                Command = viewModel.RetryCommand,
                TextColor = Color.White,
            };
            btnRetry.SetBinding(Button.TextProperty, "BtnRetryText");
            btnRetry.SetBinding(IsVisibleProperty, "IsRetry");
            AbsoluteLayout.SetLayoutFlags(btnRetry, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(btnRetry,
                new Rectangle(0.5, 0.8, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize)
            );
            layoutSplash.Children.Add(btnRetry);
            #endregion

            var txtProgress = new Label
            {
                Style = (Style)App.Current.Resources[LabelStyles.DescriptionStyle],
                TextColor = Color.White
            };
            txtProgress.SetBinding(Label.TextProperty, "ProcessMessage");
            AbsoluteLayout.SetLayoutFlags(txtProgress, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(txtProgress,
                new Rectangle(0.5, 0.9, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize)
            );
            layoutSplash.Children.Add(txtProgress);

            ContentLayout.Children.Add(layoutSplash);
        }
    }
}
