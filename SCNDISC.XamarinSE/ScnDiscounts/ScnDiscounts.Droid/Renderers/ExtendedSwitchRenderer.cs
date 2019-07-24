using Android.Content;
using Android.Graphics;
using ScnDiscounts.Droid.Renderers;
using System.ComponentModel;
using Android.OS;
using FFImageLoading.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;
using Switch = Xamarin.Forms.Switch;

[assembly: ExportRenderer(typeof(Switch), typeof(ExtendedSwitchRenderer))]

namespace ScnDiscounts.Droid.Renderers
{
    public class ExtendedSwitchRenderer : SwitchRenderer
    {
        public ExtendedSwitchRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Switch> e)
        {
            base.OnElementChanged(e);

            UpdateColors();

            if (Control != null)
            {
                Control.TextOn = string.Empty;
                Control.TextOff = string.Empty;

                if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
                    Control.SwitchMinWidth = 50.DpToPixels();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Switch.IsToggledProperty.PropertyName)
                UpdateColors();
        }

        private void UpdateColors()
        {
            if (Control != null && Element != null)
            {
                var color = Element.IsToggled ? Element.OnColor : Color.Gray;
                Control.ThumbDrawable.SetColorFilter(color.ToAndroid(), PorterDuff.Mode.SrcAtop);
                //Control.TrackDrawable.SetColorFilter(color.ToAndroid(), PorterDuff.Mode.SrcAtop);
            }
        }
    }
}