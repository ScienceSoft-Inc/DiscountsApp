using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using ScnDiscounts.ViewModels;
using ScnDiscounts.Views;
using ScnDiscounts.Views.ContextUI;

namespace ScnDiscounts.Control
{
    class SideBarPage : ContentPage, IBasePage
    {
        [Flags]
        public enum SideBarViewEnum
        {
            sbLeft = 1,
            sbRight = 2,
            sbLeftBottom = 4,
            sbRightBottom = 8,
            sbLeftRight = sbLeft | sbRight,
            sbLeftRightBottom = sbLeftBottom | sbRightBottom
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

        private SideBarViewEnum sideBarBtnVisible;
        //базовый слой
        private RelativeLayout _baseLayout;

        //слой пользовательского GUI
        static object locker = new object();

        private RelativeLayout _mainLayout;
        public RelativeLayout MainLayout
        {
            get { return _mainLayout; }
        }

        private Image _leftSideBarBtn;
        public Image LeftSideBarBtn
        {
            get { return _leftSideBarBtn; } 
        }

        private RelativeLayout _leftPanelLayout;
        public RelativeLayout LeftPanelLayout
        {
            get { return _leftPanelLayout; }
        }

        private double _leftPanelWidth = 0;
        public double LeftPanelWidth
        {
            get { return _leftPanelWidth; }
            set { _leftPanelWidth = (value > 0) ? value : 0; }
        }
    
        private bool _isShowRightPanel = false;
        public bool IsShowRightPanel
        {
            get
            {
                lock (locker)
                    return _isShowRightPanel;
            }
            set
            {
                lock (locker)
                {
                    _isShowRightPanel = value;

                    if (value)
                        ShowRightPanel();
                    else
                        HideRightPanel();

                    OnPanelChanged(new SideBarEventArgs(value, 1));
                }
            }
        }
        
        private Image _rightSideBarBtn;
        public Image RightSideBarBtn
        {
            get { return _rightSideBarBtn; }
        }

        private RelativeLayout _rightPanelLayout;
        public RelativeLayout RightPanelLayout
        {
            get { return _rightPanelLayout; }
        }

        private double _rightPanelWidth = 0;
        public double RightPanelWidth
        {
            get { return _rightPanelWidth; }
            set { _rightPanelWidth = (value > 0) ? value : 0; }
        }

        private bool _isShowLeftPanel = false;
        public bool IsShowLeftPanel
        {
            get
            {
                lock (locker)
                    return _isShowLeftPanel;
            }
            set
            {
                lock (locker)
                {
                    _isShowLeftPanel = value;

                    if (value)
                        ShowLeftPanel();
                    else
                        HideLeftPanel();

                    OnPanelChanged(new SideBarEventArgs(value, -1));
                }
            }
        }

        public event EventHandler<SideBarEventArgs> PanelChanged;
        public void OnPanelChanged(SideBarEventArgs e)
        {
            if (PanelChanged != null) PanelChanged(this, e);
        }

        private uint _speedAnimatePanel = 300;
        public uint SpeedAnimatePanel
        {
            get { return _speedAnimatePanel; }
            set { _speedAnimatePanel = (value > 0) ? value : 0; }
        }

        public SideBarPage(BaseViewModel paramViewModel, BaseContext paramContext, SideBarViewEnum sideBarBtn)
        {
            _viewModel = paramViewModel;
            _context = paramContext;
            sideBarBtnVisible = sideBarBtn;

            _viewModel.SetPage(this, _context);

            //связь ViewModel для binding свойств
            BindingContext = _viewModel;

            //заголовок окна
            this.SetBinding(ContentPage.TitleProperty, "Title");

            _baseLayout = new RelativeLayout();
            InitSideBar();
            this.Content = _baseLayout;
        }

