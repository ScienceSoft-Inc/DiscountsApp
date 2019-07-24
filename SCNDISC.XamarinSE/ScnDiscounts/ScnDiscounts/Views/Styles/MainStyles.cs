using ScnDiscounts.Helpers;
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
        public static string SwitchColor = "SwitchColor";
        public static string LoadingColor = "LoadingColor";

        public override void Load()
        {
            var mainBackgroundColor = Color.FromRgb(0, 94, 184);
            var mainLightBackgroundColor = Color.FromRgb(238, 238, 238);
            var listBorderColor = Color.FromRgb(204, 204, 204);
            var listBackgroundColor = Color.FromRgb(255, 255, 255);
            var listSelectColor = Color.FromRgb(100, 194, 255);
            var lightTextColor = Color.FromRgb(255, 255, 255);
            var statusBarColor = Color.FromRgb(40, 120, 192);
            var switchColor = Functions.OnPlatform(Color.FromHex("4B8DCC"), Color.White);
            var loadingColor = Color.White;

            Resources.Add(MainBackgroundColor, mainBackgroundColor);
            Resources.Add(MainLightBackgroundColor, mainLightBackgroundColor);
            Resources.Add(ListBorderColor, listBorderColor);
            Resources.Add(ListBackgroundColor, listBackgroundColor);
            Resources.Add(ListSelectColor, listSelectColor);
            Resources.Add(LightTextColor, lightTextColor);
            Resources.Add(StatusBarColor, statusBarColor);
            Resources.Add(SwitchColor, switchColor);
            Resources.Add(LoadingColor, loadingColor);
        }
    }
}
