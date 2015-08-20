using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

using ScnDiscounts.DependencyInterface;
using ScnDiscounts.Droid.DependencyInterface;
using ScnDiscounts.DependencyInterface.LocalStorage;

[assembly: Dependency(typeof(LocalStorage))]

namespace ScnDiscounts.Droid.DependencyInterface
{
    public class LocalStorage : ILocalStorage
    {
        private string GetStorageDirectory()
        {
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), LocalStorageConfig.StorageDirectory);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        public async Task<string> SaveBase64Async(string fileName, string sreamFile)
        {
            byte[] fileBytes = System.Convert.FromBase64String(sreamFile);

            var dirPath = GetStorageDirectory();
            var path = Path.Combine(dirPath, fileName);

            using (var stream = File.Create(path))
            {
                await stream.WriteAsync(fileBytes, 0, fileBytes.Length);
            }

            return path;
        }

        public async Task<string> SaveTextAsync(string fileName, string text)
        {
            var dirPath = GetStorageDirectory();
            var path = Path.Combine(dirPath, fileName);

            using (StreamWriter sw = File.CreateText(path))
            {
                await sw.WriteAsync(text);
            }

            return path;
        }

        public async Task<string> LoadTextAsync(string fileName)
        {
            string text;
            var dirPath = GetStorageDirectory();
            var path = Path.Combine(dirPath, fileName);

            using (StreamReader sr = File.OpenText(path))
            {
                text = await sr.ReadToEndAsync();
            }
            return text;
        }

        public bool FileExist(string filePath)
        {
            return File.Exists(filePath);
        }

        public string GetFilePath(string fileName)
        {
            var dirPath = GetStorageDirectory();
            var filePath = Path.Combine(dirPath, fileName);

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
            var dirPath = GetStorageDirectory();
            if (Directory.Exists(dirPath))
                Directory.Delete(dirPath, true);
        }
    }
}