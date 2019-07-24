using FFImageLoading.Forms;
using ScnDiscounts.Helpers;
using ScnDiscounts.ViewModels;
using ScnDiscounts.Views.Styles;
using ScnPage.Plugin.Forms;
using Xamarin.Forms;

namespace ScnDiscounts.Views
{
    public class FilterViewTemplate : ContentView
    {
        public FilterViewTemplate(MainViewModel parentViewModel, BaseViewModel localViewModel)
        {
            #region Title

            var imgFilterItem = new CachedImage
            {
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 28,
                HeightRequest = 28
            };
            imgFilterItem.SetBinding(CachedImage.SourceProperty, "Icon");

            var txtFilterItem = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Triggers =
                {
                    new DataTrigger(typeof(Label))
                    {
                        Binding = new Binding("IsToggle"),
                        Value = true,
                        Setters =
                        {
                            new Setter
                            {
                                Property = StyleProperty,
                                Value = LabelStyles.MenuStyle.FromResources()
                            }
                        }
                    },
                    new DataTrigger(typeof(Label))
                    {
                        Binding = new Binding("IsToggle"),
                        Value = false,
                        Setters =
                        {
                            new Setter
                            {
                                Property = StyleProperty,
                                Value = LabelStyles.MenuDisabledStyle.FromResources()
                            }
                        }
                    }
                }
            };
            txtFilterItem.SetBinding(Label.TextProperty, "Name");

            var stackTitle = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Spacing = 16,
                Children =
                {
                    imgFilterItem,
                    txtFilterItem
                }
            };

            #endregion

            #region Toggle

            var switchFilter = new Switch
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                OnColor = MainStyles.SwitchColor.FromResources<Color>()
            };
            switchFilter.SetBinding(Switch.IsToggledProperty, "IsToggle");

            switch (localViewModel)
            {
                case MainViewModel mainViewModel:
                    switchFilter.Toggled += mainViewModel.OnFilterCategorySwitched;
                    break;
                case DiscountViewModel discountViewModel:
                    switchFilter.Toggled += discountViewModel.OnFilterCategorySwitched;
                    break;
            }

            #endregion

            var stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(App.IsMoreThan320Dpi ? 40 : 20, 4),
                Spacing = 0,
                Children =
                {
                    stackTitle,
                    switchFilter
                }
            };

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += parentViewModel.FilterGestures_Tap;
            stackLayout.GestureRecognizers.Add(tapGestureRecognizer);

            Content = stackLayout;
        }
    }
}
