using System;
using System.IO;
using Xamarin.Forms;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.WinPhone.DependencyInterface;

[assembly: Dependency(typeof(ClientDatabase))]

namespace ScnDiscounts.WinPhone.DependencyInterface
{
    public class ClientDatabase : IClientDatabase
    {
        public string GetPath(string fileName)
        {
            return Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, fileName);
        }
    }
}
