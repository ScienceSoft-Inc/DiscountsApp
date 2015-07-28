using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ScnViewGestures.Plugin.Forms.WinPhone.Renderers;

namespace ScnDiscounts.WinPhone
{
    public partial class MainPage : global::Xamarin.Forms.Platform.WinPhone.FormsApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            SupportedOrientations = SupportedPageOrientation.Portrait;

            global::Xamarin.Forms.Forms.Init();
            ViewGesturesRenderer.Init();

            string applicationId = "deede3e0-a6c7-4a26-84af-b6042b1ca302", authToken = "YVSma2K-u4Ctrd4IPdNvGw";
            global::Xamarin.FormsMaps.Init(applicationId, authToken);

            LoadApplication(new ScnDiscounts.App());
        }
    }
}
