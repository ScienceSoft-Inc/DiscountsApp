using System;
using System.Globalization;
using Xamarin.Forms;

namespace ScnDiscounts.ValueConverter
{
    public class TextCapitalizationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = (string) value;
            return input?.ToUpper(culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = (string) value;
            return input?.ToUpper(culture);
        }
    }
}
