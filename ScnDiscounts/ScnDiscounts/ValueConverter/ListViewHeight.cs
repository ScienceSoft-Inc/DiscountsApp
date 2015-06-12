using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace ScnDiscounts.ValueConverter
{
    public class ListViewHeight : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var items = (List<String>)value;
            int itemHeight = (int)parameter;

            return items.Count * itemHeight;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
