using CoreGraphics;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.iOS.DependencyInterface;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(ImageService))]

namespace ScnDiscounts.iOS.DependencyInterface
{
    public class ImageService : IImageService
    {
        public byte[] GetPinIconForColor(Color color)
        {
            var image = UIImage.FromBundle("ic_pin_disabled.png");

            UIGraphics.BeginImageContextWithOptions(image.Size, false, image.CurrentScale);
            using (var context = UIGraphics.GetCurrentContext())
            {
                context.TranslateCTM(0, image.Size.Height);
                context.ScaleCTM(1.0f, -1.0f);

                var rect = new CGRect(0, 0, image.Size.Width, image.Size.Height);

                context.SetBlendMode(CGBlendMode.Normal);
                context.DrawImage(rect, image.CGImage);

                context.SetBlendMode(CGBlendMode.SourceAtop);
                context.SetFillColor(color.ToCGColor());
                context.FillRect(rect);

                var coloredImage = UIGraphics.GetImageFromCurrentImageContext();
                var bytes = coloredImage.AsPNG().ToArray();

                UIGraphics.EndImageContext();

                return bytes;
            }
        }
    }
}