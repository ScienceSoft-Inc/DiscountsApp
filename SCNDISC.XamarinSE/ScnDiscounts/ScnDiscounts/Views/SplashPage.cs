using ScnDiscounts.Control;
using ScnDiscounts.Helpers;
using ScnDiscounts.ViewModels;
using ScnDiscounts.Views.ContentUI;
using ScnDiscounts.Views.Styles;
using ScnPage.Plugin.Forms;
using Xamarin.Forms;

namespace ScnDiscounts.Views
{
    public class SplashPage : BaseContentPage
    {
        private SplashViewModel viewModel => (SplashViewModel) ViewModel;

        private SplashContentUI contentUI => (SplashContentUI) ContentUI;

        public SplashPage()
            : base(typeof(SplashViewModel), typeof(SplashContentUI))
        {
            NavigationPage.SetHasNavigationBar(this, false);

            BackgroundColor = MainStyles.MainBackgroundColor.FromResources<Color>();

            var loadingColor = MainStyles.LoadingColor.FromResources<Color>();
            LoadingActivityIndicator.Color = loadingColor;
            LoadingActivityText.TextColor = loadingColor;

            var layoutSplash = new AbsoluteLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            var imgLogo = new Image
            {
                Source = contentUI.ImgLogo
            };
            AbsoluteLayout.SetLayoutFlags(imgLogo, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(imgLogo,
                new Rectangle(0.5, 0.4, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            layoutSplash.Children.Add(imgLogo);

            var activityIndicator = new ActivityIndicator
            {
                Color = Color.White
            };
            activityIndicator.SetBinding(IsVisibleProperty, "IsShowLoading");
            activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsShowLoading");
            AbsoluteLayout.SetLayoutFlags(activityIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(activityIndicator,
                new Rectangle(0.5, 0.8, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            layoutSplash.Children.Add(activityIndicator);

            var btnRetry = new ButtonExtended
            {
                Style = LabelStyles.ButtonStyle.FromResources<Style>(),
                Command = viewModel.RetryCommand,
                Text = contentUI.BtnTxtRetry.ToUpper()
            };
            btnRetry.SetBinding(IsVisibleProperty, "IsRetry");
            AbsoluteLayout.SetLayoutFlags(btnRetry,
                AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);
            AbsoluteLayout.SetLayoutBounds(btnRetry, new Rectangle(0.5, 0.7, 0.5f, AbsoluteLayout.AutoSize));
            layoutSplash.Children.Add(btnRetry);

            var btnSkip = new ButtonExtended
            {
                Style = LabelStyles.ButtonStyle.FromResources<Style>(),
                Command = viewModel.SkipCommand,
                Text = contentUI.BtnTxtSkip.ToUpper()
            };
            btnSkip.SetBinding(IsVisibleProperty, "IsRetry");
            AbsoluteLayout.SetLayoutFlags(btnSkip,
                AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);
            AbsoluteLayout.SetLayoutBounds(btnSkip, new Rectangle(0.5, 0.8, 0.5f, AbsoluteLayout.AutoSize));
            layoutSplash.Children.Add(btnSkip);

            var txtProgress = new Label
            {
                Style = LabelStyles.DescriptionLightStyle.FromResources<Style>(),
                HorizontalTextAlignment = TextAlignment.Center
            };
            txtProgress.SetBinding(Label.TextProperty, "ProcessMessage");
            AbsoluteLayout.SetLayoutFlags(txtProgress,
                AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);
            AbsoluteLayout.SetLayoutBounds(txtProgress, new Rectangle(0f, 0.9, 1f, AbsoluteLayout.AutoSize));
            layoutSplash.Children.Add(txtProgress);

            ContentLayout.Children.Add(layoutSplash);
        }
    }
}
