using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace ScnDiscounts.WinPhone
{
    public partial class MainPage : global::Xamarin.Forms.Platform.WinPhone.FormsApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            SupportedOrientations = SupportedPageOrientation.Portrait;

            global::Xamarin.Forms.Forms.Init();

            //TODO: when
            //string applicationId = "APP_ID_FROM_PORTAL", authToken = "AUTH_TOKEN_FROM_PORTAL";
            //FormsMaps.Init(applicationId, authToken);
            global::Xamarin.FormsMaps.Init();

            LoadApplication(new ScnDiscounts.App());
        }
    }
}
