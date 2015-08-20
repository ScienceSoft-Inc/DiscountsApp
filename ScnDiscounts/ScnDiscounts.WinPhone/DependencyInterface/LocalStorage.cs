using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Xamarin.Forms;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.WinPhone.DependencyInterface;
using ScnDiscounts.DependencyInterface.LocalStorage;

[assembly: Dependency(typeof(LocalStorage))]

namespace ScnDiscounts.WinPhone.DependencyInterface
{
    public class LocalStorage : ILocalStorage
    {
        private async Task<StorageFolder> GetStorageDirectory()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            return await localFolder.CreateFolderAsync(LocalStorageConfig.StorageDirectory, CreationCollisionOption.OpenIfExists); 
        }

        public async Task<string> SaveBase64Async(string fileName, string sreamFile)
        {
            byte[] fileBytes = System.Convert.FromBase64String(sreamFile);

            StorageFolder localFolder = await GetStorageDirectory();
            IStorageFile file = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            using (var stream = await file.OpenStreamForWriteAsync())
            {
                await stream.WriteAsync(fileBytes, 0, fileBytes.Length);
            }

            return file.Path.ToString();
        }

        public async Task<string> SaveTextAsync(string fileName, string text)
        {
            StorageFolder localFolder = await GetStorageDirectory();
            IStorageFile file = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            using (StreamWriter streamWriter = new StreamWriter(file.Path))
            {
                await streamWriter.WriteAsync(text);
            }

            return file.Path.ToString();
        }

        public async Task<string> LoadTextAsync(string fileName)
        {
            StorageFolder localFolder = await GetStorageDirectory();
            IStorageFile file = await localFolder.GetFileAsync(fileName);
            string text;

            using (StreamReader streamReader = new StreamReader(file.Path))
            {
                text = await streamReader.ReadToEndAsync();
            }

            return text;
        }

        public bool FileExist(string filePath)
        {
            return File.Exists(filePath);
        }

        public string GetFilePath(string fileName)
        {
            var filePath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, LocalStorageConfig.StorageDirectory, fileName);

            if (File.Exists(filePath))
                return filePath;
            else
                return "";
        }

        public void DeleteFile(string fileName)
        {
            var filePath = GetFilePath(fileName);
            if (!String.IsNullOrWhiteSpace(filePath))
                File.Delete(filePath);
        }

        public void ClearStorage()
        {
            var dirPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, LocalStorageConfig.StorageDirectory);
            if (Directory.Exists(dirPath))
                Directory.Delete(dirPath, true);
        }

    }
}
