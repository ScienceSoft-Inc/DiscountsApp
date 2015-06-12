using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

using ScnDiscounts.Views.StyleControl;
using ScnDiscounts.Views.ContextUI;
using ScnDiscounts.ViewModels;
using ScnDiscounts.Control;

namespace ScnDiscounts.Views
{
    class BaseContentPage : ContentPage, IBasePage
    {
        [Flags] 
        public enum ActionBarTypeEnum 
        { 
            abNone = 0,
            abActionBarPanel = 1,
            abTitle = 2,
            abBack = 4,
            abOpacity = 8,

            //Use only this flag
            abBackOpacity = abBack | abOpacity,
            abActionBarTitle = abActionBarPanel | abTitle,
            abActionBarBack = abActionBarPanel | abBack,
            abActionBarBackTitle = abActionBarBack | abTitle
        }

        protected App CurrentApp
        {
            get { return (App)Application.Current; }
        }

        //контекст для UI
        private BaseContext _context;
        public BaseContext Context
        {
            get { return _context; }
        }

        //связанная ViewModel
        private BaseViewModel _viewModel;
        public BaseViewModel ViewModel
        {
            get { return _viewModel; }
        }

        //базовый слой
        private RelativeLayout _baseLayout;

        //слой пользовательского GUI
        private StackLayout _contentLayout;
        public StackLayout ContentLayout
        {
            get { return _contentLayout; } 
        }

        //слой визуализации загрузки
        private StackLayout _loadingLayout;

        //toolbar
        private IList<ToolbarItem> _toolbar;
        public IList<ToolbarItem> Toolbar
        {
            get
            {
                return _toolbar ??
                    (_toolbar = new List<ToolbarItem>());
            }
        }

        private ActionBarTypeEnum actionBarType { get; set; }

        public BaseContentPage(BaseViewModel paramViewModel, BaseContext paramContext, ActionBarTypeEnum actionType = ActionBarTypeEnum.abNone)
        {
            _viewModel = paramViewModel;
            _context = paramContext;
            actionBarType = actionType;

            //указание ViewModel на связанное с ним окно
            _viewModel.SetPage(this, _context);
            
            this.SetBinding(Page.IsBusyProperty, "IsLoadBusy");

            //связь ViewModel для binding свойств
            BindingContext = _viewModel;

            //заголовок окна
            this.SetBinding(ContentPage.TitleProperty, "Title");

            _baseLayout = new RelativeLayout();
            this.Content = _baseLayout;

            InitContentLayout();
            InitLoadingLayout();

            BackgroundColor = _context.MainSystemColor;
        }

