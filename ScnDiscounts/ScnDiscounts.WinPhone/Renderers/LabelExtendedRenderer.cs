using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

using ScnDiscounts.Control;
using ScnDiscounts.WinPhone.Renderers;
using System.Windows.Media;

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
                var view = (LabelExtended)Element;

                if (view.IsWrapped)
                {
                    Control.TextWrapping = System.Windows.TextWrapping.Wrap;
                    Control.TextTrimming = System.Windows.TextTrimming.WordEllipsis;

                    Control.Loaded += (s, args) =>
                    {
                        var parent = Control.Parent as LabelExtendedRenderer;

                        if (parent != null)
                        {
                            var grid = new System.Windows.Controls.Grid();
                            parent.Children.Remove(Control);
                            parent.Children.Add(grid);
                            grid.Children.Add(Control);
                        }
                    };
                }
            }
        }
    }
}
