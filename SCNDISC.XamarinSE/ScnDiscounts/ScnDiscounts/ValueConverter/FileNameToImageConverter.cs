using ScnDiscounts.Helpers;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace ScnDiscounts.ValueConverters
{
    public class FileNameToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fileName = (string) value;

            string result = null;

            if (!string.IsNullOrEmpty(fileName))
            {
                result = IsolatedStorageHelper.FileExist(fileName)
                    ? fileName
                    : IsolatedStorageHelper.GetFilePath(fileName);
            }

            if (string.IsNullOrEmpty(result))
                result = (bool) parameter ? "img_empty_big.png" : "img_empty_small.png";

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
