using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace ScnDiscounts.ValueConverter
{
    public class ListViewHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int count = (int)value;
            int itemHeight = (int)parameter;

            return count * itemHeight;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
