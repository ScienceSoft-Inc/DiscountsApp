using ScnDiscounts.Control;
using ScnDiscounts.Models;
using ScnDiscounts.ViewModels;
using ScnDiscounts.Views.ContentUI;
using ScnDiscounts.Views.Styles;
using ScnSideMenu.Forms;
using ScnTitleBar.Forms;
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
            : base(typeof(MainViewModel), typeof(MainContentUI))
        {
            BackgroundColor = (Color)App.Current.Resources[MainStyles.ListBackgroundColor];

            var appBar = new TitleBar(this, TitleBar.BarBtnEnum.bbLeftRight, TitleBar.BarAlignEnum.baBottom)
            {
                BarColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.FillAndExpand  
            };
            appBar.BtnRight.BackgroundColor = new Color(255, 255, 255, 0.7);
            appBar.BtnRight.Source = contentUI.IconMenuSideBar;
            appBar.BtnRight.Click += viewModel.AppBar_BtnRightClick;

            appBar.BtnLeft.BackgroundColor = new Color(255, 255, 255, 0.7);
            appBar.BtnLeft.Source = contentUI.IconLocation;
            appBar.BtnLeft.Click += viewModel.AppBar_BtnLeftClick;

            RightPanel.BackgroundColor = (Color)App.Current.Resources[MainStyles.MainBackgroundColor];
            RightPanel.Opacity = 0.9;
            SpeedAnimatePanel = 200;

            MapLocation = new MapTile
            {
                HasScrollEnabled = true,
                HasZoomEnabled = true,
                Context = contentUI,
            };

            MapLocation.ClickPinDetail += viewModel.MapLocation_ClickPinDetail;
            
            foreach (var item in AppData.Discount.MapPinCollection)
                MapLocation.PinList.Add(item);

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
            
            ContentLayout.Children.Add(mainLayout);

            PanelChanged += (s, e) => 
            {
                if (e.IsShow)
                    MapLocation.CloseDetailInfo();
            };

            #region SideBar menu
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
            menuView.RowHeight = Device.OnPlatform(40, 40, 60);
            menuView.SeparatorVisibility = SeparatorVisibility.None;
            menuView.ItemTemplate = new DataTemplate(() => new MenuViewTemplate());
            menuView.ItemSelected += viewModel.OnMenuViewItemTapped;
            menuView.IsScrollable = false;
            RightPanel.AddToContext(menuView, false);

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
