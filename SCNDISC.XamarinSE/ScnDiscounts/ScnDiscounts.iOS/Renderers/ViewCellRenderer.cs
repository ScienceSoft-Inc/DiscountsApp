using ScnDiscounts.iOS.Renderers;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(ViewCell), typeof(ViewCellRenderer))]

namespace ScnDiscounts.iOS.Renderers
{
    public class ViewCellRenderer : Xamarin.Forms.Platform.iOS.ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var result = base.GetCell(item, reusableCell, tv);

            if (result != null)
                result.SelectionStyle = UITableViewCellSelectionStyle.None;

            return result;
        }
    }
}