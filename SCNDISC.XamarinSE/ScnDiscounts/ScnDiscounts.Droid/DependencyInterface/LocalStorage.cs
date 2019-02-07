using ScnDiscounts.DependencyInterface;
using ScnDiscounts.DependencyInterface.LocalStorage;
using ScnDiscounts.Droid.DependencyInterface;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocalStorage))]

namespace ScnDiscounts.Droid.DependencyInterface
{
    public class LocalStorage : ILocalStorage
    {
        private static string GetStorageDirectory()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), LocalStorageConfig.StorageDirectory);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        public async Task<string> SaveBase64Async(string fileName, string base64)
        {
            var fileBytes = Convert.FromBase64String(base64);

            var dirPath = GetStorageDirectory();
            var path = Path.Combine(dirPath, fileName);

            using (var stream = File.Create(path))
            {
                await stream.WriteAsync(fileBytes, 0, fileBytes.Length);
            }

            return path;
        }

        public async Task<string> SaveStreamAsync(string fileName, Stream stream)
        {
            var dirPath = GetStorageDirectory();
            var path = Path.Combine(dirPath, fileName);

            using (var fileStream = File.Create(path))
            {
                stream.Position = 0;
                await stream.CopyToAsync(fileStream);
            }

            return path;
        }

        public async Task<string> SaveTextAsync(string fileName, string text)
        {
            var dirPath = GetStorageDirectory();
            var path = Path.Combine(dirPath, fileName);

            using (var sw = File.CreateText(path))
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

            using (var sr = File.OpenText(path))
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

            return File.Exists(filePath) ? filePath : string.Empty;
        }

        public void DeleteFile(string fileName)
        {
            var filePath = GetFilePath(fileName);
            if (!string.IsNullOrEmpty(filePath))
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