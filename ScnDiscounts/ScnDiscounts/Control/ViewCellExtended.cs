using Xamarin.Forms;

namespace ScnDiscounts.Control
{
    public class ViewCellExtended : ViewCell
    {
        #region IsHighlightSelection
        public static readonly BindableProperty IsHighlightSelectionProperty =
            BindableProperty.Create<ViewCellExtended, bool>(p => p.IsHighlightSelection, true);

        public bool IsHighlightSelection
        {
            get { return (bool)GetValue(IsHighlightSelectionProperty); }
            set { SetValue(IsHighlightSelectionProperty, value); }
        }
        #endregion

        #region SelectColor
        public static readonly BindableProperty SelectColorProperty =
            BindableProperty.Create<ViewCellExtended, Color>(p => p.SelectColor, Color.Transparent);

        public Color SelectColor
        {
            get { return (Color)GetValue(SelectColorProperty); }
            set { SetValue(SelectColorProperty, value); }
        }
        #endregion

    }
}
