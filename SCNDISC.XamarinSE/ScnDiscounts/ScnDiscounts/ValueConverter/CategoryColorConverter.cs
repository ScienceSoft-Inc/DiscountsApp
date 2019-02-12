using ScnDiscounts.Helpers;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace ScnDiscounts.ValueConverter
{
    public class CategoryColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (string) value;
            return color.GetColorTheme();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
