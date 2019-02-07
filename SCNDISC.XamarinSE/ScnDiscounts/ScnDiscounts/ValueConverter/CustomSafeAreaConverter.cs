using ScnDiscounts.Helpers;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace ScnDiscounts.ValueConverters
{
    public class CustomSafeAreaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (Thickness) value;
            var customSafeAreaFlags = (SafeAreaHelper.CustomSafeAreaFlags) parameter;

            if (!customSafeAreaFlags.HasFlag(SafeAreaHelper.CustomSafeAreaFlags.Left))
                result.Left = 0;

            if (!customSafeAreaFlags.HasFlag(SafeAreaHelper.CustomSafeAreaFlags.Top))
                result.Top = 0;

            if (!customSafeAreaFlags.HasFlag(SafeAreaHelper.CustomSafeAreaFlags.Right))
                result.Right = 0;

            if (!customSafeAreaFlags.HasFlag(SafeAreaHelper.CustomSafeAreaFlags.Bottom))
                result.Bottom = 0;

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
