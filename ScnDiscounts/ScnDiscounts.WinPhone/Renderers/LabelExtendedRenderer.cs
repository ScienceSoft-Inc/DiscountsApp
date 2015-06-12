using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

using ScnDiscounts.Control;
using ScnDiscounts.WinPhone.Renderers;

[assembly: ExportRenderer(typeof(LabelExtended), typeof(LabelExtendedRenderer))]

namespace ScnDiscounts.WinPhone.Renderers
{
    public class LabelExtendedRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.TextWrapping = System.Windows.TextWrapping.Wrap;
                Control.TextTrimming = System.Windows.TextTrimming.WordEllipsis;
            }
        }
    }
}
