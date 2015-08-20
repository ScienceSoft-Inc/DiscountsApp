using ScnDiscounts.Control;
using ScnDiscounts.ValueConverter;
using ScnDiscounts.ValueConverters;
using ScnDiscounts.ViewModels;
using ScnDiscounts.Views.ContentUI;
using ScnDiscounts.Views.Styles;
using ScnPage.Plugin.Forms;
using ScnTitleBar.Forms;
using System.Threading.Tasks;
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

        public ListViewAnimation DiscountListView;
        private StackLayout discountLayout;

        public DiscountPage()
            : base(typeof(DiscountViewModel), typeof(DiscountContentUI))
        {
            BackgroundColor = (Color)App.Current.Resources[MainStyles.MainLightBackgroundColor];

            var appBar = new TitleBar(this, TitleBar.BarBtnEnum.bbBack)
            {
                BarColor = (Color)App.Current.Resources[MainStyles.StatusBarColor]
            };
            appBar.BtnBack.BackgroundColor = Color.Transparent;
            appBar.BtnBack.Source = contentUI.IconBack;

            ContentLayout.Children.Add(appBar);

            DiscountListView = new ListViewAnimation();
            DiscountListView.HasUnevenRows = true;
            DiscountListView.SetBinding(ListView.HeightRequestProperty, new Binding("DiscountItemsCount", BindingMode.Default, new ListViewHeightConverter(), (Device.OnPlatform(109, 109, 136))));
            DiscountListView.SeparatorVisibility = SeparatorVisibility.None;
            DiscountListView.SetBinding(ListView.ItemsSourceProperty, "DiscountItems");
            DiscountListView.ItemTemplate = new DataTemplate(() => new DiscountTemplate(DiscountListView,  Device.OnPlatform(108, 108, 136)));
            DiscountListView.ItemTapped += viewModel.OnDiscountItemTapped;
            DiscountListView.IsEnabled = false;
            DiscountListView.BackgroundColor = (Color)App.Current.Resources[MainStyles.MainLightBackgroundColor];
            DiscountListView.AnimationFinished += async (s, e) => 
            {
                await Task.Delay(100);
                DiscountListView.IsEnabled = true; 
            };

            discountLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = Device.OnPlatform(new Thickness(0, 4), new Thickness(0, 4), new Thickness(0, 4, -12, 0)),
            };
            discountLayout.Children.Add(DiscountListView);

            var mainLayout = new AbsoluteLayout();
            if (Device.OS == TargetPlatform.WinPhone)
            {
                AbsoluteLayout.SetLayoutFlags(discountLayout, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(discountLayout, new Rectangle(0f, 0f, 1f, 1f));
                mainLayout.Children.Add(discountLayout);
            }
            else
            {
                var scrollDiscount = new ScrollView
                {
                    Content = discountLayout,
                };

                AbsoluteLayout.SetLayoutFlags(scrollDiscount, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(scrollDiscount, new Rectangle(0f, 0f, 1f, 1f));
                mainLayout.Children.Add(scrollDiscount);
            }
            ContentLayout.Children.Add(mainLayout);
        }

        class DiscountTemplate : ViewCellExtended
        {
            public DiscountTemplate(ListViewAnimation parentListView, int rowHeight)
            {
                SelectColor = (Color)App.Current.Resources[MainStyles.ListSelectColor];

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

                var fileNameConverter = new FileNameToImageConverter();

                // Company logo
                var imgCompanyLogo = new ImageExtended
                {
                    WidthRequest = Device.OnPlatform(64, 64, 64),
                    HeightRequest = Device.OnPlatform(64, 64, 64),
                    Aspect = Aspect.AspectFit,
                    
                };
                imgCompanyLogo.SetBinding(Image.SourceProperty, new Binding("LogoFileName", BindingMode.Default, fileNameConverter));


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
                        new RowDefinition { Height = GridLength.Auto }
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
                    Style = (Style)App.Current.Resources[LabelStyles.ListPercentStyle],
                    VerticalOptions = LayoutOptions.End
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
                    Padding = Device.OnPlatform(new Thickness(4), new Thickness(4), new Thickness(6)),
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
                    Padding = Device.OnPlatform(new Thickness(4), new Thickness(4), new Thickness(6)),
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
                };
                txtDescription.GestureRecognizers.Clear();
                txtDescription.SetBinding(LabelExtended.TextProperty, new Binding("Description", BindingMode.Default, new TextHeightLimitation()));

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
                    Padding = new Thickness(8, 4),
                    HeightRequest = rowHeight - 16,
                };
                boxBorder.BorderColor = (Color)App.Current.Resources[MainStyles.ListBorderColor];
                boxBorder.BorderWidth = 1;
                boxBorder.Content = gridDiscountItem;
                
                var layout = new AbsoluteLayout
                {
                    Padding = new Thickness (8, 4),
                };
                parentListView.AnimationListAdd(layout);
                
                AbsoluteLayout.SetLayoutFlags(boxBorder, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(boxBorder, new Rectangle(0f, 0f, 1f, 1f));
                layout.Children.Add(boxBorder);

                View = layout;
            }
        }
    }
}
