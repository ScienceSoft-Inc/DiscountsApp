using Polly;
using Refit;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ScnDiscounts.Helpers
{
    public static class HttpHelper
    {
        public static Task<T> RequestAsync<T>(this Task<T> task)
        {
            return Policy.Handle<WebException>()
                .Or<HttpRequestException>()
                .Or<ApiException>()
                .Or<TaskCanceledException>()
                .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Pow(2, i)))
                .ExecuteAsync(() => task);
        }

        public static async Task<byte[]> BytesAsync<T>(this Task<T> task)
            where T : HttpContent
        {
            var response = await task.RequestAsync();
            return await response.ReadAsByteArrayAsync();
        }
    }
}
