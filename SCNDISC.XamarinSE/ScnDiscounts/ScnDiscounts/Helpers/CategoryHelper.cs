using ScnDiscounts.DependencyInterface;
using ScnDiscounts.Models.Data;
using System.Collections.Concurrent;
using System.IO;
using Xamarin.Forms;

namespace ScnDiscounts.Helpers
{
    public static class CategoryHelper
    {
        private const int NoIconId = -1;
        private const int DisabledIconId = -2;

        private static ConcurrentDictionary<int, byte[]> Icons { get; } = new ConcurrentDictionary<int, byte[]>();

        public static Color GetColorTheme(this string color)
        {
            var result = Color.Default;

            if (!string.IsNullOrEmpty(color))
                result = Color.FromHex(color);

            if (result == Color.Default)
                result = Color.Black;

            return result;
        }

        public static Color GetColorTheme(this CategoryData category)
        {
            return category != null ? GetColorTheme(category.Color) : Color.Black;
        }

        public static byte[] GetIconThemeBytes(this CategoryData category, bool isEnabled = true)
        {
            var id = isEnabled 
                ? category?.Id ?? NoIconId 
                : DisabledIconId;

            return Icons.GetOrAdd(id, i =>
            {
                var color = isEnabled ? category.GetColorTheme() : Color.Gray;
                return DependencyService.Get<IImageService>().GetPinIconForColor(color);
            });
        }

        public static ImageSource GetIconThemeImage(this CategoryData category, bool isEnabled = true)
        {
            return ImageSource.FromStream(() =>
            {
                var bytes = GetIconThemeBytes(category, isEnabled);
                return new MemoryStream(bytes);
            });
        }
    }
}
