using ScnDiscounts.DependencyInterface;
using ScnDiscounts.iOS.DependencyInterface;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocalStorage))]

namespace ScnDiscounts.iOS.DependencyInterface
{
    public class LocalStorage : ILocalStorage
    {
        private static string GetStorageDirectory()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);// Documents folder
            var libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            var storagePath = Path.Combine(libraryPath, "ScnDiscountsStorage");

            if (!Directory.Exists(storagePath))
                Directory.CreateDirectory(storagePath);

            return storagePath;
        }

        public async Task<string> SaveBase64Async(string fileName, string base64)
        {
            var fileBytes = Convert.FromBase64String(base64);

            var storagePath = GetStorageDirectory();
            var filePath = Path.Combine(storagePath, fileName);

            using (var stream = File.Create(filePath))
            {
                await stream.WriteAsync(fileBytes, 0, fileBytes.Length);
            }

            return filePath;
        }

        public async Task<string> SaveStreamAsync(string fileName, Stream stream)
        {
            var dirPath = GetStorageDirectory();
            var path = Path.Combine(dirPath, fileName);

            using (var fileStream = File.Create(path))
            {
                await stream.CopyToAsync(fileStream);
            }

            return path;
        }

        public async Task<string> SaveTextAsync(string fileName, string text)
        {
            var storagePath = GetStorageDirectory();
            var filePath = Path.Combine(storagePath, fileName);

            using (var sw = File.CreateText(filePath))
            {
                await sw.WriteAsync(text);
            }

            return filePath;
        }

        public async Task<string> LoadTextAsync(string fileName)
        {
            string text;
            
            var storagePath = GetStorageDirectory();
            var filePath = Path.Combine(storagePath, fileName);
            
            using (var sr = File.OpenText(filePath))
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
            var storagePath = GetStorageDirectory();
            var filePath = Path.Combine(storagePath, fileName);
            
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