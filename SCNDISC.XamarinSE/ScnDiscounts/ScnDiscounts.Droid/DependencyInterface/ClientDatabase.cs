using ScnDiscounts.DependencyInterface;
using ScnDiscounts.Droid.DependencyInterface;
using SQLite;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(ClientDatabase))]

namespace ScnDiscounts.Droid.DependencyInterface
{
    public class ClientDatabase : IClientDatabase
    {
        public SQLiteConnection GetSQLiteConnection(string fileName)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, fileName);
            return new SQLiteConnection(path);
        }
    }
}