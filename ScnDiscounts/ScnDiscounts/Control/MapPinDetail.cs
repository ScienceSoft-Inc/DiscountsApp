using System.Threading.Tasks;
using Xamarin.Forms;
using ScnDiscounts.Views.Styles;
using ScnViewGestures.Plugin.Forms;

namespace ScnDiscounts.Control
{
    public class MapPinDetail : AbsoluteLayout 
    {
        public MapPinDetail()
        {
            #region Back box
            viewGestures = new ViewGestures()
            {
                BackgroundColor = new Color(0, 0, 0, 0.01)
            };

            SetLayoutFlags(viewGestures, AbsoluteLayoutFlags.All);
            SetLayoutBounds(viewGestures, new Rectangle(0f, 0f, 1f, 1f));
            Children.Add(viewGestures);

            viewGestures.Tap += (s, e) => { Hide(); };
            viewGestures.SwipeLeft += (s, e) => { Hide(); };
            viewGestures.SwipeRight += (s, e) => { Hide(); };
            viewGestures.SwipeUp += (s, e) => { Hide(); };
            viewGestures.SwipeDown += (s, e) => { Hide(); };

            #endregion

            Grid gridDetail = new Grid
            {
                Padding = Device.OnPlatform(new Thickness(6), new Thickness(6), new Thickness(8)) ,
                BackgroundColor = InfoPanelForegroundColor,
                Opacity = InfoPanelOpacity,

                RowDefinitions = 
                    {
                        new RowDefinition { Height = GridLength.Auto }
                    },
                ColumnDefinitions = 
                    {
                        new ColumnDefinition { Width = new GridLength(10, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    }
            };

            #region Header
            txtTitle = new Label
            {
                Text = Title,
                Style = (Style)App.Current.Resources[LabelStyles.TitleStyle]
            };

            txtCategory = new Label
            {
                Style = (Style)App.Current.Resources[LabelStyles.CategoryStyle],
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Text = CategoryName.ToUpper(),
            };

            stackCategory = new StackLayout
            {
                Padding = Device.OnPlatform(new Thickness(4), new Thickness(4), new Thickness(6)),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                BackgroundColor = CategoryColor,
                Children =
                    {
                        txtCategory
                    }
            };

            Grid gridHeader = new Grid
            {
                BackgroundColor = Color.Transparent,

                RowDefinitions = 
                    {
                        new RowDefinition { Height = GridLength.Auto },
                    },
                ColumnDefinitions = 
                    {
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        new ColumnDefinition { Width = GridLength.Auto }//new GridLength(1, GridUnitType.Star) },
                    }
            };

            gridHeader.Children.Add(txtTitle, 0, 0);
            gridHeader.Children.Add(stackCategory, 1, 0);
            #endregion

            #region Info
            txtDiscountCaption = new Label
            {
                Text = DiscountCaption,
                Style = (Style)App.Current.Resources[LabelStyles.DescriptionStyle]
            };
            
            txtPercentValue = new Label
            {
                Text = DiscountValue,
                Style = (Style)App.Current.Resources[LabelStyles.DescriptionStyle]
            };

            imgDistanceIcon = new Image
            {
                Source = DistanceIcon
            };

            txtDistanceValue = new Label
            {
                Text = DistanceValue,
                Style = (Style)App.Current.Resources[LabelStyles.DescriptionStyle]
            };

            txtDistanceCaption = new Label
            {
                Text = DistanceCaption,
                Style = (Style)App.Current.Resources[LabelStyles.DescriptionStyle]
            };

            var stackInfo = new StackLayout
            {
                Spacing = Device.OnPlatform(3, 3, 3),
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    txtDiscountCaption,
                    txtPercentValue,
                    imgDistanceIcon,
                    txtDistanceValue,
                    txtDistanceCaption
                }
            };

            #endregion

            var stackDetail = new StackLayout
            {
                Spacing = Device.OnPlatform (2, 4, 8),
                Children =
                {
                    gridHeader,
                    stackInfo
                }
            };
            gridDetail.Children.Add(stackDetail, 0, 0);

            imgShowDetail = new Image
            {
                Source = DetailIcon,
                HorizontalOptions = LayoutOptions.End
            };

            gridDetail.Children.Add(imgShowDetail, 1, 0);

            borderDetail = new BorderBox(BorderBox.BorderTypeEnum.btLabel);

            borderDetail.HeightRequest = Device.OnPlatform(50, 58, 84);
            borderDetail.WidthRequest = Device.OnPlatform(210, 210, 280);
            borderDetail.BorderWidth = 1;
            borderDetail.BorderColor = (Color)App.Current.Resources[MainStyles.ListBorderColor];
            borderDetail.Content = gridDetail;

            SetLayoutFlags(borderDetail, AbsoluteLayoutFlags.PositionProportional);
            SetLayoutBounds(borderDetail, Device.OnPlatform(
                new Rectangle(0.5, 0.365, AutoSize, AutoSize),
                new Rectangle(0.5, 0.360, AutoSize, AutoSize),
                new Rectangle(0.5, 0.355, AutoSize, AutoSize))
            );

            borderDetail.Scale = 0;
            borderDetail.AnchorY = 1;

            Children.Add(borderDetail);
        }
        
        private BorderBox borderDetail;
        private ViewGestures viewGestures;
        private Label txtTitle;
        private Label txtCategory;
        private StackLayout stackCategory;
        private Label txtDiscountCaption;
        private Label txtPercentValue;
        private Image imgDistanceIcon;
        private Label txtDistanceValue;
        private Label txtDistanceCaption;
        private Image imgShowDetail;

        public Color _borderColor = Color.Transparent;
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
            }
        }

