using Xamarin.Forms;

namespace ScnDiscounts.Views.Styles
{
    public class MainStyles : BaseStyles
    {
        public static string MainBackgroundColor = "MainBackgroundColor";
        public static string MainLightBackgroundColor = "MainLightBackgroundColor";
        public static string ListBorderColor = "ListBorderColor";
        public static string ListBackgroundColor = "ListBackgroundColor";
        public static string ListSelectColor = "ListSelectColor";
        public static string LightTextColor = "LightTextColor";
        public static string StatusBarColor = "StatusBarColor";

        public override void Load()
        {
            Color mainBackgroundColor = Color.FromRgb(0, 94, 184);
            Color mainLightBackgroundColor = Color.FromRgb(238, 238, 238);
            Color listBorderColor = Color.FromRgb(204, 204, 204);
            Color listBackgroundColor = Color.FromRgb(255, 255, 255);
            Color listSelectColor = Color.FromRgb(100, 194, 255);
            Color lightTextColor = Color.FromRgb(255, 255, 255);
            Color statusBarColor = Color.FromRgb(40, 120, 192);

            Resources.Add(MainBackgroundColor, mainBackgroundColor);
            Resources.Add(MainLightBackgroundColor, mainLightBackgroundColor);
            Resources.Add(ListBorderColor, listBorderColor);
            Resources.Add(ListBackgroundColor, listBackgroundColor);
            Resources.Add(ListSelectColor, listSelectColor);
            Resources.Add(LightTextColor, lightTextColor);
            Resources.Add(StatusBarColor, statusBarColor);
        }
    }
}
