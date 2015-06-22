using ScnDiscounts.Control;
using ScnDiscounts.Control.Pages;
using ScnDiscounts.Models.Data;
using ScnDiscounts.ValueConverter;
using ScnDiscounts.ViewModels;
using ScnDiscounts.Views.ContentUI;
using ScnDiscounts.Views.Styles;
using Xamarin.Forms;

namespace ScnDiscounts.Views
{
    class DiscountDetailPage : BaseContentPage
    {
        private DiscountDetailViewModel viewModel
        {
            get { return (DiscountDetailViewModel)BindingContext; }
        }

        private DiscountDetailContentUI contentUI
        {
            get { return (DiscountDetailContentUI)ContentUI; }
        }

        public ListView BranchListView;
        private StackLayout discountLayout;

        public DiscountDetailPage(DiscountData discountData)
            : base(typeof(DiscountDetailViewModel), typeof(DiscountDetailContentUI))
        {
            viewModel.SetDiscount(discountData);

            BackgroundColor = (Color)App.Current.Resources[MainStyles.MainLightBackgroundColor];

            var mainLayout = new AbsoluteLayout();

            var appBar = new CustomAppBar(this, CustomAppBar.CustomBarBtnEnum.cbBack)
            {
                BackgroundColor = Color.Transparent
            };
            appBar.BtnBack.BackgroundColor = Color.Black;
            appBar.BtnBack.Opacity = 0.6;
            appBar.BtnBack.Source = contentUI.IconBack;
            appBar.BtnBack.WidthRequest = appBar.HeightRequest;
            appBar.BtnBack.HeightRequest = appBar.HeightRequest;

            discountLayout = new StackLayout
            { 
                Spacing = Device.OnPlatform(0, 0, 4),
            };

            #region Photo
            var imageLayout = new RelativeLayout 
            {
                HeightRequest = Device.OnPlatform(200, 200, 240)
            };

            var imgPhoto = new Image
            {
                Aspect = Aspect.AspectFill,
            };
            imgPhoto.SetBinding(Image.SourceProperty, new Binding("ImgPhoto", BindingMode.Default, new FileStreamToImageSource(), FileStreamToImageSource.SizeImage.siBig));

            imageLayout.Children.Add(imgPhoto,
                Constraint.Constant(0),
                Constraint.Constant(0), Constraint.RelativeToParent(parent => { return parent.Width; }), Constraint.RelativeToParent(parent => { return parent.Height; })
                );
            #endregion

            #region Label percent
            int sizeImgLabel = Device.OnPlatform(60, 60, 80);

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
                Constraint.RelativeToParent(parent =>
                {
                    return parent.Width - sizeImgLabel - 5;
                }),
                Constraint.RelativeToView(imgPhoto, (parent, sibling) =>
                {
                    return sibling.Y + sibling.Height - sizeImgLabel - 5;
                }));

            // Percent
            var txtPercent = new Label
            {
                Style = (Style)App.Current.Resources[LabelStyles.LabelPercentStyle]
            };
            txtPercent.SetBinding(Label.TextProperty, "DiscountPercent");

            var percentLayout = new StackLayout
            {
                Spacing = 0,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    txtPercent,
                    new Label
                    { 
                        Style = (Style)App.Current.Resources[LabelStyles.LabelPercentSymbolStyle],
                        VerticalOptions = LayoutOptions.End
                    }
                }
            };
            
