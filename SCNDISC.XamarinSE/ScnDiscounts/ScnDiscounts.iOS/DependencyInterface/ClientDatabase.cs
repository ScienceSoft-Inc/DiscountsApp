using ScnDiscounts.DependencyInterface;
using ScnDiscounts.iOS.DependencyInterface;
using SQLite;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(ClientDatabase))]

namespace ScnDiscounts.iOS.DependencyInterface
{
    public class ClientDatabase : IClientDatabase
    {
        public SQLiteConnection GetSQLiteConnection(string fileName)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(documentsPath, "..", "Library");
            var dbPath = Path.Combine(libraryPath, "ScnDiscountsDB");

            if (!Directory.Exists(dbPath))
                Directory.CreateDirectory(dbPath);

            var path = Path.Combine(dbPath, fileName);
            return new SQLiteConnection(path);
        }
    }
}