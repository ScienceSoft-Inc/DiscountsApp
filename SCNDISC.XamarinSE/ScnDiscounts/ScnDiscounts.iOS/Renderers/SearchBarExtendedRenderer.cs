using CoreGraphics;
using ScnDiscounts.Control;
using ScnDiscounts.iOS.Renderers;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SearchBarExtended), typeof(SearchBarExtendedRenderer))]

namespace ScnDiscounts.iOS.Renderers
{
    public class SearchBarExtendedRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if (Control != null && e.NewElement != null)
            {
                Control.SearchBarStyle = UISearchBarStyle.Minimal;
                Control.SetShowsCancelButton(false, false);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == SearchBar.TextProperty.PropertyName)
                Control.SetShowsCancelButton(false, false);
        }

        public override CGSize SizeThatFits(CGSize size)
        {
            return new CGSize(20, 20);
        }
    }
}