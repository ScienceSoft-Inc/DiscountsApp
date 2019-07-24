using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace ScnDiscounts.Helpers
{
    public static class Functions
    {
        public static bool IsDebug =>
#if DEBUG
            true;
#else
            false;
#endif

        public static bool SafeCall(this Action action)
        {
            var result = true;

            try
            {
                action();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                result = false;
            }

            return result;
        }

        public static string SafeTrim(this string value)
        {
            return string.IsNullOrEmpty(value) ? value : value.Trim();
        }

        public static string NormalizePhoneNumber(this string value)
        {
            return string.IsNullOrEmpty(value)
                ? value
                : Regex.Replace(value.Trim().ToLower(), @"[^\+\dx]", string.Empty);
        }

        public static string NormalizeLink(this string value)
        {
            var result = value.SafeTrim();

            if (!string.IsNullOrEmpty(result) &&
                !result.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !result.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                result = $"http://{result}";

            return result;
        }

        public static string NormalizePartnerName(this string value)
        {
            return string.IsNullOrEmpty(value)
                ? value
                : Regex.Replace(value.Trim(), "[\"'`]", string.Empty);
        }

        public static T OnPlatform<T>(T ios = default, T android = default, T uwp = default)
        {
            T result;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    result = ios;
                    break;
                case Device.Android:
                    result = android;
                    break;
                case Device.UWP:
                    result = uwp;
                    break;
                default:
                    result = default;
                    break;
            }

            return result;
        }
    }
}
