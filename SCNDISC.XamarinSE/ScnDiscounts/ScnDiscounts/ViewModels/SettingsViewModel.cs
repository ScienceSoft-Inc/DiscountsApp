using Plugin.Connectivity;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models;
using ScnDiscounts.Views.ContentUI;
using ScnPage.Plugin.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ScnDiscounts.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private SettingsContentUI contentUI => (SettingsContentUI) ContentUI;

        public MainViewModel MainViewModel => (MainViewModel) App.RootPage.ViewModel;

        protected override void InitProperty()
        {
            _currLanguageName = LanguageHelper.LanguageList[AppParameters.Config.SystemLang];
        }

        public new string Title => contentUI.Title;

        public string CurrentLanguageTitle => contentUI.TxtLanguage;

        public string UpdateDbTitle => contentUI.TxtUpdateDb.ToUpper();

        private string _currLanguageName;

        public string CurrLanguageName
        {
            get => _currLanguageName;
            set
            {
                if (_currLanguageName != value)
                {
                    _currLanguageName = value;
                    OnPropertyChanged();

                    OnPropertyChanged(nameof(Title));
                    OnPropertyChanged(nameof(CurrentLanguageTitle));
                    OnPropertyChanged(nameof(UpdateDbTitle));
                    ProcessMessage = null;
                }
            }
        }

        private string _processMessage;

        public string ProcessMessage
        {
            get => _processMessage;
            set
            {
                if (_processMessage != value)
                {
                    _processMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public void LangSetting_Click(object sender, EventArgs e)
        {
            var view = (View) sender;
            view.ClickAnimation(async () =>
            {
                var list = new List<string>(LanguageHelper.LanguageList.Values);
                var lang = await ViewPage.DisplayActionSheet(contentUI.TxtLanguageSel, null, null, list.ToArray());

                if (!string.IsNullOrEmpty(lang))
                {
                    AppParameters.Config.SystemLang = LanguageHelper.LangNameToEnum(lang);
                    AppParameters.Config.SaveValues();

                    CurrLanguageName = lang;
                }
            });
        }

        public async void BtnUpdateDb_Clicked(object sender, EventArgs e)
        {
            IsLoading = true;

            var splashContentUI = new SplashContentUI();

            ProcessMessage = splashContentUI.TxtProcessCheckInternet;

            var isHasInternet = CrossConnectivity.Current.IsConnected;
            if (isHasInternet)
            {
                var isSuccess = await AppData.Discount.SyncData(OnProcessMessage);
                if (isSuccess)
                {
                    ProcessMessage = splashContentUI.TxtProcessLoadingData;
                    await Task.Delay(50);

                    AppData.Discount.Db.LoadData();

                    var mapPinCollection = AppData.Discount.MapPinCollection.ToList();
                    mapPinCollection.ForEach(i => i.CalculateDistance());

                    MainViewModel.InitFilterList();
                    MainViewModel.OnDataRefreshing();

                    ProcessMessage = contentUI.TxtProcessLoadingCompleted;
                }
                else
                {
                    await ViewPage.DisplayAlert(contentUI.TitleErrLoading, contentUI.TxtProcessLoadingError,
                        contentUI.TxtOk);

                    ProcessMessage = contentUI.TitleErrLoading;
                }
            }
            else
            {
                await ViewPage.DisplayAlert(splashContentUI.TitleErrInternet, splashContentUI.MsgErrInternet,
                    contentUI.TxtOk);

                ProcessMessage = splashContentUI.TxtRetryCheckInternet;
            }

            IsLoading = false;
        }

        private void OnProcessMessage(string message)
        {
            ProcessMessage = message;
        }
    }
}
