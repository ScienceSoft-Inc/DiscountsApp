using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using ScnDiscounts.Helpers;

namespace ScnDiscounts.ValueConverters
{
    public class FileNameToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fileName = (string)value;
            var imageSource = "";

            if (IsolatedStorageHelper.FileExist(fileName))
                imageSource = fileName;
            else
                imageSource = IsolatedStorageHelper.GetFilePath(fileName);

            if (String.IsNullOrWhiteSpace(imageSource))
                imageSource = Device.OnPlatform("Image/image.png", "ic_image.png", "Assets/Image/image.png");

            return imageSource;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
