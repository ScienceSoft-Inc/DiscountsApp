using System.Collections.Generic;
using Xamarin.Forms;

namespace ScnDiscounts.Views
{
    class SelectionPage : ContentPage
    {
        public ListView SelList;

        public SelectionPage(string title, List<string> items)
        {
            var txtTitle = new Label
            {
                Text = title
            };
            
            SelList = new ListView();
            SelList.ItemsSource = items;
            SelList.ItemTemplate = new DataTemplate(typeof(SelectTemplate));
            SelList.ItemTapped += SelList_ItemTapped;

            var stackSelection = new StackLayout
            {
                BackgroundColor = Color.Transparent,
                Padding = new Thickness(16),
                Children =
                {
                    txtTitle,
                    SelList
                }
            };

            Content = stackSelection;
        }

        void SelList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.PopModalAsync();
        }

        class SelectTemplate : TextCell
        {
            public SelectTemplate()
            {
                this.SetBinding(TextProperty, ".");
            }
        }
    }
}
