using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

using ScnDiscounts.DependencyInterface;
using ScnDiscounts.iOS.DependencyInterface;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.IO;

[assembly: Dependency(typeof(LocalStorage))]
namespace ScnDiscounts.iOS.DependencyInterface
{
    public class LocalStorage : ILocalStorage
    {
        private string GetStorageDirectory()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);// Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            string storagePath = Path.Combine(libraryPath, "ScnDiscountsStorage");

            if (!Directory.Exists(storagePath))
                Directory.CreateDirectory(storagePath);

            return storagePath;
        }

        public async Task<string> SaveBase64Async(string fileName, string sreamFile)
        {
            byte[] fileBytes = System.Convert.FromBase64String(sreamFile);

            string storagePath = GetStorageDirectory();
            var filePath = Path.Combine(storagePath, fileName);

            using (var stream = File.Create(filePath))
            {
                await stream.WriteAsync(fileBytes, 0, fileBytes.Length);
            }

            return filePath;
        }

        public async Task<string> SaveTextAsync(string fileName, string text)
        {
            string storagePath = GetStorageDirectory();
            var filePath = Path.Combine(storagePath, fileName);

            using (StreamWriter sw = File.CreateText(filePath))
            {
                await sw.WriteAsync(text);
            }

            return filePath;
        }

        public async Task<string> LoadTextAsync(string fileName)
        {
            string text = string.Empty;
            
            string storagePath = GetStorageDirectory();
            var filePath = Path.Combine(storagePath, fileName);
            
            using (StreamReader sr = File.OpenText(filePath))
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
            string storagePath = GetStorageDirectory();
            var filePath = Path.Combine(storagePath, fileName);
            
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