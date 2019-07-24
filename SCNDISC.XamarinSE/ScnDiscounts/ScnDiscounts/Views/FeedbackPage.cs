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
    public class FeedbackPage : BaseContentPage
    {
        private FeedbackViewModel viewModel => (FeedbackViewModel) ViewModel;

        private FeedbackContentUI contentUI => (FeedbackContentUI) ContentUI;

        private readonly Entry _txtName;

        private readonly Editor _txtComment;

        private readonly KeyboardView _layoutFeedback;

        public FeedbackPage()
            : base(typeof(FeedbackViewModel), typeof(FeedbackContentUI))
        {
            BackgroundColor = MainStyles.StatusBarColor.FromResources<Color>();
            Content.BackgroundColor = MainStyles.MainBackgroundColor.FromResources<Color>();

            var loadingColor = MainStyles.LoadingColor.FromResources<Color>();
            LoadingActivityIndicator.Color = loadingColor;
            LoadingActivityText.TextColor = loadingColor;

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

            var txtNameTitle = new Label
            {
                Style = LabelStyles.FeedbackLabelStyle.FromResources<Style>(),
                Text = contentUI.TxtName
            };

            _txtName = new Entry
            {
                Style = LabelStyles.FeedbackEntryStyle.FromResources<Style>(),
                MaxLength = 30
            };
            _txtName.SetBinding(Entry.TextProperty, "Name");
            _txtName.SetBinding(IsEnabledProperty, "IsNotLoading");

            var txtCommentTitle = new Label
            {
                Style = LabelStyles.FeedbackLabelStyle.FromResources<Style>(),
                Text = contentUI.TxtComment
            };

            _txtComment = new Editor
            {
                Style = LabelStyles.FeedbackEditorStyle.FromResources<Style>(),
                VerticalOptions = LayoutOptions.FillAndExpand,
                MaxLength = 1000
            };
            _txtComment.SetBinding(Editor.TextProperty, "Comment");
            _txtComment.SetBinding(IsEnabledProperty, "IsNotLoading");

            var btnSubmit = new ButtonExtended
            {
                Style = LabelStyles.ButtonStyle.FromResources<Style>(),
                Text = contentUI.TxtSubmit.ToUpper(),
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = 150
            };
            btnSubmit.SetBinding(IsEnabledProperty, "IsValidFeedback");
            btnSubmit.SetBinding(IsVisibleProperty, "IsNotLoading");
            btnSubmit.Clicked += viewModel.BtnSubmit_Clicked;

            var activityIndicator = new ActivityIndicator
            {
                Color = Color.White,
                HorizontalOptions = LayoutOptions.Center
            };
            activityIndicator.SetBinding(IsVisibleProperty, "IsSubmitting");
            activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsSubmitting");

            _layoutFeedback = new KeyboardView
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = GridLength.Auto},
                    new RowDefinition {Height = GridLength.Auto},
                    new RowDefinition {Height = GridLength.Auto},
                    new RowDefinition {Height = GridLength.Star},
                    new RowDefinition {Height = GridLength.Auto},
                    new RowDefinition {Height = GridLength.Auto}
                }
            };

            _layoutFeedback.Children.Add(txtNameTitle, 0, 0);
            _layoutFeedback.Children.Add(_txtName, 0, 1);
            _layoutFeedback.Children.Add(txtCommentTitle, 0, 2);
            _layoutFeedback.Children.Add(_txtComment, 0, 3);
            _layoutFeedback.Children.Add(btnSubmit, 0, 4);
            _layoutFeedback.Children.Add(activityIndicator, 0, 5);

            var layoutContainer = new ContentView
            {
                Padding = 24,
                Content = _layoutFeedback,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            var safeAreaHelper = new SafeAreaHelper();
            safeAreaHelper.UseSafeArea(this, SafeAreaHelper.CustomSafeAreaFlags.Top);
            safeAreaHelper.UseSafeArea(appBar.BtnBack, SafeAreaHelper.CustomSafeAreaFlags.Left);
            safeAreaHelper.UseSafeArea(layoutContainer, SafeAreaHelper.CustomSafeAreaFlags.Horizontal);

            ContentLayout.Children.Add(appBar);
            ContentLayout.Children.Add(layoutContainer);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _layoutFeedback.MinimumHeightRequest = _layoutFeedback.Height - _txtComment.Height + _txtName.Height;
        }
    }
}
