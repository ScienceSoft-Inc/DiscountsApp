using ScnDiscounts.Control;
using ScnDiscounts.Helpers;
using ScnDiscounts.ViewModels;
using ScnDiscounts.Views.ContentUI;
using ScnDiscounts.Views.Styles;
using ScnPage.Plugin.Forms;
using ScnSideMenu.Forms;
using ScnTitleBar.Forms;
using Xamarin.Forms;
using ImageButton = ScnTitleBar.Forms.ImageButton;

namespace ScnDiscounts.Views
{
    public class MainPage : SideBarPage
    {
        private MainViewModel viewModel => (MainViewModel) ViewModel;

        private MainContentUI contentUI => (MainContentUI) ContentUI;

        public MapTile MapLocation;

        private readonly TitleBar appBar;
        private readonly RelativeLayout mainLayout;
        private ImageButton btnLocation;

        public MainPage()
            : base(typeof(MainViewModel), typeof(MainContentUI),
                Device.Idiom == TargetIdiom.Phone ? PanelSetEnum.psLeftRight : PanelSetEnum.psRight)
        {
            BackgroundColor = MainStyles.ListBackgroundColor.FromResources<Color>();

            appBar = new TitleBar(this, TitleBar.BarBtnEnum.bbLeftRight, TitleBar.BarAlignEnum.baBottom)
            {
                BarColor = Color.Transparent,
                BtnRight =
                {
                    Source = contentUI.IconMenuSideBar
                }
            };

            appBar.BtnRight.Click += viewModel.AppBar_BtnRightClick;

            MapLocation = new MapTile
            {
                HasScrollEnabled = true,
                HasZoomEnabled = true,
                Context = contentUI
            };

            MapLocation.ClickPinDetail += viewModel.MapLocation_ClickPinDetail;

            mainLayout = new RelativeLayout();
            mainLayout.Children.Add(MapLocation.MapLayout,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent(parent => parent.Width),
                Constraint.RelativeToParent(parent => parent.Height));

            mainLayout.Children.Add(appBar,
                Constraint.Constant(0),
                Constraint.RelativeToParent(
                    parent => parent.Height - appBar.HeightBar - appBar.Margin.VerticalThickness),
                Constraint.RelativeToParent(parent => parent.Width));

            PanelChanged += (sender, args) =>
            {
                if (args.IsShow)
                    MapLocation.CloseDetailInfo();
            };

            InitPanelsAndLayout();

            var safeAreaHelper = new SafeAreaHelper();
            safeAreaHelper.UseSafeArea(this, SafeAreaHelper.CustomSafeAreaFlags.None);
            safeAreaHelper.UseSafeArea(appBar,
                SafeAreaHelper.CustomSafeAreaFlags.Horizontal | SafeAreaHelper.CustomSafeAreaFlags.Bottom);
            safeAreaHelper.UseSafeArea(RightPanel.Content,
                SafeAreaHelper.CustomSafeAreaFlags.Top | SafeAreaHelper.CustomSafeAreaFlags.Right);
            if (LeftPanel != null)
                safeAreaHelper.UseSafeArea(LeftPanel.Content,
                    SafeAreaHelper.CustomSafeAreaFlags.Top | SafeAreaHelper.CustomSafeAreaFlags.Left);
            if (btnLocation != null)
                safeAreaHelper.UseSafeArea(btnLocation, SafeAreaHelper.CustomSafeAreaFlags.Left);
            if (Device.RuntimePlatform == Device.Android)
                safeAreaHelper.UseSafeArea(MapLocation.MapPinDetail, SafeAreaHelper.CustomSafeAreaFlags.All);

            ContentLayout.Children.Add(mainLayout);
        }

        #region Init Panels and Layout

        private void InitPanelsAndLayout()
        {
            SpeedAnimatePanel = 200;

            RightPanel.BackgroundColor = MainStyles.MainBackgroundColor.FromResources<Color>();
            RightPanel.Opacity = 0.9;

            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "FilterCategoryList")
                {
                    if (Device.Idiom == TargetIdiom.Phone)
                        InitPhoneLeftPanel();
                    else
                        InitTabletRightPanel();
                }
            };

