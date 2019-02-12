using ScnDiscounts.Helpers;
using ScnDiscounts.ViewModels;
using ScnDiscounts.Views.ContentUI;
using ScnDiscounts.Views.Styles;
using Xamarin.Forms;

namespace ScnDiscounts.Views
{
    public class BranchInfoViewTemplate : ViewCell
    {
        public BranchInfoViewTemplate(DiscountDetailContentUI parentContentUi, DiscountDetailViewModel parentViewModel)
        {
            var stackBranch = new StackLayout
            {
                Padding = new Thickness(20, 10),
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            #region Location

            var gridLocation = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition {Height = GridLength.Auto}
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = GridLength.Auto},
                    new ColumnDefinition {Width = GridLength.Auto},
                    new ColumnDefinition {Width = GridLength.Star}
                }
            };

            var txtDistanceValue = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                Style = LabelStyles.DetailDistanceStyle.FromResources<Style>()
            };
            txtDistanceValue.SetBinding(Label.TextProperty, "DistanceString");

            gridLocation.Children.Add(txtDistanceValue, 0, 0);

            var distanceLabel = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 0, 10, 0),
                TranslationY = 2,
                Spacing = 0,
                Children =
                {
                    new Image
                    {
                        Source = parentContentUi.ImgDistance
                    },
                    new Label
                    {
                        Text = parentContentUi.TxtDistanceScaleValue,
                        Style = LabelStyles.DescriptionStyle.FromResources<Style>()
                    }
                }
            };

            gridLocation.Children.Add(distanceLabel, 1, 0);

            var txtPartnerAddress = new Label
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Style = LabelStyles.DescriptionStyle.FromResources<Style>()
            };
            txtPartnerAddress.SetBinding(Label.TextProperty, "Address");

            var txtShowOnMap = new Label
            {
                Text = parentContentUi.TxtShowOnMap,
                Style = LabelStyles.LinkStyle.FromResources<Style>(),
                LineBreakMode = LineBreakMode.TailTruncation
            };

            var locationLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Spacing = 0,
                Children =
                {
                    txtPartnerAddress,
                    txtShowOnMap
                }
            };

            var tapShowOnMapp = new TapGestureRecognizer();
            tapShowOnMapp.SetBinding(TapGestureRecognizer.CommandParameterProperty, "DocumentId");
            tapShowOnMapp.Tapped += parentViewModel.ShowOnMap_Click;

            locationLayout.GestureRecognizers.Add(tapShowOnMapp);

            gridLocation.Children.Add(locationLayout, 2, 0);

            stackBranch.Children.Add(gridLocation);

            #endregion

            #region Phone list

            var stackPhoneView = new StackLayout
            {
                Padding = new Thickness(0, 4)
            };

            #region phone1

            var tapPhone1 = new TapGestureRecognizer();
            tapPhone1.SetBinding(TapGestureRecognizer.CommandParameterProperty, "Phone1");
            tapPhone1.Tapped += parentViewModel.BtnCall_Click;

            var phone1 = CreateCallButton("Phone1", "PhoneOperatorIcon1");
            phone1.SetBinding(VisualElement.IsVisibleProperty, "IsPhone1Exists");
            phone1.GestureRecognizers.Add(tapPhone1);

            stackPhoneView.Children.Add(phone1);

            #endregion

            #region phone2

            var tapPhone2 = new TapGestureRecognizer();
            tapPhone2.SetBinding(TapGestureRecognizer.CommandParameterProperty, "Phone2");
            tapPhone2.Tapped += parentViewModel.BtnCall_Click;

            var phone2 = CreateCallButton("Phone2", "PhoneOperatorIcon2");
            phone2.SetBinding(VisualElement.IsVisibleProperty, "IsPhone2Exists");
            phone2.GestureRecognizers.Add(tapPhone2);

            stackPhoneView.Children.Add(phone2);

            #endregion

            stackBranch.Children.Add(stackPhoneView);

            #endregion

            View = stackBranch;
        }

        private static Frame CreateCallButton(string phoneBindingProperty, string operatorBindingProperty)
        {
            const int btnHeight = 40;

            var imgPhone = new Image
            {
                HeightRequest = btnHeight / 1.5,
                WidthRequest = btnHeight / 1.5
            };
            imgPhone.SetBinding(Image.SourceProperty, operatorBindingProperty);

            var txtPhone = new Label
            {
                Style = LabelStyles.DetailPhoneStyle.FromResources<Style>()
            };
            txtPhone.SetBinding(Label.TextProperty, phoneBindingProperty);

            var phoneLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Spacing = 10,
                Children =
                {
                    imgPhone,
                    txtPhone
                }
            };

            var borderPhone = new Frame
            {
                Padding = 0,
                CornerRadius = 1,
                BackgroundColor = Color.Transparent,
                BorderColor = MainStyles.MainBackgroundColor.FromResources<Color>(),
                HasShadow = false,
                HeightRequest = btnHeight,
                Content = phoneLayout
            };

            return borderPhone;
        }
    }
}
