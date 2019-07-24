using Android.Graphics;
using Java.Net;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ScnDiscounts.Droid.Helpers
{
    public static class BitmapHelper
    {
        public static async Task<Bitmap> GetBitmapFromUrlAsync(string imageUrl)
        {
            Bitmap result;

            try
            {
                var url = new URL(imageUrl);
                var connection = (HttpURLConnection) url.OpenConnection();
                connection.DoInput = true;
                await connection.ConnectAsync();
                result = await BitmapFactory.DecodeStreamAsync(connection.InputStream);
                connection.Disconnect();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                result = null;
            }

            return result;
        }

        public static Bitmap GetBitmapFromUrl(string imageUrl)
        {
            return GetBitmapFromUrlAsync(imageUrl).Result;
        }
    }
}