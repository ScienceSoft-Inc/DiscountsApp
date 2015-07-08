using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ScnDiscounts.Control;
using ScnDiscounts.iOS.Renderers;
using CoreGraphics;

[assembly: ExportRenderer(typeof(LabelExtended), typeof(LabelExtendedRenderer))]

namespace ScnDiscounts.iOS.Renderers
{
    public class LabelExtendedRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if ((Control != null) && (Element != null))
            {
                var view = (LabelExtended)Element;
                if (view.IsWrapped)
                {
                    Control.LineBreakMode = UILineBreakMode.TailTruncation;
                    Control.Lines = 0;
                }
            }
        }
    }
}