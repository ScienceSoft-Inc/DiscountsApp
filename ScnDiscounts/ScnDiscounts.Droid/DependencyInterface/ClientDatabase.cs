using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ScnDiscounts.DependencyInterface;
using ScnDiscounts.Droid.DependencyInterface;
using Xamarin.Forms;
using System.IO;

[assembly: Dependency(typeof(ClientDatabase))]

namespace ScnDiscounts.Droid.DependencyInterface
{
    public class ClientDatabase : IClientDatabase
    {
        public string GetPath(string fileName)
        {
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            return Path.Combine(documentsPath, fileName);
        }
    }
}