            if (Device.Idiom == TargetIdiom.Phone)
                InitPhoneLayout();
            else
                InitTabletLayout();
        }

        private void InitPhoneLeftPanel()
        {
            var leftPanelContent = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 0
            };

            leftPanelContent.Children.Add(new BoxView
            {
                BackgroundColor = Color.Transparent,
                HeightRequest = 50
            });

            AddFilterPanelTo(leftPanelContent, viewModel);

            LeftPanel.Content = leftPanelContent;
        }

        private void InitPhoneRightPanel()
        {
            var rightPanelContent = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 0
            };

            rightPanelContent.Children.Add(new BoxView
            {
                BackgroundColor = Color.Transparent,
                HeightRequest = 50
            });

            var imgLogo = new Image
            {
                Source = ImageSource.FromFile(contentUI.ImgLogo)
            };
            rightPanelContent.Children.Add(imgLogo);

            rightPanelContent.Children.Add(new BoxView
            {
                BackgroundColor = Color.Transparent,
                HeightRequest = 50
            });

            foreach (var menuItem in viewModel.MenuItemList)
            {
                var menuView = new MenuViewTemplate(viewModel).View;
                menuView.BindingContext = menuItem;

                rightPanelContent.Children.Add(menuView);
            }

            rightPanelContent.Children.Add(new BoxView
            {
                BackgroundColor = Color.Transparent,
                HeightRequest = 30
            });

            RightPanel.Content = rightPanelContent;
        }

        private void InitTabletRightPanel()
        {
            var rightPanelContent = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 0
            };

            rightPanelContent.Children.Add(new BoxView
            {
                BackgroundColor = Color.Transparent,
                HeightRequest = 50
            });

            var imgLogo = new Image
            {
                Source = ImageSource.FromFile(contentUI.ImgLogo)
            };
            rightPanelContent.Children.Add(imgLogo);

            rightPanelContent.Children.Add(new BoxView
            {
                BackgroundColor = Color.Transparent,
                HeightRequest = 50
            });

            foreach (var menuItem in viewModel.MenuItemList)
            {
                var menuView = new MenuViewTemplate(viewModel).View;
                menuView.BindingContext = menuItem;

                rightPanelContent.Children.Add(menuView);
            }

            rightPanelContent.Children.Add(new BoxView
            {
                BackgroundColor = Color.Transparent,
                HeightRequest = 30
            });

            AddFilterPanelTo(rightPanelContent, viewModel);

            RightPanel.Content = rightPanelContent;
        }

        private void InitPhoneLayout()
        {
            LeftPanelWidthRequest = 400;
            RightPanelWidthRequest = 400;

            LeftSwipeSize = 30;
            RightSwipeSize = 30;
            TransparentSize = new Thickness(50, 0, 50, 110);

            LeftPanel.BackgroundColor = MainStyles.MainBackgroundColor.FromResources<Color>();
            LeftPanel.Opacity = 0.9;

            btnLocation = new ImageButton
            {
                HeightRequest = appBar.HeightBar,
                WidthRequest = appBar.HeightBar,
                BackgroundColor = new Color(255, 255, 255, 0),
                Source = contentUI.IconLocation
            };
            btnLocation.Click += viewModel.BtnLocation_Click;

            mainLayout.Children.Add(btnLocation,
                Constraint.Constant(0),
                Constraint.RelativeToView(appBar, (parent, sibling) => sibling.Y - appBar.HeightBar - 10));

            appBar.BtnLeft.BackgroundColor = new Color(255, 255, 255, 0);
            appBar.BtnLeft.Source = contentUI.IconFilter;
            appBar.BtnLeft.Click += viewModel.AppBar_BtnLeftPhoneClick;

            InitPhoneLeftPanel();
            InitPhoneRightPanel();
        }

        private void InitTabletLayout()
        {
            RightPanelWidthRequest = 400;

            RightSwipeSize = 30;
            TransparentSize = new Thickness(50, 0, 0, 50);

            appBar.BtnLeft.BackgroundColor = new Color(255, 255, 255, 0);
            appBar.BtnLeft.Source = contentUI.IconLocation;
            appBar.BtnLeft.Click += viewModel.AppBar_BtnLeftTabletClick;

            InitTabletRightPanel();
        }

        #endregion

        protected override bool OnBackButtonPressed()
        {
            if (MapLocation.IsShowDetailInfo)
            {
                MapLocation.CloseDetailInfo();
                return true;
            }

            return base.OnBackButtonPressed();
        }

        public void AddFilterPanelTo(StackLayout panelContent, BaseViewModel localViewModel)
        {
            #region Filter All

            var filterGroup = new Label
            {
                BindingContext = viewModel,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Style = LabelStyles.MenuStyle.FromResources<Style>()
            };

            filterGroup.SetBinding(Label.TextProperty, "CategoriesTitle");

            var switchFilter = new Switch
            {
                BindingContext = viewModel,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                OnColor = Color.White
            };

            switchFilter.SetBinding(Switch.IsToggledProperty, "IsAllFiltersSelected");

            var filterGroupContainer = new StackLayout
            {
                BackgroundColor = MainStyles.StatusBarColor.FromResources<Color>(),
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(App.IsMoreThan320Dpi ? 40 : 20, 0),
                HeightRequest = 40,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Spacing = 0,
                Children =
                {
                    filterGroup,
                    switchFilter
                }
            };

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += viewModel.AllFiltersGestures_Tap;
            filterGroupContainer.GestureRecognizers.Add(tapGestureRecognizer);

            panelContent.Children.Add(filterGroupContainer);

            #endregion

            foreach (var filterCategoryItem in viewModel.FilterCategoryList)
            {
                var filterView = new FilterViewTemplate(viewModel, localViewModel).View;
                filterView.BindingContext = filterCategoryItem;

                panelContent.Children.Add(filterView);
            }

            panelContent.Children.Add(new BoxView
            {
                BackgroundColor = Color.Transparent,
                HeightRequest = 30
            });
        }
    }
}