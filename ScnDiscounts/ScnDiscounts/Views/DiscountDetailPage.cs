using ScnDiscounts.Control;
using ScnDiscounts.Models.Data;
using ScnDiscounts.ValueConverter;
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

        public DiscountDetailPage(string discountID)
            : base(typeof(DiscountDetailViewModel), typeof(DiscountDetailContentUI))
        {
            viewModel.SetDiscount(discountID);
            var fileNameConverter = new FileNameToImageConverter();

            BackgroundColor = (Color)App.Current.Resources[MainStyles.MainLightBackgroundColor];

            var mainLayout = new AbsoluteLayout();

            var appBar = new TitleBar(this, TitleBar.BarBtnEnum.bbBack)
            {
                BarColor = Color.Transparent,
            };
            appBar.BoxPadding.BackgroundColor = (Color)App.Current.Resources[MainStyles.StatusBarColor];
            appBar.BoxPadding.Opacity = 0.9;

            appBar.BtnBack.BackgroundColor = (Color)App.Current.Resources[MainStyles.StatusBarColor];
            appBar.BtnBack.Opacity = 0.9;
            appBar.BtnBack.Source = contentUI.IconBack;

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
            imgPhoto.SetBinding(Image.SourceProperty, new Binding("ImageFileName", BindingMode.Default, fileNameConverter));

            //imgPhoto.SetBinding(Image.SourceProperty, new Binding("ImgPhoto", BindingMode.Default, new FileStreamToImageSource(), FileStreamToImageSource.SizeImage.siBig));

            imageLayout.Children.Add(imgPhoto,
                Constraint.Constant(0),
                Constraint.Constant(0), 
                Constraint.RelativeToParent(parent => { return parent.Width; }),
                Constraint.RelativeToParent(parent => { return parent.Height; })
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
                VerticalOptions = LayoutOptions.Start,
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
            imgCompanyLogo.SetBinding(Image.SourceProperty, new Binding("LogoFileName", BindingMode.Default, fileNameConverter));
            //imgCompanyLogo.SetBinding(Image.SourceProperty, new Binding("ImgLogo", BindingMode.Default, new FileStreamToImageSource(), FileStreamToImageSource.SizeImage.siSmall));

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
            
            var viewGesturesURL = new ViewGestures
            {
                Content = txtUrlAddress,
                DeformationValue = -5,
            };
            viewGesturesURL.BackgroundColor = this.BackgroundColor;
            viewGesturesURL.Tap += viewModel.txtUrlAddress_Click;
            titleDetailLayout.Children.Add(viewGesturesURL);
            #endregion

            gridHeader.Children.Add(titleDetailLayout, 1, 0);

            discountLayout.Children.Add(gridHeader);
            #endregion

            #region Description
            var txtDescription = new Label
            {
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

            discountLayout.VerticalOptions = LayoutOptions.Start;
            var scrollDiscount = new ScrollView
            {
                VerticalOptions = LayoutOptions.Start,
                Content = discountLayout,
                HeightRequest = Device.OnPlatform(600, 600, -1),
            };

            mainLayout.VerticalOptions = LayoutOptions.StartAndExpand ;
            AbsoluteLayout.SetLayoutFlags(scrollDiscount, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(scrollDiscount, new Rectangle(0f, 0f, 1f, 1f));
            mainLayout.Children.Add(scrollDiscount);

            AbsoluteLayout.SetLayoutFlags(appBar, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(appBar,
                new Rectangle(0, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            mainLayout.Children.Add(appBar);

            ContentLayout.Children.Add(mainLayout);

            if (Device.OS == TargetPlatform.iOS)
                InitBranchListView();
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
            BranchListView.SetBinding(ListView.HeightRequestProperty, new Binding("BranchItemsCount", BindingMode.Default, new ListViewHeightConverter(), (Device.OnPlatform(160, 190, 220))));
            BranchListView.BackgroundColor = (Color)App.Current.Resources[MainStyles.MainLightBackgroundColor];

            var stackBranchView = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
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
                IsHighlightSelection = false;

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
                        new ColumnDefinition { Width = GridLength.Auto },
                        new ColumnDefinition { Width = GridLength.Auto },
                        new ColumnDefinition  { Width = new GridLength(1, GridUnitType.Star) }
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

                gridLocation.Children.Add(txtDistanceValue, 0, 0);
                gridLocation.Children.Add(distanceLabel, 1, 0);

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
                
                var viewGesturesShowOnMap = new ViewGestures
                {
                    Content = locationLayout,
                    DeformationValue = -5,
                };
                viewGesturesShowOnMap.BackgroundColor = (Color)App.Current.Resources[MainStyles.MainLightBackgroundColor];
                viewGesturesShowOnMap.Tap += parentViewModel.ShowOnMap_Click;
                viewGesturesShowOnMap.SetBinding(ViewGestures.TagProperty, "DocumentId");

                gridLocation.Children.Add(viewGesturesShowOnMap, 2, 0);

                stackBranch.Children.Add(gridLocation);

                #endregion

                #region Phone list
                var stackPhoneView = new StackLayout
                {
                    Padding = Device.OnPlatform(new Thickness(0, 4), new Thickness(0, 4), new Thickness(0, 4, -8, 4)),
                };

                #region phone1
                var phone1 = CreateCallButton(parentContentUI, "Phone1");
                phone1.SetBinding(BorderBox.TagProperty, "Phone1");

                var viewGesturesPhone1 = new ViewGestures
                {
                    Content = phone1,
                    DeformationValue = -5,
                };
                viewGesturesPhone1.SetBinding(ViewGestures.IsVisibleProperty, "IsPhone1FillIn");
                viewGesturesPhone1.BackgroundColor = (Color)App.Current.Resources[MainStyles.MainLightBackgroundColor];
                viewGesturesPhone1.Tap += parentViewModel.BtnCall_Click;
                stackPhoneView.Children.Add(viewGesturesPhone1);
                #endregion

                #region phone2
                var phone2 = CreateCallButton(parentContentUI, "Phone2");
                phone2.SetBinding(BorderBox.TagProperty, "Phone2");

                var viewGesturesPhone2 = new ViewGestures
                {
                    Content = phone2,
                    DeformationValue = -5,
                };
                viewGesturesPhone2.SetBinding(ViewGestures.IsVisibleProperty, "IsPhone2FillIn");
                viewGesturesPhone2.BackgroundColor = (Color)App.Current.Resources[MainStyles.MainLightBackgroundColor];
                viewGesturesPhone2.Tap += parentViewModel.BtnCall_Click;
                stackPhoneView.Children.Add(viewGesturesPhone2);
                #endregion

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
