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

[assembly: ExportRenderer(typeof(ViewCellExtended), typeof(ViewCellExtendedRenderer))]

namespace ScnDiscounts.iOS.Renderers
{
    public class ViewCellExtendedRenderer : ViewCellRenderer
    {
        private UIView selView;

        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var viewCell = (ViewCellExtended)item;

            var cell = base.GetCell(item, reusableCell, tv);
            if (cell != null)
            {
                cell.SelectionStyle = viewCell.IsHighlightSelection ? UITableViewCellSelectionStyle.Default : UITableViewCellSelectionStyle.None;


                if (viewCell.SelectColor != Color.Transparent)
                {
                    if (selView == null)
                    {
                        selView = new UIView(cell.SelectedBackgroundView.Bounds);
                        selView.BackgroundColor = new UIColor((nfloat)viewCell.SelectColor.R, (nfloat)viewCell.SelectColor.G, (nfloat)viewCell.SelectColor.B, (nfloat)viewCell.SelectColor.A);
                        //selView.Layer.BackgroundColor = UIColor.Blue;
                        //selView.Layer.BorderColor = UIColor.Yellow;
                        //selView.Layer.BorderWidth = 2.0f;
                    }

                    cell.SelectedBackgroundView = selView;
                }
            }

            return cell;
        }
    }
}