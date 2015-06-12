using Android.Graphics;
using Android.Text;
using ScnDiscounts.Control;
using ScnDiscounts.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(LabelExtended), typeof(LabelExtendedRenderer))]

namespace ScnDiscounts.Droid.Renderers
{
    public class LabelExtendedRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            var view = (LabelExtended)Element;

            if (Control != null)
            {
                Control.LayoutChange += (s, args) =>
                {
                    if (view.IsWrapped)
                    {
                        Control.Ellipsize = TextUtils.TruncateAt.End;
                        Control.SetMaxLines((args.Bottom - args.Top) / Control.LineHeight);
                    };
                };

                if (view.IsUnderline)
                {
                    Control.PaintFlags = Control.PaintFlags | PaintFlags.UnderlineText;
                }
            }
        }
    }
}