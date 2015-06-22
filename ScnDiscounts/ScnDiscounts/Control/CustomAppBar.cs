using System;
using Xamarin.Forms;

namespace ScnDiscounts.Control
{
    public class CustomAppBar : AbsoluteLayout
    {
        [Flags]
        public enum CustomBarBtnEnum
        {
            cbNone = 0,
            cbBack = 1,
            cbRight = 2,
            cbRightRight = 4,
            cbLeft = 8,
            cbLeftLeft = 16,
            cbMore = 32, //TODO

            cbRightLeft = cbRight | cbLeft,
            cbRightRightLeft = cbRightRight | cbLeft,
            cbRightLeftLeft = cbRight | cbLeftLeft,
            cbRightRightLeftLeft = cbRightRight | cbLeftLeft,
            
            cbBackRight = cbBack | cbRight,
            cbBackLeft = cbBack | cbLeft,
            cbBackLeftLeft = cbBack | cbLeftLeft,
            
            cbBackRightLeft = cbBackRight | cbLeft,
            cbBackRightLeftLeft = cbBackRight | cbLeftLeft
        }

        private int HeightBar = Device.OnPlatform(48, 48, 64);

        public CustomAppBar(Page page, CustomBarBtnEnum customBarBtn = CustomBarBtnEnum.cbNone)
        {
            NavigationPage.SetHasNavigationBar(page, false);

            HeightRequest = HeightBar;
            BackgroundColor = barColor;
            
            if (Device.OS == TargetPlatform.iOS)
                Padding = new Thickness(0, 14, 0, 0);

            #region Title create
            txtTitle = new Label 
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            SetLayoutFlags(txtTitle, AbsoluteLayoutFlags.PositionProportional);
            SetLayoutBounds(txtTitle,
                new Rectangle(0.5, 0.5, AutoSize, AutoSize)
            );
            Children.Add(txtTitle);
            #endregion

            #region Panel for left buttons
            var stackLeftBtn = new StackLayout
            {
                Padding = new Thickness (0),
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Start
            };
            SetLayoutFlags(stackLeftBtn, AbsoluteLayoutFlags.PositionProportional);
            SetLayoutBounds(stackLeftBtn,
                new Rectangle(0, 0.5, AutoSize, AutoSize));
            Children.Add(stackLeftBtn);
            #endregion

            #region Panel for right buttons
            var stackRightBtn = new StackLayout
            {
                Padding = new Thickness(0),
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.End
            };
            SetLayoutFlags(stackRightBtn, AbsoluteLayoutFlags.PositionProportional);
            SetLayoutBounds(stackRightBtn,
                new Rectangle(1, 0.5, AutoSize, AutoSize));
            Children.Add(stackRightBtn);
            #endregion

            #region Back button create
            BtnBack = new BackImageButton(page);

            if ((customBarBtn & CustomBarBtnEnum.cbBack) != 0)
                stackLeftBtn.Children.Add(BtnBack);

            #endregion

            #region Right button create
            BtnRight = new ImageButton();

            if ((customBarBtn & CustomBarBtnEnum.cbRight) != 0)
                stackRightBtn.Children.Add(BtnRight);
            #endregion

            #region Left button create
            BtnLeft = new ImageButton();

            if ((customBarBtn & CustomBarBtnEnum.cbLeft) != 0)
                stackLeftBtn.Children.Add(BtnLeft);
            #endregion
        }

        #region Background color
        private Color barColor = Color.White;
        public Color BarColor
        {
            get { return barColor; }
            set
            {
                barColor = value;
                BackgroundColor = barColor;
            }
        }
        #endregion

        #region Title label
        private Label txtTitle;
        private string title = "";
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                txtTitle.Text = title;
            }
        }

        public Style TitleStyle
        {
            get { return txtTitle.Style; }
            set { txtTitle.Style = value; }
        }
        #endregion

        public ImageButton BtnBack { get; private set; }
        public ImageButton BtnRight { get;  private set; }
        public ImageButton BtnLeft { get;  private set; }
    
        private class BackImageButton : ImageButton
        {
            private Page curPage;
            public BackImageButton (Page page)
            {
                curPage = page;
            }
            async public override void OnClick()
            {
                base.OnClick();

                await curPage.Navigation.PopAsync(true);
            } 
        }
    }
}
