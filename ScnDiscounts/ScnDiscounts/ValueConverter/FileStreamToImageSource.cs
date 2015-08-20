using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace ScnDiscounts.ValueConverter
{
    public class FileStreamToImageSource : IValueConverter
    {
        public enum SizeImage { siSmall, siBig } 

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Device.OnPlatform("Image/img_logo.png", "img_logo.png", "Assets/Image/img_logo.png");

            /*var base64Stream = (string)value;
            if (String.IsNullOrEmpty(base64Stream))
            {
                SizeImage sizeImage = (SizeImage)parameter;
    
                if (sizeImage == SizeImage.siBig)
                    return Device.OnPlatform("", "img_empty_big.png", "");
                if (sizeImage == SizeImage.siSmall)
                    return Device.OnPlatform("", "img_empty_small.png", "");
            }*/

            //byte[] imageBytes = System.Convert.FromBase64String(base64Stream);

            //ImageSource imgSource = null;

            /*using (var stream = new MemoryStream(imageBytes))
            {
                return ImageSource.FromStream(() => { return stream; });
            }*/
            //return imgSource;

            //ImageSource. 

            //return ImageSource.FromStream(() => { return new MemoryStream(imageBytes); });
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
