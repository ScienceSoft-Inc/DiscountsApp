using FFImageLoading.Forms;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models.Data;
using ScnDiscounts.ValueConverters;
using ScnDiscounts.Views.Styles;
using System.Globalization;
using Xamarin.Forms;

namespace ScnDiscounts.Views
{
    public class DiscountItemTemplate : ViewCell
    {
        protected static readonly FileNameToImageConverter FileNameConverter = new FileNameToImageConverter();

        protected CachedImage ImgCompanyLogo;

        public DiscountItemTemplate()
        {
            var gridDiscountItem = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition {Height = GridLength.Auto},
                    new RowDefinition {Height = GridLength.Auto},
                    new RowDefinition {Height = GridLength.Star}
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = GridLength.Auto},
                    new ColumnDefinition {Width = GridLength.Star},
                    new ColumnDefinition {Width = GridLength.Auto}
                }
            };

            ImgCompanyLogo = new CachedImage
            {
                WidthRequest = 64,
                HeightRequest = 64,
                Aspect = Aspect.AspectFit,
                DownsampleToViewSize = true,
                ErrorPlaceholder = "img_empty_small.png"
            };

            gridDiscountItem.Children.Add(ImgCompanyLogo, 0, 1, 0, 3);

            var txtTitle = new Label
            {
                VerticalOptions = LayoutOptions.End,
                Style = LabelStyles.ListTitleStyle.FromResources<Style>()
            };
            txtTitle.SetBinding(Label.TextProperty, "Name");

            gridDiscountItem.Children.Add(txtTitle, 1, 0);

            #region Percent label

            var spanDiscountPercent = new Span();
            spanDiscountPercent.SetBinding(Span.TextProperty, "DiscountPercent");

            var spanDiscountType = new Span();
            spanDiscountType.SetBinding(Span.TextProperty, "DiscountType");

            var txtPercent = new Label
            {
                Style = LabelStyles.ListPercentStyle.FromResources<Style>(),
                VerticalOptions = LayoutOptions.End,
                FormattedText = new FormattedString
                {
                    Spans =
                    {
                        spanDiscountPercent,
                        spanDiscountType
                    }
                }
            };

            gridDiscountItem.Children.Add(txtPercent, 2, 0);

            #endregion

            #region Category layout

            var categoriesLayout = new StackLayout
            {
                Spacing = 5,
                Orientation = StackOrientation.Horizontal
            };

            var categoryItemTemplate = new DataTemplate(typeof(CategoryItemTemplate));
            BindableLayout.SetItemTemplate(categoriesLayout, categoryItemTemplate);

            categoriesLayout.SetBinding(BindableLayout.ItemsSourceProperty, "CategoryList");

            gridDiscountItem.Children.Add(categoriesLayout, 1, 3, 1, 2);

            #endregion

            #region Description

            var txtDescription = new Label
            {
                Style = LabelStyles.DescriptionStyle.FromResources<Style>(),
                MaxLines = 3
            };
            txtDescription.SetBinding(Label.TextProperty, "Description");

            gridDiscountItem.Children.Add(txtDescription, 1, 3, 2, 3);

            #endregion

            var boxBorder = new Frame
            {
                Margin = new Thickness(8, 4),
                Padding = 8,
                CornerRadius = 1,
                BackgroundColor = MainStyles.ListBackgroundColor.FromResources<Color>(),
                BorderColor = MainStyles.ListBorderColor.FromResources<Color>(),
                HasShadow = false,
                Content = gridDiscountItem
            };

            View = boxBorder;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var viewModel = (DiscountData) BindingContext;
            RebindLogoImage(ImgCompanyLogo, viewModel);
        }

        private static void RebindLogoImage(CachedImage imgCompanyLogo, DiscountData viewModel)
        {
            imgCompanyLogo.Source = null;

            var imagePath = viewModel != null
                ? (string) FileNameConverter.Convert(viewModel.LogoFileName, typeof(string), false,
                    CultureInfo.CurrentCulture)
                : null;

            if (!string.IsNullOrEmpty(imagePath))
                imgCompanyLogo.Source = imagePath;
        }
    }
}
