using ScnDiscounts.Control;
using ScnDiscounts.Control.Pages;
using ScnDiscounts.ValueConverter;
using ScnDiscounts.ViewModels;
using ScnDiscounts.Views.ContentUI;
using ScnDiscounts.Views.Styles;
using Xamarin.Forms;

namespace ScnDiscounts.Views
{
    class DiscountPage : BaseContentPage 
    {
        private DiscountViewModel viewModel
        {
            get { return (DiscountViewModel)BindingContext; }
        }

        private DiscountContentUI contentUI
        {
            get { return (DiscountContentUI)ContentUI; }
        }

        public DiscountPage()
            : base(typeof(DiscountViewModel), typeof(DiscountContentUI))
        {
            BackgroundColor = (Color)App.Current.Resources[MainStyles.MainLightBackgroundColor];

            var appBar = new CustomAppBar(this, CustomAppBar.CustomBarBtnEnum.cbBack)
            {
                BarColor = (Color)App.Current.Resources[MainStyles.MainBackgroundColor]
            };
            appBar.BtnBack.BackgroundColor = Color.Transparent;
            appBar.BtnBack.Source = contentUI.IconBack;
            appBar.BtnBack.WidthRequest = appBar.HeightRequest;
            appBar.BtnBack.HeightRequest = appBar.HeightRequest;

            ContentLayout.Children.Add(appBar);

            var discountListView = new ListView();
            discountListView.RowHeight = Device.OnPlatform(100, 108, 110);
            discountListView.SetBinding(ListView.ItemsSourceProperty, "DiscountItems");
            discountListView.ItemTemplate = new DataTemplate(() => new DiscountTemplate(contentUI)); ;
            discountListView.ItemTapped += viewModel.OnDiscountItemTapped;

            var stackDiscount = new StackLayout
            {
                Padding = new Thickness(0, 4),
                Children = { discountListView }
            };

            ContentLayout.Children.Add(stackDiscount);
        }

        class DiscountTemplate : ViewCell
        {
            public DiscountTemplate(DiscountContentUI parentcontentUI)
            {
                Grid gridDiscountItem = new Grid
                {
                    Padding = new Thickness (10),
                    BackgroundColor = (Color)App.Current.Resources[MainStyles.ListBackgroundColor],
                    RowDefinitions = 
                    {
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)  },
                        new RowDefinition { Height = new GridLength(2, GridUnitType.Star)  }
                    },
                    ColumnDefinitions = 
                    {
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)},
                        new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star)}
                    }
                };

                // Company logo
                var imgCompanyLogo = new Image
                {
                    WidthRequest = Device.OnPlatform(64, 64, 64),
                    HeightRequest = Device.OnPlatform(64, 64, 64),
                    Aspect = Aspect.AspectFit
                };
                imgCompanyLogo.SetBinding(Image.SourceProperty, new Binding("Icon", BindingMode.Default, new FileStreamToImageSource(), FileStreamToImageSource.SizeImage.siBig));

                var stackCompanyLogo = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = { imgCompanyLogo }
                };
                gridDiscountItem.Children.Add(stackCompanyLogo, 0, 1, 0, 2);

                // Header
                Grid headerLayout = new Grid
                {
                    Padding = new Thickness(2, 0, 0, 0),
                    RowDefinitions = 
                    {
                        new RowDefinition { Height =  GridLength.Auto }
                    },
                    ColumnDefinitions = 
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star)},
                        new ColumnDefinition { Width = GridLength.Auto }//new GridLength(1, GridUnitType.Star)},
                    }
                };
                
                // Title
                var txtTitle = new Label
                {
                    VerticalOptions = LayoutOptions.End,
                    Style = (Style)App.Current.Resources[LabelStyles.ListTitleStyle]
                };
                txtTitle.SetBinding(Label.TextProperty, "Name");

                headerLayout.Children.Add(txtTitle, 0, 0);

                #region Percent label
                var txtPercent = new Label
                {
                    Style = (Style)App.Current.Resources[LabelStyles.ListPercentStyle]
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
                            Style = (Style)App.Current.Resources[LabelStyles.ListPercentSymbolStyle],
                            VerticalOptions = LayoutOptions.End
                        }
                    }
                };
                #endregion

                #region Category layout
                var txtCategory = new Label
                {
                    Style = (Style)App.Current.Resources[LabelStyles.CategoryStyle],
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                txtCategory.SetBinding(Label.TextProperty, "FirstCategoryName");
                txtCategory.SetBinding(BackgroundColorProperty, "FirstCategoryColor");

                var categoryLayout = new StackLayout
                {
                    Padding = new Thickness (4),
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Children =
                    {
                        txtCategory
                    }
                };
                categoryLayout.SetBinding(BackgroundColorProperty, "FirstCategoryColor");

                var txtCategoryMore = new Label
                {
                    Text = "...",
                    Style = (Style)App.Current.Resources[LabelStyles.CategoryStyle],
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                txtCategory.SetBinding(BackgroundColorProperty, "FirstCategoryColor");

                var stackCategoryMore = new StackLayout
                {
                    Padding = new Thickness(4),
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Children =
                    {
                        txtCategoryMore
                    }
                };
                stackCategoryMore.SetBinding(BackgroundColorProperty, "FirstCategoryColor");
                stackCategoryMore.SetBinding(IsVisibleProperty, "IsCategoryMore");
                #endregion

                var infoDiscountLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    
                    Children =
                    {
                        percentLayout,
                        categoryLayout,
                        stackCategoryMore
                    }
                };

                headerLayout.Children.Add(infoDiscountLayout, 1, 0);
                gridDiscountItem.Children.Add(headerLayout, 1, 2, 0, 1);

                #region Description
                var txtDescription = new LabelExtended 
                { 
                    IsWrapped = true,
                    Style = (Style)App.Current.Resources[LabelStyles.DescriptionStyle],
                    LineBreakMode = LineBreakMode.WordWrap,
                    InputTransparent = true
                };
                txtDescription.SetBinding(LabelExtended.TextProperty, "Description");

                var layoutDescription = new RelativeLayout();
                layoutDescription.Children.Add(txtDescription,
                    Constraint.Constant(2),
                    Constraint.Constant(0),
                    Constraint.RelativeToParent(parent => { return parent.Width - 2; }),
                    Constraint.RelativeToParent(parent => { return parent.Height; }));

                gridDiscountItem.Children.Add(layoutDescription, 1, 2, 1, 2);
                #endregion

                var boxBorder = new BorderBox
                {
                    Padding = new Thickness(8, 4)
                };
                boxBorder.BorderColor = (Color)App.Current.Resources[MainStyles.ListBorderColor];
                boxBorder.BorderWidth = 1;
                boxBorder.Content = gridDiscountItem;

                var layout = new AbsoluteLayout
                {
                    Padding = new Thickness (8, 4)
                };

                AbsoluteLayout.SetLayoutFlags(boxBorder, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(boxBorder, new Rectangle(0f, 0f, 1f, 1f));
                layout.Children.Add(boxBorder);

                View = layout;
            }
        }
    }
}
