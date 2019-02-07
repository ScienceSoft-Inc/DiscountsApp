using System.IO;
using System.Threading.Tasks;

namespace ScnDiscounts.DependencyInterface
{
    public interface ILocalStorage
    {
        //save base64 to file
        Task<string> SaveBase64Async(string fileName, string base64);

        //save stream to file
        Task<string> SaveStreamAsync(string fileName, Stream stream);

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
