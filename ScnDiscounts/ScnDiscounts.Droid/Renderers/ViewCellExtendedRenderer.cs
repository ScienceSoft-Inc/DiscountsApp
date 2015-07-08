using Android.Content;
using Android.Views;
using ScnDiscounts.Control;
using ScnDiscounts.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(ViewCellExtended), typeof(ViewCellExtendedRenderer))]

namespace ScnDiscounts.Droid.Renderers
{
    public class ViewCellExtendedRenderer : ViewCellRenderer
    {
        protected override View GetCellCore(Cell item, View convertView, ViewGroup parent, Context context)
        {
            var extendedCell = (ViewCellExtended)item;

            var cell = base.GetCellCore(item, convertView, parent, context);
            if (cell != null)
                cell.Selected = extendedCell.IsHighlightSelection;
            
            return cell;
        }
    }
}