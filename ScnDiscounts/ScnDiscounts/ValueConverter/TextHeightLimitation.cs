using System;
using System.Globalization;
using Xamarin.Forms;

namespace ScnDiscounts.ValueConverter
{
    public class TextHeightLimitation : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = (string)value;

            if (text.Length > 145)
                text = text.Remove(145);
            
            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
