using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Plugin.CurrentActivity;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.Droid.DependencyInterface;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;

[assembly: Dependency(typeof(ImageService))]

namespace ScnDiscounts.Droid.DependencyInterface
{
    public class ImageService : IImageService
    {
        public byte[] GetPinIconForColor(Color color)
        {
            var context = CrossCurrentActivity.Current.Activity;

            using (var drawable =
                (BitmapDrawable) ContextCompat.GetDrawable(context, Resource.Drawable.ic_pin_disabled).Mutate())
            using (var bitmap =
                Bitmap.CreateBitmap(drawable.Bitmap.Width, drawable.Bitmap.Height, Bitmap.Config.Argb8888))
            {
                using (var canvas = new Canvas(bitmap))
                {
                    drawable.SetBounds(0, 0, canvas.Width, canvas.Height);
                    drawable.SetColorFilter(color.ToAndroid(), PorterDuff.Mode.SrcAtop);
                    drawable.Draw(canvas);
                }

                using (var stream = new MemoryStream())
                {
                    bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);

                    return stream.ToArray();
                }
            }
        }
    }
}