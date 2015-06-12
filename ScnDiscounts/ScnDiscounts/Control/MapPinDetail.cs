using ScnDiscounts.Views.Styles;
using Xamarin.Forms;

namespace ScnDiscounts.Control
{
    public class MapPinDetail : AbsoluteLayout 
    {
        public MapPinDetail()
        {
            #region Back box
            boxBack = new BoxView
            {
                BackgroundColor = new Color(0, 0, 0, 0.01)
            };

            SetLayoutFlags(boxBack, AbsoluteLayoutFlags.All);
            SetLayoutBounds(boxBack, new Rectangle(0f, 0f, 1f, 1f));
            Children.Add(boxBack);

            var tapClosePinDetail = new TapGestureRecognizer();
            tapClosePinDetail.Tapped += (sender, e) =>
            {
                IsVisible = false;
            };
            boxBack.GestureRecognizers.Add(tapClosePinDetail);
            #endregion

            Grid gridDetail = new Grid
            {
                Padding = new Thickness(6),
                BackgroundColor = InfoPanelForegroundColor,
                Opacity = InfoPanelOpacity,

                RowDefinitions = 
                    {
                        new RowDefinition { Height = GridLength.Auto }
                    },
                ColumnDefinitions = 
                    {
                        new ColumnDefinition { Width = new GridLength(10, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
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
                Text = CategoryName,
                Style = (Style)App.Current.Resources[LabelStyles.DescriptionStyle]
            };
            var stackHeader = new StackLayout
            {
                Spacing = 3,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    txtTitle,
                    txtCategory
                }
            };

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
                Spacing = 3,
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
                Children =
                {
                    stackHeader,
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

            borderDetail.HeightRequest = Device.OnPlatform(50, 58, 70);
            borderDetail.WidthRequest = Device.OnPlatform(220, 210, 220);
            borderDetail.BorderWidth = 1;
            borderDetail.BorderColor = (Color)App.Current.Resources[MainStyles.ListBorderColor];
            borderDetail.Content = gridDetail;

            SetLayoutFlags(borderDetail, AbsoluteLayoutFlags.PositionProportional);
            SetLayoutBounds(borderDetail,
                new Rectangle(0.5, 0.35, AutoSize, AutoSize)
            );
            Children.Add(borderDetail);
        }
        
        private BorderBox borderDetail;
        private BoxView boxBack;
        private Label txtTitle;
        private Label txtCategory;
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

        private string _title = "title title title";
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
                txtCategory.Text = _categoryName;
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


        public void Show()
        {
            IsVisible = true;
        }

        public void Hide()
        {
            IsVisible = false;
        }

        #region Style
        public Color InfoPanelForegroundColor = Color.FromHex("fff");
        public double InfoPanelOpacity = 0.9;
        public double ImgIconDistanceSize = Device.OnPlatform(20, 20, 20);

        #endregion

    }
}