        public TapGestureRecognizer TapPinDetail 
        { 
            set
            {
                borderDetail.TapGesture = value;
            }
        }

        private string _title = "title";
        public string Title
        {
            get { return _title; }
            set 
            { 
                _title = value;
                txtTitle.Text = _title;
            }
        }

        private string _categoryName = "empty";
        public string CategoryName
        {
            get { return _categoryName; }
            set 
            { 
                _categoryName = value;
                txtCategory.Text = _categoryName.ToUpper();
            }
        }

        private Color _categoryColor = Color.Transparent;
        public Color CategoryColor
        {
            get { return _categoryColor; }
            set
            {
                _categoryColor = value;
                stackCategory.BackgroundColor = _categoryColor;
            }
        }

        private string _discountCaption = "Discount";
        public string DiscountCaption
        {
            get { return _discountCaption; }
            set 
            { 
                _discountCaption = value;
                txtDiscountCaption.Text = _discountCaption;
            }
        }

        private string _discountValue = "0";
        public string DiscountValue
        {
            get { return _discountValue + "%"; }
            set 
            {
                _discountValue = value;
                txtPercentValue.Text = _discountValue;
            }
        }

        private string _distanceIcon = "";
        public string DistanceIcon
        {
            get { return _distanceIcon; }
            set
            {
                _distanceIcon = value;
                imgDistanceIcon.Source = _distanceIcon;
            }
        }

        private string _distanceValue = "0";
        public string DistanceValue
        {
            get { return _distanceValue; }
            set 
            { 
                _distanceValue = value;
                txtDistanceValue.Text = _distanceValue;
            }
        }

        private string _distanceCaption = "km";
        public string DistanceCaption
        {
            get { return _distanceCaption; }
            set 
            { 
                _distanceCaption = value;
                txtDistanceCaption.Text = _distanceCaption;
            }
        }

        private string _detailIcon = "";
        public string DetailIcon
        {
            get { return _detailIcon; }
            set
            {
                _detailIcon = value;
                imgShowDetail.Source = _detailIcon;
            }
        }

        static object locker = new object();
        async public Task Show()
        {
            IsVisible = true;

            ViewExtensions.CancelAnimations(borderDetail);
            await borderDetail.ScaleTo(1.1, 200, Easing.CubicIn);
            await borderDetail.ScaleTo(1, 50, Easing.CubicIn);
        }

        async public Task Hide()
        {
            ViewExtensions.CancelAnimations(borderDetail);
            await borderDetail.ScaleTo(0, 100, Easing.CubicOut);

            IsVisible = false;
        }

        #region Style
        public Color InfoPanelForegroundColor = Color.FromHex("fff");
        public double InfoPanelOpacity = 0.9;
        public double ImgIconDistanceSize = Device.OnPlatform(20, 20, 20);

        #endregion

    }
}
