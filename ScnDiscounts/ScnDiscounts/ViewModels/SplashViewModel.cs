using System;
using ScnDiscounts.Control.Pages;
using ScnDiscounts.Models;
using ScnDiscounts.Views;
using ScnDiscounts.Views.ContentUI;
using Xamarin.Forms;

namespace ScnDiscounts.ViewModels
{
    class SplashViewModel : BaseViewModel
    {
        private SplashContentUI contentUI
        {
            get { return (SplashContentUI)ContentUI; }
        }
        
        protected override void InitProperty()
        {
            ProcessMessage = contentUI.TxtLoading;

            ViewPage.Appearing += ViewPage_AppearingLogo;

            CurrentApp.Suspending += CurrentApp_Suspending;

            MessagingCenter.Subscribe<SplashViewModel>(this, "StartMainForm", sender =>
            {
                CurrentApp.MainPage = new NavigationPage(new MainPage());
            });
        }

        void CurrentApp_Suspending(object sender, EventArgs e)
        {
            AppParameters.Config.SaveValue();
        }

         void ViewPage_AppearingLogo(object sender, EventArgs e)
        {
            ViewPage.Appearing -= ViewPage_AppearingLogo;

            AppParameters.Config.Init();
            InitApp();
        }

        async private void InitApp()
        {
            IsShowLoading = true;

            if (CheckInternet())
            {
                bool isLoadingSuccess = true;

                //Load Spatial
                ProcessMessage = contentUI.TxtProcessLoadMapData;
                isLoadingSuccess = await AppData.Discount.LoadSpatial();

                if (!isLoadingSuccess)
                {
                    ProcessMessage = contentUI.TitleErrLoading;
                    await ViewPage.DisplayAlert(contentUI.TitleErrLoading, contentUI.MsgErrLoading, contentUI.TxtOk);
                    ProcessMessage = contentUI.TxtErrServiceConnection;
                    IsRetry = true;
                    return;
                }

                //Load Discounts
                ProcessMessage = contentUI.TxtProcessLoadDiscountsData;
                isLoadingSuccess = await AppData.Discount.LoadDiscounts();
                if (!isLoadingSuccess)
                {
                    ProcessMessage = contentUI.TitleErrLoading;
                    await ViewPage.DisplayAlert(contentUI.TitleErrLoading, contentUI.MsgErrLoading, contentUI.TxtOk);
                    ProcessMessage = contentUI.TxtErrServiceConnection;
                    IsRetry = true;
                    return;
                }

                ProcessMessage = "";
                //await Task.Delay(3000);//for fun, i will del it
                MessagingCenter.Send(this, "StartMainForm");
            }

            IsShowLoading = false;
        }

        private bool CheckInternet()
        {
            ProcessMessage = contentUI.TxtProcessCheckInternet;

            var isHasInternet = AppMobileService.Network.IsAvailable();

            if (!isHasInternet)
            {
                ViewPage.DisplayAlert(contentUI.TitleErrInternet, contentUI.MsgErrInternet, contentUI.TxtOk);
                ProcessMessage = contentUI.TxtRertyCheckInternet;
                IsRetry = true;
            }

            return isHasInternet;
        }

        //------------------
        // Property
        //------------------

        #region ProcessMessage
        private string _processMessage = "";
        public string ProcessMessage
        {
            get { return _processMessage; }
            set
            {
                _processMessage = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region isShowLoading
        private bool _isShowLoading;
        public bool IsShowLoading
        {
            get { return _isShowLoading; }
            set
            {
                _isShowLoading = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region isRetry
        private bool _isRetry;
        public bool IsRetry
        {
            get { return _isRetry; }
            set
            {
                _isRetry = value;
                IsShowLoading = !_isRetry;
                BtnRetryText = contentUI.BtnTxtRerty;
                OnPropertyChanged();
            }
        }
        #endregion

        #region BtnRetryText
        private string _btnRetryText = "";
        public string BtnRetryText
        {
            get { return _btnRetryText; }
            set
            {
                _btnRetryText = value;
                OnPropertyChanged();
            }
        }
        #endregion

        //------------------
        // Command
        //------------------

        #region RetryCommand
        private Command retryCommand;
        public Command RetryCommand
        {
            get
            {
                return retryCommand ??
                    (retryCommand = new Command(ExecuteRetry));
            }
        }

        private void ExecuteRetry()
        {
            IsRetry = false;
            InitApp();
        }
        #endregion
    }
}
