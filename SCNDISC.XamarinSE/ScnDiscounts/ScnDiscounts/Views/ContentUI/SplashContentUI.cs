using ScnDiscounts.Models;

namespace ScnDiscounts.Views.ContentUI
{
    public class SplashContentUI : RootContentUI
    {
        public SplashContentUI()
        {
            title = new LanguageStrings("ScienceSoft", "ScienceSoft");
            _titleErrInternet = new LanguageStrings("Internet is disabled", "Отсутствует интернет");
            _msgErrInternet = new LanguageStrings("Check connection. Application can't function without the Internet.",
                                                "Проверьте подключение. Приложение не может работать без интернета.");
            _txtProcessCheckInternet = new LanguageStrings("Checking Internet available", "Проверка доступа в интернет");
            _txtProcessLoadingData = new LanguageStrings("Loading local data", "Загрузка локальных данных");
            _txtProcessLoadMapData = new LanguageStrings("Loading map data", "Загрузка информации для карты");
            _txtProcessLoadDiscountsData = new LanguageStrings("Loading discounts data", "Загрузка информации о скидках");
            _titleErrLoading = new LanguageStrings("Error loading data", "Ошибка загрузки данных");
            _msgErrLoading = new LanguageStrings("Try to launch application again. If the error appears, contact the developers.",
                                              "Попробуйте запустить приложение снова. Если ошибка появится, то сообщите об этом разработчикам.");
            _btnTxtRetry = new LanguageStrings("Retry again", "Попробовать еще раз");
            _btnTxtSkip = new LanguageStrings("Skip", "Пропустить");
            _txtRetryCheckInternet = new LanguageStrings("Check internet connection", "Проверьте подключение к интернету");
        }

        private readonly LanguageStrings _titleErrInternet;
        public string TitleErrInternet => _titleErrInternet.Current;

        private readonly LanguageStrings _msgErrInternet;
        public string MsgErrInternet => _msgErrInternet.Current;

        private readonly LanguageStrings _txtProcessCheckInternet;
        public string TxtProcessCheckInternet => _txtProcessCheckInternet.Current;

        private readonly LanguageStrings _txtProcessLoadingData;
        public string TxtProcessLoadingData => _txtProcessLoadingData.Current;

        private readonly LanguageStrings _txtProcessLoadMapData;
        public string TxtProcessLoadMapData => _txtProcessLoadMapData.Current;

        private readonly LanguageStrings _txtProcessLoadDiscountsData;
        public string TxtProcessLoadDiscountsData => _txtProcessLoadDiscountsData.Current;

        private readonly LanguageStrings _titleErrLoading;
        public string TitleErrLoading => _titleErrLoading.Current;

        private readonly LanguageStrings _msgErrLoading;
        public string MsgErrLoading => _msgErrLoading.Current;

        private readonly LanguageStrings _btnTxtRetry;
        public string BtnTxtRetry => _btnTxtRetry.Current;

        private readonly LanguageStrings _btnTxtSkip;
        public string BtnTxtSkip => _btnTxtSkip.Current;

        private readonly LanguageStrings _txtRetryCheckInternet;
        public string TxtRetryCheckInternet => _txtRetryCheckInternet.Current;
    }
}
