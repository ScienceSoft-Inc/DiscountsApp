using CoreGraphics;
using ScnDiscounts.Control;
using ScnDiscounts.iOS.Renderers;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ButtonExtended), typeof(ButtonExtendedRenderer))]

namespace ScnDiscounts.iOS.Renderers
{
    public class ButtonExtendedRenderer : ButtonRenderer
    {
        public override void Draw(CGRect rect)
        {
            if (Control != null && Element != null)
            {
                var element = (ButtonExtended) Element;

                Control.SetTitleColor(element.TextColor.ToUIColor(), UIControlState.Normal);
                Control.SetTitleColor(element.DisabledTextColor.ToUIColor(), UIControlState.Disabled);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Button.TextColorProperty.PropertyName ||
                e.PropertyName == ButtonExtended.DisabledTextColorProperty.PropertyName)
                SetNeedsDisplay();
        }
    }
}
