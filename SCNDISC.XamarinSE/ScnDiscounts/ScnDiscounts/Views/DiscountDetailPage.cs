using ScnDiscounts.Helpers;
using ScnDiscounts.Models.Data;
using ScnDiscounts.ValueConverters;
using ScnDiscounts.ViewModels;
using ScnDiscounts.Views.ContentUI;
using ScnDiscounts.Views.Styles;
using ScnPage.Plugin.Forms;
using ScnTitleBar.Forms;
using ScnViewGestures.Plugin.Forms;
using Xamarin.Forms;

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

            #region Photo

            var imageLayout = new RelativeLayout
            {
                HeightRequest = Device.Idiom == TargetIdiom.Phone ? 200 : 400
            };

            var fileNameConverter = new FileNameToImageConverter();
            var imgPhoto = new Image
            {
                Aspect = Aspect.AspectFill
            };
            imgPhoto.SetBinding(Image.SourceProperty,
                new Binding("ImageFileName", BindingMode.Default, fileNameConverter, true));

            imageLayout.Children.Add(imgPhoto,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent(parent => parent.Width),
                Constraint.RelativeToParent(parent => parent.Height));

            #endregion

            #region Label percent

            const int sizeImgLabel = 60;

            var imgLabel = new Image
            {
                HeightRequest = sizeImgLabel,
                WidthRequest = sizeImgLabel,
                Source = contentUI.ImgPercentLabel
            };

            var labelLayout = new AbsoluteLayout();
            AbsoluteLayout.SetLayoutFlags(imgLabel, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(imgLabel, new Rectangle(0f, 0f, 1f, 1f));
            labelLayout.Children.Add(imgLabel);

            imageLayout.Children.Add(labelLayout,
                Constraint.RelativeToParent(parent => parent.Width - sizeImgLabel - 5),
                Constraint.RelativeToView(imgPhoto,
                    (parent, sibling) => sibling.Y + sibling.Height - sizeImgLabel - 5));

            var spanDiscountPercent = new Span
            {
                Style = LabelStyles.LabelPercentStyle.FromResources<Style>()
            };
            spanDiscountPercent.SetBinding(Span.TextProperty, "DiscountPercent", BindingMode.OneWay);

            var spanDiscountType = new Span
            {
                Style = LabelStyles.LabelPercentSymbolStyle.FromResources<Style>()
            };
            spanDiscountType.SetBinding(Span.TextProperty, "DiscountType", BindingMode.OneWay);

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

            foreach (var webAddress in viewModel.WebAddresses)
            {
                var imgWebAddress = new Image
                {
                    Source = webAddress.Type.GetWebAddressIcon()
                };

                var viewGesturesWebAddress = new ViewGestures
                {
                    Content = imgWebAddress,
                    AnimationEffect = ViewGestures.AnimationType.atScaling,
                    AnimationScale = -5,
                    Tag = webAddress.Url,
                    Margin = new Thickness(0, 0, 5, 5)
                };
                viewGesturesWebAddress.Tap += viewModel.TxtUrlAddress_Click;

                flexCategories.Children.Add(viewGesturesWebAddress);
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

            #region Name company

            var txtPartnerName = new Label
            {
                Style = LabelStyles.DetailTitleStyle.FromResources<Style>()
            };
            txtPartnerName.SetBinding(Label.TextProperty, "NameCompany");

            #endregion

            #region Description

            var txtDescription = new Label
            {
                Style = LabelStyles.DescriptionStyle.FromResources<Style>(),
                LineBreakMode = LineBreakMode.WordWrap
            };
            txtDescription.SetBinding(Label.TextProperty, "Description");

            #endregion

            var stackDetails = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(20, 0),
                Children =
                {
                    txtPartnerName,
                    txtDescription
                }
            };

            var discountLayout = new StackLayout
            {
                Spacing = 0,
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    imageLayout,
                    flexCategories,
                    stackDetails
                }
            };

            foreach (var brachItem in viewModel.BranchItems)
            {
                var view = new BranchInfoViewTemplate(contentUI, viewModel).View;
                view.BindingContext = brachItem;

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
