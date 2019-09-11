using CarouselView.FormsPlugin.Abstractions;
using ScnDiscounts.Control;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models.Data;
using ScnDiscounts.ViewModels;
using ScnDiscounts.Views.ContentUI;
using ScnDiscounts.Views.Styles;
using ScnPage.Plugin.Forms;
using ScnTitleBar.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ScnDiscounts.Views
{
    public class DiscountDetailPage : BaseContentPage
    {
        private DiscountDetailViewModel viewModel => (DiscountDetailViewModel) ViewModel;

        private DiscountDetailContentUI contentUI => (DiscountDetailContentUI) ContentUI;

        public DiscountDetailPage(DiscountDetailData discountDetailData)
            : base(typeof(DiscountDetailViewModel), typeof(DiscountDetailContentUI))
        {
            BackgroundColor = MainStyles.StatusBarColor.FromResources<Color>();
            Content.BackgroundColor = MainStyles.MainLightBackgroundColor.FromResources<Color>();

            var loadingColor = MainStyles.LoadingColor.FromResources<Color>();
            LoadingActivityIndicator.Color = loadingColor;
            LoadingActivityText.TextColor = loadingColor;

            viewModel.SetDiscount(discountDetailData);

            var mainLayout = new AbsoluteLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            var appBar = new TitleBar(this, TitleBar.BarBtnEnum.bbBack)
            {
                BarColor = Color.Transparent,
                BoxPadding =
                {
                    BackgroundColor = MainStyles.StatusBarColor.FromResources<Color>()
                },
                BtnBack =
                {
                    BackgroundColor = MainStyles.StatusBarColor.FromResources<Color>(),
                    Source = contentUI.IconBack
                }
            };

            #region Label percent

            const int sizeImgLabel = 60;

            var imgLabel = new Image
            {
                HeightRequest = sizeImgLabel,
                WidthRequest = sizeImgLabel,
                Source = contentUI.ImgPercentLabel
            };

            var labelLayout = new AbsoluteLayout
            {
                InputTransparent = true
            };
            AbsoluteLayout.SetLayoutFlags(imgLabel, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(imgLabel, new Rectangle(0f, 0f, 1f, 1f));
            labelLayout.Children.Add(imgLabel);

            var spanDiscountPercent = new Span
            {
                Style = LabelStyles.LabelPercentStyle.FromResources<Style>()
            };
            spanDiscountPercent.SetBinding(Span.TextProperty, nameof(DiscountDetailViewModel.DiscountPercent));

            var spanDiscountType = new Span
            {
                Style = LabelStyles.LabelPercentSymbolStyle.FromResources<Style>()
            };
            spanDiscountType.SetBinding(Span.TextProperty, nameof(DiscountDetailViewModel.DiscountType));

            var txtPercent = new Label
            {
                Rotation = -15,
                TranslationY = -1,
                FormattedText = new FormattedString
                {
                    Spans =
                    {
                        spanDiscountPercent,
                        spanDiscountType
                    }
                }
            };

            AbsoluteLayout.SetLayoutFlags(txtPercent, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(txtPercent,
                new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            labelLayout.Children.Add(txtPercent);

            #endregion

            #region WebAddress & Category list

            var flexCategories = new FlexLayout
            {
                Direction = FlexDirection.Row,
                Wrap = FlexWrap.Wrap,
                JustifyContent = FlexJustify.Start,
                AlignItems = FlexAlignItems.Center,
                AlignContent = FlexAlignContent.Start,
                Margin = new Thickness(20, 15, 15, 0)
            };

            const int sizeWebAddress = 40;

            foreach (var webAddress in viewModel.WebAddresses)
            {
                var imgWebAddress = new Image
                {
                    Source = webAddress.Type.GetWebAddressIcon(),
                    Margin = new Thickness(0, 0, 5, 5),
                    WidthRequest = sizeWebAddress,
                    HeightRequest = sizeWebAddress
                };

                var tapWebAddress = new TapGestureRecognizer
                {
                    CommandParameter = webAddress.Url
                };
                tapWebAddress.Tapped += viewModel.TxtUrlAddress_Click;

                imgWebAddress.GestureRecognizers.Add(tapWebAddress);

                flexCategories.Children.Add(imgWebAddress);
            }

            var flexSeparator = new ContentView();
            FlexLayout.SetGrow(flexSeparator, 1);
            flexCategories.Children.Add(flexSeparator);

            foreach (var category in viewModel.Categories)
            {
                var categoryLayout = new CategoryItemTemplate
                {
                    Margin = new Thickness(0, 0, 5, 5),
                    BindingContext = category
                };

                flexCategories.Children.Add(categoryLayout);
            }

            #endregion

            #region Carousel images

            var carouselView = new CarouselViewControl
            {
                Orientation = CarouselViewOrientation.Horizontal,
                ItemTemplate = new DataTemplate(typeof(GalleryImageItemTemplate)),
                CurrentPageIndicatorTintColor = MainStyles.MainBackgroundColor.FromResources<Color>()
            };
            carouselView.SetBinding(CarouselViewControl.ItemsSourceProperty,
                nameof(DiscountDetailViewModel.GalleryImages));
            carouselView.SetBinding(CarouselViewControl.ShowIndicatorsProperty,
                nameof(DiscountDetailViewModel.HasGalleryImages));
            carouselView.SetBinding(CarouselViewControl.IsSwipeEnabledProperty,
                nameof(DiscountDetailViewModel.HasGalleryImages));

            var carouselLayout = new RelativeLayout
            {
                VerticalOptions = LayoutOptions.Start
            };

            carouselLayout.Children.Add(carouselView,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent(parent => parent.Width),
                Constraint.RelativeToParent(parent =>
                    Device.Info.CurrentOrientation.IsPortrait() ? parent.Width * 0.5625 :
                    Device.Idiom == TargetIdiom.Phone ? 200 : 400));

            carouselLayout.Children.Add(labelLayout,
                Constraint.RelativeToView(carouselView, (parent, view) => view.Width - sizeImgLabel - 5),
                Constraint.RelativeToView(carouselView, (parent, view) => view.Height - sizeImgLabel - 5));

            #endregion

            #region Name company

            var txtPartnerName = new Label
            {
                Style = LabelStyles.DetailTitleStyle.FromResources<Style>(),
                Margin = new Thickness(20, 0)
            };
            txtPartnerName.SetBinding(Label.TextProperty, nameof(DiscountDetailViewModel.NameCompany));

            #endregion

            #region Description

            var txtDescription = new Label
            {
                Style = LabelStyles.DescriptionStyle.FromResources<Style>(),
                Margin = new Thickness(20, 10, 20, 0),
                LineBreakMode = LineBreakMode.WordWrap
            };
            txtDescription.SetBinding(Label.TextProperty, nameof(DiscountDetailViewModel.Description));

            #endregion

            #region Rating view

            var ratingView = new RatingView
            {
                Style = LabelStyles.RatingStyle.FromResources<Style>()
            };
            ratingView.SetBinding(RatingView.ValueProperty, nameof(DiscountDetailViewModel.UserRating));

            var ratingLabel = new Label
            {
                Style = LabelStyles.DescriptionStyle.FromResources<Style>(),
                VerticalOptions = LayoutOptions.End
            };
            ratingLabel.SetBinding(Label.TextProperty, nameof(DiscountDetailViewModel.RatingString));
            ratingLabel.SetBinding(IsVisibleProperty, nameof(DiscountDetailViewModel.IsDiscountRatingAvailable));

            var activityIndicator = new ActivityIndicator
            {
                Color = Color.FromHex("444"),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,
                WidthRequest = 20,
                HeightRequest = 20
            };
            activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, nameof(DiscountDetailViewModel.IsLoadingRating));
            activityIndicator.SetBinding(IsVisibleProperty, nameof(DiscountDetailViewModel.IsLoadingRating));

            var stackRating = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Margin = new Thickness(20, 0),
                Spacing = 10,
                Children =
                {
                    ratingView,
                    ratingLabel,
                    activityIndicator
                }
            };

            #endregion

            var discountLayout = new StackLayout
            {
                Spacing = 0,
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    carouselLayout,
                    flexCategories,
                    txtPartnerName,
                    stackRating,
                    txtDescription
                }
            };

            foreach (var brachItem in viewModel.BranchItems)
            {
                var view = new BranchInfoViewTemplate(contentUI, viewModel)
                {
                    BindingContext = brachItem
                };

                discountLayout.Children.Add(view);
            }

            var scrollDiscount = new ScrollView
            {
                Content = discountLayout
            };

            AbsoluteLayout.SetLayoutFlags(scrollDiscount, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(scrollDiscount, new Rectangle(0f, 0f, 1f, 1f));
            mainLayout.Children.Add(scrollDiscount);

            AbsoluteLayout.SetLayoutFlags(appBar,
                AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);
            AbsoluteLayout.SetLayoutBounds(appBar, new Rectangle(0f, 0f, 1f, AbsoluteLayout.AutoSize));
            mainLayout.Children.Add(appBar);

            var safeAreaHelper = new SafeAreaHelper();
            safeAreaHelper.UseSafeArea(this, SafeAreaHelper.CustomSafeAreaFlags.Top);
            safeAreaHelper.UseSafeArea(appBar.BtnBack, SafeAreaHelper.CustomSafeAreaFlags.Left);
            safeAreaHelper.UseSafeArea(scrollDiscount, SafeAreaHelper.CustomSafeAreaFlags.Horizontal);

            ContentLayout.Children.Add(mainLayout);
        }
    }
}
