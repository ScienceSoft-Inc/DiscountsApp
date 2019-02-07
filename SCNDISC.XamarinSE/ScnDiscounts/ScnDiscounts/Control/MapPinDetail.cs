using ScnDiscounts.Helpers;
using ScnDiscounts.Models.Data;
using ScnDiscounts.Views.Styles;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ScnDiscounts.Control
{
    public class MapPinDetail : BorderBox
    {
        private readonly Label _txtTitle;
        private readonly Label _txtCategory;
        private readonly ContentView _stackCategory;
        private readonly Label _txtDiscountCaption;
        private readonly Label _txtPercentValue;
        private readonly Image _imgDistanceIcon;
        private readonly Label _txtDistanceValue;
        private readonly Label _txtDistanceCaption;
        private readonly Image _imgShowDetail;

        public MapPinDetail()
            : base(BorderTypeEnum.btLabel)
        {
            var gridDetail = new Grid
            {
                Padding = 6,
                BackgroundColor = Color.White,
                Opacity = 0.9,
                RowDefinitions =
                {
                    new RowDefinition {Height = GridLength.Auto}
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(10, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                }
            };

            #region Header

            _txtTitle = new Label
            {
                Text = Title,
                Style = LabelStyles.TitleStyle.FromResources<Style>()
            };

            _txtCategory = new Label
            {
                Style = LabelStyles.CategoryStyle.FromResources<Style>(),
                Text = CategoryName
            };

            _stackCategory = new ContentView
            {
                Padding = 4,
                BackgroundColor = CategoryColor,
                Content = _txtCategory,
                HorizontalOptions = LayoutOptions.Start
            };

            #endregion

            #region Info

            _txtDiscountCaption = new Label
            {
                Text = DiscountCaption,
                Style = LabelStyles.DescriptionStyle.FromResources<Style>(),
                HorizontalOptions = LayoutOptions.Start
            };

            _txtPercentValue = new Label
            {
                Text = DiscountValue,
                Style = LabelStyles.DescriptionStyle.FromResources<Style>(),
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            _imgDistanceIcon = new Image
            {
                Source = DistanceIcon,
                HorizontalOptions = LayoutOptions.End
            };

            _txtDistanceValue = new Label
            {
                Text = DistanceValue,
                Style = LabelStyles.DescriptionStyle.FromResources<Style>(),
                HorizontalOptions = LayoutOptions.End
            };

            _txtDistanceCaption = new Label
            {
                Text = DistanceCaption,
                Style = LabelStyles.DescriptionStyle.FromResources<Style>(),
                HorizontalOptions = LayoutOptions.End
            };

            var stackInfo = new StackLayout
            {
                Spacing = 3,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    _txtDiscountCaption,
                    _txtPercentValue,
                    _imgDistanceIcon,
                    _txtDistanceValue,
                    _txtDistanceCaption
                }
            };

            #endregion

            #region Container

            var stackDetail = new StackLayout
            {
                Children =
                {
                    _txtTitle,
                    _stackCategory,
                    stackInfo
                }
            };
            gridDetail.Children.Add(stackDetail, 0, 0);

            _imgShowDetail = new Image
            {
                Source = DetailIcon,
                HorizontalOptions = LayoutOptions.End
            };

            gridDetail.Children.Add(_imgShowDetail, 1, 0);

            #endregion

            var height = Functions.OnPlatform(75, 85);

            BorderWidth = 1;
            BorderColor = MainStyles.ListBorderColor.FromResources<Color>();
            WidthRequest = Device.Idiom == TargetIdiom.Phone ? 220 : 275;
            HeightRequest = height;
            Scale = 0;
            AnchorY = 1;
            TranslationY = -height;

            Content = gridDetail;
        }

        private string _title;
        public string Title
        {
            get => _title;
            set 
            { 
                _title = value;
                _txtTitle.Text = _title;
            }
        }

        private CategoryData _primaryCategory;
        public CategoryData PrimaryCategory
        {
            get => _primaryCategory;
            set
            {
                _primaryCategory = value;

                CategoryName = value?.Name?.ToUpper();
                CategoryColor = value?.GetColorTheme() ?? Color.Transparent;
            }
        }

        private string _categoryName;
        public string CategoryName
        {
            get => _categoryName;
            private set 
            {
                _categoryName = value;
                _txtCategory.Text = value;
            }
        }

        private Color _categoryColor;
        public Color CategoryColor
        {
            get => _categoryColor;
            private set
            {
                _categoryColor = value;
                _stackCategory.BackgroundColor = _categoryColor;
            }
        }

        private string _discountCaption;
        public string DiscountCaption
        {
            get => _discountCaption;
            set 
            { 
                _discountCaption = value;
                _txtDiscountCaption.Text = _discountCaption;
            }
        }

        private string _discountValue;
        public string DiscountValue
        {
            get => _discountValue;
            set 
            {
                _discountValue = value;
                _txtPercentValue.Text = _discountValue;
            }
        }

        private string _distanceIcon;
        public string DistanceIcon
        {
            get => _distanceIcon;
            set
            {
                _distanceIcon = value;
                _imgDistanceIcon.Source = _distanceIcon;
            }
        }

        private string _distanceValue = "0";
        public string DistanceValue
        {
            get => _distanceValue;
            set 
            { 
                _distanceValue = value;
                _txtDistanceValue.Text = _distanceValue;
            }
        }

        private string _distanceCaption = "km";
        public string DistanceCaption
        {
            get => _distanceCaption;
            set 
            { 
                _distanceCaption = value;
                _txtDistanceCaption.Text = _distanceCaption;
            }
        }

        private string _detailIcon;
        public string DetailIcon
        {
            get => _detailIcon;
            set
            {
                _detailIcon = value;
                _imgShowDetail.Source = _detailIcon;
            }
        }

        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);

        public async Task Show()
        {
            await SemaphoreSlim.WaitAsync();

            try
            {
                IsVisible = true;

                ViewExtensions.CancelAnimations(this);

                await this.ScaleTo(1.1, 200, Easing.CubicIn);
                await this.ScaleTo(1, 50, Easing.CubicIn);
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }

        public async Task Hide()
        {
            await SemaphoreSlim.WaitAsync();

            try
            {
                ViewExtensions.CancelAnimations(this);

                await this.ScaleTo(0, 100, Easing.CubicOut);

                IsVisible = false;
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }
    }
}
