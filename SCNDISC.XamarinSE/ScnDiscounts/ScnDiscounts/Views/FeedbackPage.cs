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

        public FeedbackPage()
            : base(typeof(FeedbackViewModel), typeof(FeedbackContentUI))
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

            var txtNameTitle = new Label
            {
                Style = LabelStyles.FeedbackLabelStyle.FromResources<Style>(),
                Text = contentUI.TxtName
            };

            var txtName = new Entry
            {
                Style = LabelStyles.FeedbackEntryStyle.FromResources<Style>(),
                MaxLength = 30
            };
            txtName.SetBinding(Entry.TextProperty, "Name");
            txtName.SetBinding(IsEnabledProperty, "IsNotLoading");

            var txtCommentTitle = new Label
            {
                Style = LabelStyles.FeedbackLabelStyle.FromResources<Style>(),
                Text = contentUI.TxtComment
            };

            var txtComment = new Editor
            {
                Style = LabelStyles.FeedbackEditorStyle.FromResources<Style>(),
                VerticalOptions = LayoutOptions.FillAndExpand,
                MaxLength = 1000
            };
            txtComment.SetBinding(Editor.TextProperty, "Comment");
            txtComment.SetBinding(IsEnabledProperty, "IsNotLoading");

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
            activityIndicator.SetBinding(IsVisibleProperty, "IsLoading");
            activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");

            var layoutFeedback = new KeyboardView
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = GridLength.Auto},
                    new RowDefinition {Height = GridLength.Auto},
                    new RowDefinition {Height = GridLength.Auto},
                    new RowDefinition {Height = GridLength.Star},
                    new RowDefinition {Height = GridLength.Auto},
                    new RowDefinition {Height = GridLength.Auto}
                },
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = 24
            };

            layoutFeedback.Children.Add(txtNameTitle, 0, 0);
            layoutFeedback.Children.Add(txtName, 0, 1);
            layoutFeedback.Children.Add(txtCommentTitle, 0, 2);
            layoutFeedback.Children.Add(txtComment, 0, 3);
            layoutFeedback.Children.Add(btnSubmit, 0, 4);
            layoutFeedback.Children.Add(activityIndicator, 0, 5);

            var safeAreaHelper = new SafeAreaHelper();
            safeAreaHelper.UseSafeArea(this, SafeAreaHelper.CustomSafeAreaFlags.Vertical);
            safeAreaHelper.UseSafeArea(appBar.BtnBack, SafeAreaHelper.CustomSafeAreaFlags.Left);
            safeAreaHelper.UseSafeArea(layoutFeedback, SafeAreaHelper.CustomSafeAreaFlags.Horizontal);

            ContentLayout.Children.Add(appBar);
            ContentLayout.Children.Add(layoutFeedback);
        }
    }
}
