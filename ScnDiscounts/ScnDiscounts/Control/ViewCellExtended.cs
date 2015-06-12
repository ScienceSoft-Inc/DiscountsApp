using Xamarin.Forms;

namespace ScnDiscounts.Control
{
    public class ViewCellExtended : ViewCell
    {
        public static readonly BindableProperty HighlightSelectionProperty =
            BindableProperty.Create<ViewCellExtended, bool>(p => p.HighlightSelection, true);

        public bool HighlightSelection
        {
            get { return (bool)GetValue(HighlightSelectionProperty); }
            set { SetValue(HighlightSelectionProperty, value); }
        }
    }
}
