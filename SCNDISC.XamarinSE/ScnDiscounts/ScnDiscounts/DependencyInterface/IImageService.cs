using Xamarin.Forms;

namespace ScnDiscounts.DependencyInterface
{
    public interface IImageService
    {
        byte[] GetPinIconForColor(Color color);
    }
}
