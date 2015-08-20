using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScnDiscounts.DependencyInterface
{
    public interface ILocalStorage
    {
        //save base64 to file
        Task<string> SaveBase64Async(string fileName, string sreamFile);

        //save text to file
        Task<string> SaveTextAsync(string fileName, string text);

        //read text from file
        Task<string> LoadTextAsync(string fileName);

        //check exist file
        bool FileExist(string filePath);

        //full path on file
        string GetFilePath(string fileName);

        //delete file
        void DeleteFile(string fileName);

        void ClearStorage(); //use StorageDirectory
    }
}
