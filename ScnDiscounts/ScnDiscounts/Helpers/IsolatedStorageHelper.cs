using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ScnDiscounts.DependencyInterface;

namespace ScnDiscounts.Helpers
{
    public class IsolatedStorageHelper
    {
        //сохранение base64 в файл
        static public async Task<string> SaveBase64Async(string fileName, string streamImage)
        {
            return await DependencyService.Get<ILocalStorage>().SaveBase64Async(fileName, streamImage);
        }

        static public bool FileExist(string fileName)
        {
            return DependencyService.Get<ILocalStorage>().FileExist(fileName);
        }

        static public string GetFilePath(string fileName)
        {
            return DependencyService.Get<ILocalStorage>().GetFilePath(fileName);
        }

        static public void DeleteFile(string fileName)
        {
            DependencyService.Get<ILocalStorage>().DeleteFile(fileName);
        }

        static public void ClearStorage()
        {
            DependencyService.Get<ILocalStorage>().ClearStorage();
        }
    }
}
