using ScnDiscounts.Control;
using ScnDiscounts.Models;
using ScnDiscounts.ViewModels;
using ScnDiscounts.Views.ContentUI;
using ScnDiscounts.Views.Styles;
using ScnSideMenu.Forms;
using ScnTitleBar.Forms;
using ScnViewGestures.Plugin.Forms;
using Xamarin.Forms;

namespace ScnDiscounts.Views
{
    class MainPage : SideBarPage
    {
        private MainViewModel viewModel
        {
            get { return (MainViewModel)BindingContext; }
        }

        private MainContentUI contentUI
        {
            get { return (MainContentUI)ContentUI; }
        }

        public MapTile MapLocation;

        public MainPage()
            : base(typeof(MainViewModel), typeof(MainContentUI), PanelSetEnum.psLeftRight)
        {
            BackgroundColor = (Color)App.Current.Resources[MainStyles.ListBackgroundColor];

            var appBar = new TitleBar(this, TitleBar.BarBtnEnum.bbLeftRight, TitleBar.BarAlignEnum.baBottom)
            {
                BarColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.FillAndExpand  
            };
            appBar.BtnRight.BackgroundColor = new Color(255, 255, 255, 0);
            appBar.BtnRight.Source = contentUI.IconMenuSideBar;
            appBar.BtnRight.Click += viewModel.AppBar_BtnRightClick;

            appBar.BtnLeft.BackgroundColor = new Color(255, 255, 255, 0);
            appBar.BtnLeft.Source = contentUI.IconFilter;
            appBar.BtnLeft.Click += viewModel.AppBar_BtnLeftClick;

            RightPanel.BackgroundColor = (Color)App.Current.Resources[MainStyles.MainBackgroundColor];
            RightPanel.Opacity = 0.9;
            SpeedAnimatePanel = 200;

            LeftPanel.BackgroundColor = (Color)App.Current.Resources[MainStyles.MainBackgroundColor];
            LeftPanel.Opacity = 0.9;

            MapLocation = new MapTile
            {
                HasScrollEnabled = true,
                HasZoomEnabled = true,
                Context = contentUI,
            };

            MapLocation.ClickPinDetail += viewModel.MapLocation_ClickPinDetail;

            var btnLocation = new ImageButton();
            btnLocation.HeightRequest = appBar.HeightBar;
            btnLocation.WidthRequest = appBar.HeightBar;
            btnLocation.BackgroundColor = new Color(255, 255, 255, 0);
            btnLocation.Source = contentUI.IconLocation;
            btnLocation.Click += viewModel.BtnLocation_Click;

            var mainLayout = new RelativeLayout();
            mainLayout.Children.Add(MapLocation.MapLayout,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent(parent => { return parent.Width; }),
                Constraint.RelativeToParent(parent => { return parent.Height; }));
            
            mainLayout.Children.Add(appBar,
                Constraint.Constant(0),
                Constraint.RelativeToParent(parent => { return parent.Height - appBar.HeightBar; }),
                Constraint.RelativeToParent(parent => { return parent.Width; }),
                Constraint.Constant(appBar.HeightBar));

            mainLayout.Children.Add(btnLocation,
                Constraint.Constant(0),
                Constraint.RelativeToView(appBar, (parent, sibling) =>
                {
                    return sibling.Y - appBar.HeightBar - 10;
                }));       
            ContentLayout.Children.Add(mainLayout);

            PanelChanged += (s, e) => 
            {
                if (e.IsShow)
                    MapLocation.CloseDetailInfo();
            };

            #region Right SideBar menu
            RightPanel.ClearContext();
            RightPanel.AddToContext(new BoxView
                {
                    BackgroundColor = Color.Transparent,
                    HeightRequest = 50,
                });

            var imgLogo = new Image
            {
                Source = ImageSource.FromFile(contentUI.ImgLogo),
                HeightRequest = Device.OnPlatform(-1, -1, 120),
            };
            RightPanel.AddToContext(imgLogo);

            RightPanel.AddToContext(new BoxView
                {
                    BackgroundColor = Color.Transparent,
                    HeightRequest = 50,
                });

            var menuView = new ListViewExtended();
            menuView.SetBinding(ListView.ItemsSourceProperty, "MenuItemList");
            menuView.RowHeight = Device.OnPlatform(48, 48, 72);
            menuView.SeparatorVisibility = SeparatorVisibility.None;
            menuView.ItemTemplate = new DataTemplate(() => new MenuViewTemplate());
            menuView.ItemSelected += viewModel.OnMenuViewItemTapped;
            menuView.IsScrollable = false;
            RightPanel.AddToContext(menuView, false);
            #endregion

            #region Left SideBar menu
            LeftPanel.ClearContext();
            
            LeftPanel.AddToContext(new BoxView
            {
                BackgroundColor = Color.Transparent,
                HeightRequest = 50,
            });
            
            var filterView = new ListViewExtended();
            filterView.IsScrollable = false;
            filterView.SetBinding(ListView.ItemsSourceProperty, "FilterCategoryList");
            filterView.RowHeight = Device.OnPlatform(48, 48, 72);
            filterView.SeparatorVisibility = SeparatorVisibility.None;
            filterView.ItemTemplate = new DataTemplate(() => new FilterViewTemplate(viewModel));
            filterView.ItemSelected += viewModel.OnMenuViewItemTapped;
            LeftPanel.AddToContext(filterView, false);
            #endregion
        }

