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
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var extendedCell = (ViewCellExtended)item;

            var cell = base.GetCell(item, reusableCell, tv);
            if (cell != null)
            {
                cell.SelectionStyle = extendedCell.HighlightSelection ? UITableViewCellSelectionStyle.Default : UITableViewCellSelectionStyle.None;
            }

            return cell;
        }
    }
}