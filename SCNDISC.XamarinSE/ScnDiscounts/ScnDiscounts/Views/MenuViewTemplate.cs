using ScnDiscounts.Helpers;
using ScnDiscounts.ViewModels;
using ScnDiscounts.Views.Styles;
using Xamarin.Forms;

namespace ScnDiscounts.Views
{
    public class MenuViewTemplate : ContentView
    {
        public MenuViewTemplate(MainViewModel parentViewModel)
        {
            var imgMenuItem = new Image
            {
                WidthRequest = 36,
                HeightRequest = 36
            };
            imgMenuItem.SetBinding(Image.SourceProperty, "Icon");

            var txtMenuItem = new Label
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Style = LabelStyles.MenuStyle.FromResources<Style>()
            };
            txtMenuItem.SetBinding(Label.TextProperty, "Title");

            var stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(40, 0),
                Spacing = 16,
                Children =
                {
                    imgMenuItem,
                    txtMenuItem
                }
            };

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandParameterProperty, "TypeName");
            tapGestureRecognizer.Tapped += parentViewModel.MenuItem_Tap;
            stackLayout.GestureRecognizers.Add(tapGestureRecognizer);

            Content = stackLayout;
        }
    }
}