        //инициализация слоя пользовательского GUI
        private void InitContentLayout()
        {
            _contentLayout = new StackLayout
            {
                Padding = new Thickness(0)
            };

            _baseLayout.Children.Add(_contentLayout,
                xConstraint: Constraint.Constant(0),
                yConstraint: Constraint.Constant(0),
                widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                heightConstraint: Constraint.RelativeToParent((parent) => { return parent.Height; }));


            //ActionBar
            AbsoluteLayout actionBarLayout = new AbsoluteLayout();

            //Back button for actionbar
            if ((actionBarType & ActionBarTypeEnum.abBack) != 0)
            {
                var imgBackBtn = new ImageButton();

                imgBackBtn.Source = _context.IconBack;

                if ((actionBarType & ActionBarTypeEnum.abOpacity) != 0)
                    imgBackBtn.BackgroundColor = new Color(0, 0, 0, _context.ActionBarBackBtnOpacity);
                else
                    imgBackBtn.BackgroundColor = _context.MainSystemColor;

                var backBtnGestureRecognizer = new TapGestureRecognizer();
                backBtnGestureRecognizer.Tapped += (sender, e) =>
                {
                    ClosePage();
                };
                imgBackBtn.GestureRecognizers.Add(backBtnGestureRecognizer);

                AbsoluteLayout.SetLayoutFlags(imgBackBtn, AbsoluteLayoutFlags.PositionProportional);
                AbsoluteLayout.SetLayoutBounds(imgBackBtn,
                    new Rectangle(0, 0, AbsoluteLayout.AutoSize, _context.ActionBarHeight)
                );
                actionBarLayout.Children.Add(imgBackBtn);
            };

            //Title for actionbar
            if ((actionBarType & ActionBarTypeEnum.abTitle) != 0)
            {
                var txtTitle = new Label();

                txtTitle.Style = _context.ActionBarTextStyle;
                txtTitle.SetBinding(Label.TextProperty, "Title");

                AbsoluteLayout.SetLayoutFlags(
                    txtTitle,
                    AbsoluteLayoutFlags.PositionProportional
                );
                AbsoluteLayout.SetLayoutBounds(
                    txtTitle,
                    new Rectangle(
                        0.5, 				        // X
                        0.5, 				        // Y
                        AbsoluteLayout.AutoSize, 	// Width
                        _context.ActionBarHeight)	// Height
                );
                actionBarLayout.Children.Add(txtTitle);
            }

            if ((actionBarType & ActionBarTypeEnum.abActionBarPanel) != 0)
            {
                actionBarLayout.BackgroundColor = _context.MainSystemColor;
                _contentLayout.Children.Add(actionBarLayout);
            }
            else if ((actionBarType & ActionBarTypeEnum.abNone) == 0)
            {
                _baseLayout.Children.Add(actionBarLayout,
                    xConstraint: Constraint.Constant(0),
                    yConstraint: Constraint.Constant(0));
            }
        }
        
        //инициализация слоя выполнения
        private void InitLoadingLayout()
        {
            _loadingLayout = new StackLayout
            {
                BackgroundColor = new Color(0, 0, 0, 0.8)
            };
            _loadingLayout.SetBinding(StackLayout.IsVisibleProperty, "IsLoadActivity");

            var activityIndicator = new ActivityIndicator
            {
                Color = Device.OnPlatform(Color.Black, Color.Default, Color.Default),
                VerticalOptions = LayoutOptions.EndAndExpand,
            };
            activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoadActivity");
            if (Device.OS == TargetPlatform.Android)
                activityIndicator.HorizontalOptions = LayoutOptions.CenterAndExpand;

            var activityText = new Label
            {
                TextColor = Device.OnPlatform(Color.Black, Color.Default, Color.Default),
                Text = _context.TxtLoading,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            _loadingLayout.Children.Add(activityIndicator);
            _loadingLayout.Children.Add(activityText);

            _baseLayout.Children.Add(_loadingLayout,
                        xConstraint: Constraint.Constant(0),
                        yConstraint: Constraint.Constant(0),
                        widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                        heightConstraint: Constraint.RelativeToParent((parent) => { return parent.Height; }));        
        }

        //переинициализация слоя, необходимо для некоторых контролов
        public void ReloadContentLayout()
        {
            var tmpLayout = this.Content;
            this.Content = null;
            this.Content = tmpLayout;
        }

        //показать toolbar
        public void ShowToolBar()
        {
            ToolbarItems.Clear();

            foreach (var item in Toolbar)
                ToolbarItems.Add(item);
        }

        //скрыть toolbar
        public void HideToolBar()
        {
            ToolbarItems.Clear();
        }

        //обновление GUI для процесса выполнения
        public void UpdateLoadingGUI()
        {
            NavigationPage.SetHasBackButton(this, !ViewModel.IsLoading);

            if (ViewModel.IsLoading)
                HideToolBar();
            else
                ShowToolBar();
        }

        protected override bool OnBackButtonPressed()
        {
            bool IsCancelPress = _viewModel.IsLoading;

            if ((actionBarType & ActionBarTypeEnum.abBack) != 0)
            {
                IsCancelPress = true;
                ClosePage();
            }
            return IsCancelPress;
        }

        private void ClosePage()
        {
            this.Navigation.PopAsync(true); 
        }
    }
}
