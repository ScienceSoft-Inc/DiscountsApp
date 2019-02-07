using Xamarin.Forms;

namespace ScnDiscounts.Helpers
{
    public static class ResourcesHelper
    {
        public static object FromResources(this string value)
        {
            return Application.Current.Resources[value];
        }

        public static T FromResources<T>(this string value)
        {
            return (T) value.FromResources();
        }
    }
}
