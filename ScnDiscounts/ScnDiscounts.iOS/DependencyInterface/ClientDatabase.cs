using System;
using Xamarin.Forms;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.iOS.DependencyInterface;
using UIKit;
using Foundation;
using System.IO;

[assembly: Dependency(typeof(ClientDatabase))]

namespace ScnDiscounts.iOS.DependencyInterface
{
    public class ClientDatabase : IClientDatabase
    {
        public string GetPath(string fileName)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);// Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            string dbPath = Path.Combine(libraryPath, "ScnDiscountsDB");

            if (!Directory.Exists(dbPath))
                Directory.CreateDirectory(dbPath);

            return Path.Combine(dbPath, fileName);
        }
    }
}