using Plugin.Connectivity;
using ScnDiscounts.Models;
using ScnDiscounts.Views;
using ScnDiscounts.Views.ContentUI;
using ScnPage.Plugin.Forms;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ScnDiscounts.ViewModels
{
    public class SplashViewModel : BaseViewModel
    {
        private SplashContentUI contentUI => (SplashContentUI)ContentUI;

        private static Page MainPage => Application.Current.MainPage;

        protected override void InitProperty()
        {
            ViewPage.Appearing += ViewPage_InitAfterAppearing;

            AppParameters.Config.LoadValues();

            ProcessMessage = contentUI.TxtLoading;
        }

        private void ViewPage_InitAfterAppearing(object sender, EventArgs eventArgs)
        {
            ViewPage.Appearing -= ViewPage_InitAfterAppearing;

            InitApp();
        }

        private async void InitApp()
        {
            IsShowLoading = true;

            if (await CheckInternet())
            {
                if (!await AppData.Discount.SyncData(OnProcessMessage))
                {
                    ProcessMessage = contentUI.TitleErrLoading;
                    await MainPage.DisplayAlert(contentUI.TitleErrLoading, contentUI.MsgErrLoading, contentUI.TxtOk);

                    IsRetry = true;
                }
                else
                    await LoadApp();
            }

            IsShowLoading = false;
        }

        private async Task LoadApp()
        {
            IsShowLoading = true;

            ProcessMessage = contentUI.TxtProcessLoadingData;
            await Task.Yield();

            AppData.Discount.Db.LoadData();

            await Application.Current.MainPage.Navigation.PushAsync(new MainPage());
            Application.Current.MainPage.Navigation.RemovePage(ViewPage);

            IsShowLoading = false;
        }

        private void OnProcessMessage(string message)
        {
            ProcessMessage = message;
        }

        private async Task<bool> CheckInternet()
        {
            ProcessMessage = contentUI.TxtProcessCheckInternet;

            var isHasInternet = CrossConnectivity.Current.IsConnected;
            if (!isHasInternet)
            {
                await MainPage.DisplayAlert(contentUI.TitleErrInternet, contentUI.MsgErrInternet, contentUI.TxtOk);
                ProcessMessage = contentUI.TxtRetryCheckInternet;

                IsRetry = true;
            }

            return isHasInternet;
        }

        #region ProcessMessage
        private string _processMessage;
        public string ProcessMessage
        {
            get => _processMessage;
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
            get => _isShowLoading;
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
            get => _isRetry;
            set
            {
                _isRetry = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region BtnRetryText
        private string _btnRetryText;
        public string BtnRetryText
        {
            get => _btnRetryText;
            set
            {
                _btnRetryText = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region RetryCommand
        private Command _retryCommand;
        public Command RetryCommand => _retryCommand ?? (_retryCommand = new Command(ExecuteRetry));

        private void ExecuteRetry()
        {
            IsRetry = false;

            InitApp();
        }
        #endregion

        #region SkipCommand
        private Command _skipCommand;
        public Command SkipCommand => _skipCommand ?? (_skipCommand = new Command(ExecuteSkip));

        private async void ExecuteSkip()
        {
            IsRetry = false;

            await LoadApp();
        }
        #endregion
    }
}
