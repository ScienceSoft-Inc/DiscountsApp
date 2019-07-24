using ScnDiscounts.ValueConverters;
using Xamarin.Forms;

namespace ScnDiscounts.Views
{
    public class GalleryImageItemTemplate : Image
    {
        protected static readonly FileNameToImageConverter FileNameConverter = new FileNameToImageConverter();

        public GalleryImageItemTemplate()
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;

            SetBinding(SourceProperty, new Binding(".", BindingMode.Default, FileNameConverter, true));
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == SourceProperty.PropertyName)
                Aspect = (FileImageSource) Source == "img_empty_big.png" ? Aspect.AspectFit : Aspect.AspectFill;
        }
    }
}
