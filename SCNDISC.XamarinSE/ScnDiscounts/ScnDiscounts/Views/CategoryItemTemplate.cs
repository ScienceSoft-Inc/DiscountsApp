using ScnDiscounts.Helpers;
using ScnDiscounts.ValueConverter;
using ScnDiscounts.Views.Styles;
using Xamarin.Forms;

namespace ScnDiscounts.Views
{
    public class CategoryItemTemplate : ContentView
    {
        protected static readonly TextCapitalizationConverter TextCapitalizationConverter = new TextCapitalizationConverter();
        protected static readonly CategoryColorConverter CategoryColorConverter = new CategoryColorConverter();

        public CategoryItemTemplate()
        {
            var txtCategory = new Label
            {
                Style = LabelStyles.CategoryStyle.FromResources<Style>()
            };
            txtCategory.SetBinding(Label.TextProperty, "Name", converter: TextCapitalizationConverter);

            this.SetBinding(BackgroundColorProperty, "Color", converter: CategoryColorConverter);

            Padding = 4;

            Content = txtCategory;
        }
    }
}