            percentLayout.Rotation = -15; 
            AbsoluteLayout.SetLayoutFlags(percentLayout, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(percentLayout,
                new Rectangle(0.6, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize)
            );
            labelLayout.Children.Add(percentLayout);

            discountLayout.Children.Add(imageLayout);
            #endregion

            #region Header
            Grid gridHeader = new Grid
            {
                Padding = new Thickness(10),
                RowDefinitions = 
                    {
                        new RowDefinition { Height = GridLength.Auto }
                    },
                ColumnDefinitions = 
                    {
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) }
                    }
            };

            #region Company logo
            var imgCompanyLogo = new Image
            {
                WidthRequest = Device.OnPlatform(64, 64, 64),
                HeightRequest = Device.OnPlatform(64, 64, 64),
                Aspect = Aspect.AspectFit
            };
            imgCompanyLogo.SetBinding(Image.SourceProperty, new Binding("ImgLogo", BindingMode.Default, new FileStreamToImageSource(), FileStreamToImageSource.SizeImage.siSmall));

            var stackCompanyLogo = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children = { imgCompanyLogo }
            };

            gridHeader.Children.Add(stackCompanyLogo, 0, 0);
            #endregion

            #region Category list
            var stackCategories = new StackLayout
            {
                Spacing = 10,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.End
            };

            for (var i = 0; i < viewModel.CategoriesCount; i++)
            {
                var txtCategory = new Label
                {
                    Style = (Style)App.Current.Resources[LabelStyles.CategoryStyle]
                };

                txtCategory.Text = viewModel.CategoryIndexName(i);
                txtCategory.BackgroundColor = viewModel.CategoryIndexColor(i);

                var categoryLayout = new StackLayout
                {
                    Padding = Device.OnPlatform(new Thickness(4), new Thickness(4), new Thickness(6)),
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.End,
                    Children =
                        {
                            txtCategory
                        }
                };
                categoryLayout.BackgroundColor = viewModel.CategoryIndexColor(i);

                stackCategories.Children.Add(categoryLayout);
            }
            #endregion

            var titleDetailLayout = new StackLayout
            {
                Padding = new Thickness (2, 0, 0, 0),
                Spacing = Device.OnPlatform(0, 0, 4),
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            titleDetailLayout.Children.Add(stackCategories);

            #region Name company
            var txtPartnerName = new Label
            {
                Style = (Style)App.Current.Resources[LabelStyles.DetailTitleStyle],
                HorizontalOptions = LayoutOptions.Start
            };
            txtPartnerName.SetBinding(Label.TextProperty, "NameCompany");
            titleDetailLayout.Children.Add(txtPartnerName);
            #endregion

            #region Url address
            var txtUrlAddress = new LabelExtended
            {
                Style = (Style)App.Current.Resources[LabelStyles.LinkStyle],
                TextColor = Color.FromHex("777"),
                HorizontalOptions = LayoutOptions.Start
            };
            txtUrlAddress.SetBinding(Label.TextProperty, "UrlAddress");
            txtUrlAddress.Click += viewModel.txtUrlAddress_Click;
            titleDetailLayout.Children.Add(txtUrlAddress);
            #endregion

            gridHeader.Children.Add(titleDetailLayout, 1, 0);

            discountLayout.Children.Add(gridHeader);
            #endregion

            #region Description
            var txtDescription = new Label
            {
                VerticalOptions = LayoutOptions.Start,
                Style = (Style)App.Current.Resources[LabelStyles.DescriptionStyle],
                LineBreakMode = LineBreakMode.WordWrap
            };
            txtDescription.SetBinding(Label.TextProperty, "Description");
            var descriptionLayout = new StackLayout
            {
                Padding = new Thickness(18, 0),
                Children =
                {
                    txtDescription
                }
            };
            discountLayout.Children.Add(descriptionLayout);
            #endregion

            var scrollDiscount = new ScrollView
            {
                Content = discountLayout,
            };

            AbsoluteLayout.SetLayoutFlags(scrollDiscount, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(scrollDiscount, new Rectangle(0f, 0f, 1f, 1f));
            mainLayout.Children.Add(scrollDiscount);

            AbsoluteLayout.SetLayoutFlags(appBar, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(appBar,
                new Rectangle(0, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            mainLayout.Children.Add(appBar);

            ContentLayout.Children.Add(mainLayout);
        }

        public void InitBranchListView()
        {
            #region Branch
            BranchListView = new ListView();
            BranchListView.HasUnevenRows = true;
            BranchListView.SeparatorVisibility = SeparatorVisibility.None;
            BranchListView.ItemTapped += viewModel.OnBranchViewItemTapped;
            BranchListView.SetBinding(ListView.ItemsSourceProperty, "BranchItems");
            BranchListView.ItemTemplate = new DataTemplate(() => new BranchInfoViewTemplate(BranchListView, contentUI, viewModel));
            BranchListView.SetBinding(ListView.HeightRequestProperty, new Binding("BranchItemsCount", BindingMode.Default, new ListViewHeightConverter(), (Device.OnPlatform(200, 190, 250))));

            var stackBranchView = new StackLayout
            {
                Padding = Device.OnPlatform(new Thickness(0), new Thickness(0), new Thickness(0, 0, -4, 0)),
                Children = { BranchListView }
            };

            discountLayout.Children.Add(stackBranchView);
            #endregion
        }

        class BranchInfoViewTemplate : ViewCellExtended
        {
            public BranchInfoViewTemplate(ListView parentListView, DiscountDetailContentUI parentContentUI, DiscountDetailViewModel parentViewModel)
            {
                HighlightSelection = false;

                var stackBranch = new StackLayout
                {
                    Padding = new Thickness(18, 0),
                    VerticalOptions = LayoutOptions.FillAndExpand
                };

                #region Location
                Grid gridLocation = new Grid
                {
                    
                    RowDefinitions = 
                    {
                        new RowDefinition { Height = GridLength.Auto }
                    },
                    ColumnDefinitions = 
                    {
                        new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) }
                    }
                };

                var txtDistanceValue = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start,
                    Style = (Style)App.Current.Resources[LabelStyles.DetailDistanceStyle]
                };
                txtDistanceValue.SetBinding(Label.TextProperty, "Distance");

                var distanceLabel = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Spacing = 0,
                    Children =
                    {
                        new Image 
                        {
                            Source = parentContentUI.ImgDistance
                        },
                        new Label 
                        {
                            Text = parentContentUI.TxtDistanceScaleValue, 
                            Style = (Style)App.Current.Resources[LabelStyles.DescriptionStyle]
                        }
                    }
                };

                var stackDistance = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.Center,
                    Spacing = 3,
                    Children =
                    {
                        new ScrollView 
                        {
                            Orientation = ScrollOrientation.Horizontal,
                            Content = txtDistanceValue
                        },
                    
                        distanceLabel
                    }
                };

                gridLocation.Children.Add(stackDistance, 0, 0);

                var txtPartnerAddress = new Label
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Style = (Style)App.Current.Resources[LabelStyles.DescriptionStyle]
                };
                txtPartnerAddress.SetBinding(Label.TextProperty, "Address");

                var txtShowOnMap = new LabelExtended
                {
                    Text = parentContentUI.TxtShowOnMap,
                    Style = (Style)App.Current.Resources[LabelStyles.LinkStyle],
                    IsUnderline = true
                };
                txtShowOnMap.Click += parentViewModel.txtShowOnMap_Click;
                txtShowOnMap.SetBinding(LabelExtended.TagProperty, "Id");

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
                gridLocation.Children.Add(locationLayout, 1, 0);

                stackBranch.Children.Add(gridLocation);

                #endregion

                #region Phone list
                var phone1 = CreateCallButton(parentContentUI, "Phone1");
                phone1.SetBinding(BorderBox.IsVisibleProperty, "IsPhone1FillIn");
                var tapGesture1 = new TapGestureRecognizer { Command = parentViewModel.CallCommand };
                tapGesture1.SetBinding(TapGestureRecognizer.CommandParameterProperty, "Phone1");
                phone1.TapGesture = tapGesture1;

                var phone2 = CreateCallButton(parentContentUI, "Phone2");
                phone2.SetBinding(BorderBox.IsVisibleProperty, "IsPhone2FillIn");
                var tapGesture2 = new TapGestureRecognizer { Command = parentViewModel.CallCommand };
                tapGesture2.SetBinding(TapGestureRecognizer.CommandParameterProperty, "Phone2");
                phone2.TapGesture = tapGesture2;

                //var phone3 = CreateCallButton(parentContentUI, "Phone3");
                //phone3.SetBinding(StackLayout.IsVisibleProperty, "IsPhone3FillIn");
                //var tapGesture3 = new TapGestureRecognizer { Command = parentViewModel.CallCommand };
                //tapGesture3.SetBinding(TapGestureRecognizer.CommandParameterProperty, "Phone3");
                //phone3.TapGesture = tapGesture3;

                //var phone4 = CreateCallButton(parentContentUI, "Phone4");
                //phone4.SetBinding(StackLayout.IsVisibleProperty, "IsPhone4FillIn");
                //var tapGesture4 = new TapGestureRecognizer { Command = parentViewModel.CallCommand };
                //tapGesture4.SetBinding(TapGestureRecognizer.CommandParameterProperty, "Phone4");
                //phone4.TapGesture = tapGesture4;

                //var phone5 = CreateCallButton(parentContentUI, "Phone5");
                //phone5.SetBinding(StackLayout.IsVisibleProperty, "IsPhone5FillIn");
                //var tapGesture5 = new TapGestureRecognizer { Command = parentViewModel.CallCommand };
                //tapGesture5.SetBinding(TapGestureRecognizer.CommandParameterProperty, "Phone5");
                //phone5.TapGesture = tapGesture5;

                var stackPhoneView = new StackLayout
                {
                    Padding = Device.OnPlatform(new Thickness(0, 4), new Thickness(0, 4), new Thickness(0, 4, -8, 4)),
                    Children = 
                    { 
                        phone1,
                        phone2,
                        //phone3,
                        //phone4,
                        //phone5
                    }
                };
                stackBranch.Children.Add(stackPhoneView);
                #endregion
                
                View = stackBranch;
            }

            private BorderBox CreateCallButton(DiscountDetailContentUI parentContentUI, string bindingProperty)
            {
                int btnHeight = Device.OnPlatform(50, 50, 64);

                var imgPhone = new Image
                {
                    Source = parentContentUI.IconPhone,
                    HeightRequest = btnHeight / 1.5,
                    WidthRequest = btnHeight / 1.5
                };

                var txtPhone = new Label
                {
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Style = (Style)App.Current.Resources[LabelStyles.DetailPhoneStyle]
                };
                txtPhone.SetBinding(Label.TextProperty, bindingProperty);

                AbsoluteLayout phoneLayout = new AbsoluteLayout
                {
                    BackgroundColor = (Color)App.Current.Resources[MainStyles.MainLightBackgroundColor]
                };

                AbsoluteLayout.SetLayoutFlags(imgPhone, AbsoluteLayoutFlags.PositionProportional);
                AbsoluteLayout.SetLayoutBounds(imgPhone,
                    new Rectangle(0.1, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize)
                );

                AbsoluteLayout.SetLayoutFlags(txtPhone, AbsoluteLayoutFlags.PositionProportional);
                AbsoluteLayout.SetLayoutBounds(
                    txtPhone,
                    new Rectangle(
                        0.7, 				        // X
                        0.5, 				        // Y
                        AbsoluteLayout.AutoSize, 	// Width
                        AbsoluteLayout.AutoSize)	// Height
                );

                phoneLayout.Children.Add(imgPhone);
                phoneLayout.Children.Add(txtPhone);

                var borderPhone = new BorderBox();
                borderPhone.HeightRequest = btnHeight - 8;
                borderPhone.BorderWidth = 1;
                borderPhone.BorderColor = (Color)App.Current.Resources[MainStyles.MainBackgroundColor];
                borderPhone.Content = phoneLayout;

                return borderPhone;
            }
        }
    }
}
