using ScnDiscounts.DependencyInterface;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ScnDiscounts.Helpers
{
    public class IsolatedStorageHelper
    {
        protected static readonly ILocalStorage Instance = DependencyService.Get<ILocalStorage>();

        public static async Task<string> SaveBase64Async(string fileName, string base64)
        {
            return await Instance.SaveBase64Async(fileName, base64);
        }

        public static async Task<string> SaveStreamAsync(string fileName, Stream stream)
        {
            return await Instance.SaveStreamAsync(fileName, stream);
        }

        public static bool FileExist(string fileName)
        {
            return Instance.FileExist(fileName);
        }

        public static string GetFilePath(string fileName)
        {
            return Instance.GetFilePath(fileName);
        }

        public static void DeleteFile(string fileName)
        {
            Instance.DeleteFile(fileName);
        }

        public static void ClearStorage()
        {
            Instance.ClearStorage();
        }
    }
}
