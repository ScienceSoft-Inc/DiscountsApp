using ScnDiscounts.Helpers;
using ScnDiscounts.Models.Data;
using ScnDiscounts.Views;
using ScnDiscounts.Views.ContentUI;
using ScnDiscounts.Views.Styles;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ScnDiscounts.Control
{
    public class MapPinDetail : BorderBox
    {
        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);

        private bool _isDistanceAvailable;

        public bool IsDistanceAvailable
        {
            get => _isDistanceAvailable;
            set
            {
                if (_isDistanceAvailable != value)
                {
                    _isDistanceAvailable = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _title;

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        public CategoryData PrimaryCategory
        {
            set
            {
                CategoryLayout.BindingContext = null;
                CategoryLayout.BindingContext = value;
            }
        }

        private string _discountCaption;

        public string DiscountCaption
        {
            get => _discountCaption;
            set
            {
                if (_discountCaption != value)
                {
                    _discountCaption = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _discountValue;

        public string DiscountValue
        {
            get => _discountValue;
            set
            {
                if (_discountValue != value)
                {
                    _discountValue = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _distanceValue;

        public string DistanceValue
        {
            get => _distanceValue;
            set
            {
                if (_distanceValue != value)
                {
                    _distanceValue = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _distanceCaption;

        public string DistanceCaption
        {
            get => _distanceCaption;
            set
            {
                if (_discountCaption != value)
                {
                    _distanceCaption = value;
                    OnPropertyChanged();
                }
            }
        }

        protected CategoryItemTemplate CategoryLayout;

        public MapPinDetail(RootContentUI contentUI)
            : base(BorderTypeEnum.btLabel)
        {
            var gridDetail = new Grid
            {
                Padding = 6,
                BackgroundColor = Color.White,
                Opacity = 0.95,
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

            var txtTitle = new Label
            {
                Style = LabelStyles.TitleStyle.FromResources<Style>()
            };
            txtTitle.SetBinding(Label.TextProperty, new Binding(nameof(Title), source: this));

            CategoryLayout = new CategoryItemTemplate
            {
                HorizontalOptions = LayoutOptions.Start
            };

            #endregion

            #region Info

            var txtDiscountCaption = new Span();
            txtDiscountCaption.SetBinding(Span.TextProperty, new Binding(nameof(DiscountCaption), source: this));

            var txtPercentValue = new Span();
            txtPercentValue.SetBinding(Span.TextProperty, new Binding(nameof(DiscountValue), source: this));

            var txtDiscount = new Label
            {
                Style = LabelStyles.DescriptionStyle.FromResources<Style>(),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FormattedText = new FormattedString
                {
                    Spans =
                    {
                        txtDiscountCaption,
                        new Span
                        {
                            Text = " "
                        },
                        txtPercentValue
                    }
                }
            };

            var imgDistanceIcon = new Image
            {
                Source = contentUI.ImgDistance,
                HorizontalOptions = LayoutOptions.End
            };
            imgDistanceIcon.SetBinding(IsVisibleProperty, new Binding(nameof(IsDistanceAvailable), source: this));

            var txtDistanceValue = new Span();
            txtDistanceValue.SetBinding(Span.TextProperty, new Binding(nameof(DistanceValue), source: this));

            var txtDistanceCaption = new Span();
            txtDistanceCaption.SetBinding(Span.TextProperty, new Binding(nameof(DistanceCaption), source: this));

            var txtDistance = new Label
            {
                Text = DistanceValue,
                Style = LabelStyles.DescriptionStyle.FromResources<Style>(),
                HorizontalOptions = LayoutOptions.End,
                FormattedText = new FormattedString
                {
                    Spans =
                    {
                        txtDistanceValue,
                        new Span
                        {
                            Text = " "
                        },
                        txtDistanceCaption
                    }
                }
            };
            txtDistance.SetBinding(IsVisibleProperty, new Binding(nameof(IsDistanceAvailable), source: this));

            var stackInfo = new StackLayout
            {
                Spacing = 3,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    txtDiscount,
                    imgDistanceIcon,
                    txtDistance
                }
            };

            #endregion

            #region Container

            var stackDetail = new StackLayout
            {
                Children =
                {
                    txtTitle,
                    CategoryLayout,
                    stackInfo
                }
            };
            gridDetail.Children.Add(stackDetail, 0, 0);

            var imgShowDetail = new Image
            {
                Source = contentUI.ImgDetail,
                HorizontalOptions = LayoutOptions.End
            };

            gridDetail.Children.Add(imgShowDetail, 1, 0);

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
