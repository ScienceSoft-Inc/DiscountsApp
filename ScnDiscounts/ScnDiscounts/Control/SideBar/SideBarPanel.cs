using System;
using Xamarin.Forms;

namespace ScnDiscounts.Control.SideBar
{
    public class SideBarPanel : AbsoluteLayout
    {
        public SideBarPanel()
        {
            BackgroundColor = Color.White;
            VerticalOptions = LayoutOptions.FillAndExpand;
            HorizontalOptions = LayoutOptions.EndAndExpand;
            
            var tapSideBar = new TapGestureRecognizer();
            tapSideBar.Tapped += (sender, e) => { OnClick(); };
            GestureRecognizers.Add(tapSideBar);
        }

        public event EventHandler Click;
        public void OnClick()
        {
            if (Click != null) Click(this, EventArgs.Empty);
        }

        private View context;
        public View Context
        {
            get { return context; }
            set
            {
                context = value;

                Children.Clear();

                SetLayoutFlags(context, AbsoluteLayoutFlags.All);
                SetLayoutBounds(context, new Rectangle(0f, 0f, 1f, 1f));
                Children.Add(context);
            }
        }
    }
}
