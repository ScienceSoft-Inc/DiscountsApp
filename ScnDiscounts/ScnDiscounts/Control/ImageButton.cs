using System;
using Xamarin.Forms;

namespace ScnDiscounts.Control
{
    public class ImageButton : AbsoluteLayout
    {
        public ImageButton()
        {
            BackgroundColor = Color.Red;
            image = new Image();
            
            SetLayoutFlags(image, AbsoluteLayoutFlags.PositionProportional);
            SetLayoutBounds(image,
                new Rectangle(0.5, 0.5, image.Width, image.Height)
            );
            Children.Add(image);

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += (sender, e) =>
            {
                OnClick();
            };

            GestureRecognizers.Add(tapGesture);
            image.GestureRecognizers.Add(tapGesture);
        }

        private Image image;
        public ImageSource Source
        {
            get { return image.Source; }
            set { image.Source = value; }
        }

        public event EventHandler Click;
        public virtual void OnClick()
        {
            if (Click != null) Click(this, EventArgs.Empty);
        }
    }
}
