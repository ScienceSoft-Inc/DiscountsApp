using Android.Content;
using ScnDiscounts.Control;
using ScnDiscounts.Droid.Renderers;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ButtonExtended), typeof(ButtonExtendedRenderer))]

namespace ScnDiscounts.Droid.Renderers
{
    public class ButtonExtendedRenderer : ButtonRenderer
    {
        public ButtonExtendedRenderer(Context context)
            : base(context)
        {
        }

        private void RedrawButton()
        {
            if (Control != null && Element != null)
            {
                var element = (ButtonExtended) Element;
                var color = element.IsEnabled ? element.TextColor : element.DisabledTextColor;

                Control.SetTextColor(color.ToAndroid());
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            RedrawButton();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Button.TextColorProperty.PropertyName ||
                e.PropertyName == ButtonExtended.DisabledTextColorProperty.PropertyName ||
                e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
                RedrawButton();
        }
    }
}