        private void InitSideBar()
        {
            var tapHideSideBarPanel = new TapGestureRecognizer();
            tapHideSideBarPanel.Tapped += (sender, e) =>
            {
                CloseSideBarPanel();
            };

            _mainLayout = new RelativeLayout
            {
                BackgroundColor = Color.Black,
            };

            _mainLayout.GestureRecognizers.Add(tapHideSideBarPanel);

            _baseLayout.Children.Add(_mainLayout,
                xConstraint: Constraint.Constant(0),
                yConstraint: Constraint.Constant(0),
                widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                heightConstraint: Constraint.RelativeToParent((parent) => { return parent.Height; }));

            #region LeftSideBar button
            _leftSideBarBtn = new Image
            {
                BackgroundColor = Color.Red,
            };

            if (((sideBarBtnVisible & SideBarViewEnum.sbLeft) != 0) || ((sideBarBtnVisible & SideBarViewEnum.sbLeftBottom) != 0))
            {
                var tapSideBarRecognizer = new TapGestureRecognizer();
                tapSideBarRecognizer.Tapped += (sender, e) =>
                {
                    IsShowLeftPanel = !IsShowLeftPanel; 
                };
                _leftSideBarBtn.GestureRecognizers.Add(tapSideBarRecognizer);

                if ((sideBarBtnVisible & SideBarViewEnum.sbLeft) != 0)
                {
                    _baseLayout.Children.Add(_leftSideBarBtn,
                        xConstraint: Constraint.Constant(0),
                        yConstraint: Constraint.Constant(0),
                        widthConstraint: Constraint.Constant(_context.ActionBarHeight),
                        heightConstraint: Constraint.Constant(_context.ActionBarHeight));
                }
                else if ((sideBarBtnVisible & SideBarViewEnum.sbLeftBottom) != 0)
                {
                    _baseLayout.Children.Add(_leftSideBarBtn,
                        xConstraint: Constraint.Constant(0),
                        yConstraint: Constraint.RelativeToParent((parent) => { return parent.Height - _context.ActionBarHeight; }),
                        widthConstraint: Constraint.Constant(_context.ActionBarHeight),
                        heightConstraint: Constraint.Constant(_context.ActionBarHeight));
                }
            }
            #endregion

            #region RightSideBar button
            _rightSideBarBtn = new Image
            {
                BackgroundColor = Color.Red,
            };

            if (((sideBarBtnVisible & SideBarViewEnum.sbRight) != 0) || ((sideBarBtnVisible & SideBarViewEnum.sbRightBottom) != 0))
            {

                var tapSideBarRecognizer = new TapGestureRecognizer();
                tapSideBarRecognizer.Tapped += (sender, e) =>
                {
                    IsShowRightPanel = !IsShowRightPanel;
                };
                _rightSideBarBtn.GestureRecognizers.Add(tapSideBarRecognizer);

                if ((sideBarBtnVisible & SideBarViewEnum.sbRight) != 0)
                {
                    _baseLayout.Children.Add(_rightSideBarBtn,
                        xConstraint: Constraint.RelativeToParent((parent) => { return parent.Width - _context.ActionBarHeight; }),
                        yConstraint: Constraint.Constant(0),
                        widthConstraint: Constraint.Constant(_context.ActionBarHeight),
                        heightConstraint: Constraint.Constant(_context.ActionBarHeight));
                }
                else if ((sideBarBtnVisible & SideBarViewEnum.sbRightBottom) != 0)
                {
                    _baseLayout.Children.Add(_rightSideBarBtn,
                        xConstraint: Constraint.RelativeToParent((parent) => { return parent.Width - _context.ActionBarHeight; }),
                        yConstraint: Constraint.RelativeToParent((parent) => { return parent.Height - _context.ActionBarHeight; }),
                        widthConstraint: Constraint.Constant(_context.ActionBarHeight),
                        heightConstraint: Constraint.Constant(_context.ActionBarHeight));
                }
            }
            #endregion

            #region LeftSideBar panel
            _leftPanelLayout = new RelativeLayout
            {
                BackgroundColor = Color.Black,
            };

            if (((sideBarBtnVisible & SideBarViewEnum.sbLeft) != 0) || ((sideBarBtnVisible & SideBarViewEnum.sbLeftBottom) != 0))
            {

                _leftPanelLayout.GestureRecognizers.Add(tapHideSideBarPanel);

                _baseLayout.Children.Add(_leftPanelLayout,
                    Constraint.RelativeToParent((parent) => 
                    {
                        if (_leftPanelWidth == 0)
                            _leftPanelWidth = parent.Width - _context.ActionBarHeight;

                        return 0 - _leftPanelWidth; 
                    }),
                    Constraint.Constant(0),
                    Constraint.RelativeToParent((parent) => { return _leftPanelWidth; }),
                    Constraint.RelativeToParent((parent) => { return parent.Height; }));
            }
            #endregion

            #region RightSideBar panel
            _rightPanelLayout = new RelativeLayout
            {
                BackgroundColor = Color.Black,
            };
            if (((sideBarBtnVisible & SideBarViewEnum.sbRight) != 0) || ((sideBarBtnVisible & SideBarViewEnum.sbRightBottom) != 0))
            {
                _rightPanelLayout.GestureRecognizers.Add(tapHideSideBarPanel);

                _baseLayout.Children.Add(_rightPanelLayout,
                    Constraint.RelativeToParent((parent) => { return parent.Width; }),
                    Constraint.Constant(0),
                    Constraint.RelativeToParent((parent) =>
                    {
                        if (_rightPanelWidth == 0)
                           _rightPanelWidth = parent.Width - _context.ActionBarHeight;

                        return _rightPanelWidth;
                    }),
                    Constraint.RelativeToParent((parent) => { return parent.Height; }));
            }
            #endregion
        }

        async private void ShowLeftPanel()
        {
            IsShowRightPanel = false;

            await _leftPanelLayout.TranslateTo(_leftPanelWidth, _leftPanelLayout.TranslationY, SpeedAnimatePanel, Easing.CubicOut);
        }

        async private void HideLeftPanel()
        {
            await _leftPanelLayout.TranslateTo(0, _leftPanelLayout.TranslationY, SpeedAnimatePanel, Easing.CubicOut);
        }

        async private void ShowRightPanel()
        {
            IsShowLeftPanel = false;

            await _rightPanelLayout.TranslateTo((0 - _rightPanelWidth), _rightPanelLayout.TranslationY, SpeedAnimatePanel, Easing.CubicOut);
        }

        async private void HideRightPanel()
        {
            await _rightPanelLayout.TranslateTo(0, _rightPanelLayout.TranslationY, SpeedAnimatePanel, Easing.CubicOut);
        }

        public void CloseSideBarPanel()
        {
            IsShowLeftPanel = false;
            IsShowRightPanel = false;
        }

        protected override bool OnBackButtonPressed()
        {
            bool IsCancelPress = IsShowLeftPanel || IsShowRightPanel;

            if (IsCancelPress)
            {
                IsShowLeftPanel = false;
                IsShowRightPanel = false;
            }

            return IsCancelPress;
        }
 
        public void ShowToolBar()
        {
        }

        //скрыть toolbar
        public void HideToolBar()
        {
        }

        public void UpdateLoadingGUI()
        { }
    }

    public class SideBarEventArgs : EventArgs
    {
        public readonly bool IsShow;
        public readonly int Panel;

        public SideBarEventArgs(bool isShow, int panel)
        {
            IsShow = isShow;
            Panel = panel;
        }
    }
}
