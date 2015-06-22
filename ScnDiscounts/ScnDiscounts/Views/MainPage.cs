using ScnDiscounts.Control;
using ScnDiscounts.Control.SideBar;
using ScnDiscounts.Models;
using ScnDiscounts.ViewModels;
using ScnDiscounts.Views.ContentUI;
using ScnDiscounts.Views.Styles;
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

            var appBar = new CustomAppBar(this, CustomAppBar.CustomBarBtnEnum.cbRightLeft)
            {
                BarColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.FillAndExpand  
            };
            appBar.BtnRight.BackgroundColor = new Color(255, 255, 255, 0.7);
            appBar.BtnRight.Source = contentUI.IconMenuSideBar;
            appBar.BtnRight.Click += viewModel.AppBar_BtnRightClick;
            appBar.BtnRight.WidthRequest = appBar.HeightRequest;
            appBar.BtnRight.HeightRequest = appBar.HeightRequest;

            appBar.BtnLeft.BackgroundColor = new Color(255, 255, 255, 0.7);
            appBar.BtnLeft.Source = contentUI.IconLocation;
            appBar.BtnLeft.Click += viewModel.AppBar_BtnLeftClick;
            appBar.BtnLeft.WidthRequest = appBar.HeightRequest;
            appBar.BtnLeft.HeightRequest = appBar.HeightRequest;

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
                Constraint.RelativeToParent(parent => { return parent.Height - appBar.HeightRequest; }),
                Constraint.RelativeToParent(parent => { return parent.Width; }),
                Constraint.Constant(appBar.HeightRequest));
            
            ContentLayout.Children.Add(mainLayout);

            PanelChanged += (s, e) => 
            {
                if (e.IsShow)
                    MapLocation.CloseDetailInfo();
            };

            //SideBar menu
            var imgLogo = new Image
            {
                Source = ImageSource.FromFile(contentUI.ImgLogo),
                HeightRequest = Device.OnPlatform(-1, -1, 120),
            };

            var menuView = new ListView();
            menuView.SetBinding(ListView.ItemsSourceProperty, "MenuItemList");
            menuView.RowHeight = Device.OnPlatform(50, 40, 60);
            menuView.SeparatorVisibility = SeparatorVisibility.None;
            menuView.ItemTemplate = new DataTemplate(() => new MenuViewTemplate());
            menuView.ItemSelected += viewModel.OnMenuViewItemTapped;

            var rightPanelLayout = new RelativeLayout();
            rightPanelLayout.Children.Add(imgLogo,
                Constraint.Constant(0),
                Constraint.Constant(50),
                Constraint.RelativeToParent(parent => { return parent.Width; }));

            rightPanelLayout.Children.Add(menuView,
                Constraint.Constant(0),
                Constraint.RelativeToView(imgLogo, (parent, sibling) =>
                {
                    return sibling.Y + sibling.Height + 50;
                }));

            RightPanel.Context = rightPanelLayout;
        }

        class MenuViewTemplate : ViewCell
        {
            public MenuViewTemplate()
            {
                Grid gridMenuItem = new Grid
                {
                    BackgroundColor = Color.Transparent,
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
