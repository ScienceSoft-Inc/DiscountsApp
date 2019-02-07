using Android.Content;
using Android.Widget;
using ScnDiscounts.Control;
using ScnDiscounts.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SearchBarExtended), typeof(SearchBarExtendedRenderer))]

namespace ScnDiscounts.Droid.Renderers
{
    public class SearchBarExtendedRenderer : SearchBarRenderer
    {
        public SearchBarExtendedRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if (Control != null && e.NewElement != null)
            {
                var autoCompleteTextViewId =
                    Control.Context.Resources.GetIdentifier("android:id/search_src_text", null, null);
                var autoCompleteTextView = (AutoCompleteTextView) Control.FindViewById(autoCompleteTextViewId);
                autoCompleteTextView.LayoutParameters.Height = LayoutParams.MatchParent;
            }
        }
    }
}