        class MenuViewTemplate : ViewCellExtended
        {
            public MenuViewTemplate()
            {
                SelectColor = (Color)App.Current.Resources[MainStyles.ListSelectColor];

                Grid gridMenuItem = new Grid
                {
                    BackgroundColor = (Color)App.Current.Resources[MainStyles.MainBackgroundColor],
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Padding = new Thickness(40, 0),
                    RowDefinitions = 
                    {
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                    },
                    ColumnDefinitions = 
                    {
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)},
                        new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star)}
                    },
                };

                var imgMenuItem = new Image();
                imgMenuItem.SetBinding(Image.SourceProperty, "Icon");
                gridMenuItem.Children.Add(imgMenuItem, 0, 0);

                var txtMenuItem = new Label
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Style = (Style)App.Current.Resources[LabelStyles.MenuStyle]
                };
                txtMenuItem.SetBinding(Label.TextProperty, "Name");
                gridMenuItem.Children.Add(txtMenuItem, 1, 0);

                View = gridMenuItem;
            }
        }

        public class FilterViewTemplate : ViewCellExtended
        {
            public FilterViewTemplate(MainViewModel parentViewModel)
            {
                IsHighlightSelection = false;
                SelectColor = Color.Transparent;


                Grid gridFilterItem = new Grid
                {
                    BackgroundColor = (Color)App.Current.Resources[MainStyles.MainBackgroundColor],
                    Padding = new Thickness(16, 0),
                    RowDefinitions = 
                    {
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                    },
                    ColumnDefinitions = 
                    {
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)},
                        new ColumnDefinition { Width = GridLength.Auto}
                    },
                };

                #region Title
                var stackTitle = new StackLayout
                {
                    Spacing = 16,
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Start,
                };
                var imgFilterItem = new Image();
                imgFilterItem.SetBinding(Image.SourceProperty, "Icon");
                stackTitle.Children.Add(imgFilterItem);

                var txtFilterItem = new Label
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };
                txtFilterItem.SetBinding(Label.StyleProperty, "NameStyle");
                txtFilterItem.SetBinding(Label.TextProperty, "Name");
                stackTitle.Children.Add(txtFilterItem);

                var filterTitleGestures = new ViewGestures();
                filterTitleGestures.Content = stackTitle;
                filterTitleGestures.Tap += parentViewModel.FilterGestures_Tap;
                filterTitleGestures.SwipeLeft += parentViewModel.FilterGestures_Tap;
                filterTitleGestures.BackgroundColor = (Color)App.Current.Resources[MainStyles.MainBackgroundColor];

                gridFilterItem.Children.Add(filterTitleGestures, 0, 0);
                #endregion

                #region Toggle
                var switchFilter = new SwitchExtended
                {
                    HorizontalOptions = LayoutOptions.End,
                };
                switchFilter.SetBinding(SwitchExtended.TextOffProperty, "TurnOffValue");
                switchFilter.SetBinding(SwitchExtended.TextOnProperty, "TurnOnValue");

                if (Device.OS == TargetPlatform.iOS)
                    switchFilter.TranslationY = 8;
                
                switchFilter.SetBinding(Switch.IsToggledProperty, "IsToggle", BindingMode.TwoWay);
                switchFilter.Toggled += parentViewModel.SwitchFilter_Toggled;

                gridFilterItem.Children.Add(switchFilter, 1, 0);
                #endregion

                View = gridFilterItem;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (MapLocation.IsShowDetailInfo)
            {
                MapLocation.IsShowDetailInfo = false;
                return true ;
            }

            return base.OnBackButtonPressed();
        }
    }
}