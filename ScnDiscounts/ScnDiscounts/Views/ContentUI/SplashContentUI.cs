using Xamarin.Forms;
using ScnDiscounts.Helpers;
using ScnDiscounts.Models;

namespace ScnDiscounts.Views.ContentUI
{
    public class SplashContentUI : RootContentUI
    {
        public SplashContentUI()
        {
            title = new LanguageStrings("ScienceSoft", "ScienceSoft", "ScienceSoft");
            _titleErrInternet = new LanguageStrings("Internet is disabled", "Отсутствует интернет", "Aдсутнічае інтэрнэт");
            _msgErrInternet = new LanguageStrings("Check connection. Application can't function without the Internet.",
                                                "Проверьте подключение. Приложение не может работать без интернета.",
                                                "Праверце падлучэнне. Прыкладанне не можа працаваць без інтэрнэту.");
            _txtProcessCheckInternet = new LanguageStrings("Checking Internet available", "Проверка доступа в интернет", "Праверка доступу ў інтэрнэт");
            _txtProcessConnection = new LanguageStrings("Connection to server", "Подключение к серверу", "Падключэнне да сервера");
            _txtProcessLoadingData = new LanguageStrings("Loading data", "Загрузка данных", "Загрузка даных");
            _txtProcessLoadMapData = new LanguageStrings("Loading map data", "Загрузка информации для карты", "Загрузка інфармацыі для мапы");
            _txtProcessLoadDiscountsData = new LanguageStrings("Loading discounts data", "Загрузка информации о скидках", "Загрузка інфармацыі пра зніжкі");
            
            _titleErrLoading = new LanguageStrings("Error loading data", "Ошибка загрузки данных", "Памылка загрузкі інфармацыі");
            _msgErrLoading = new LanguageStrings("Try to launch application again. If the error appears, contact the developers.",
                                              "Попробуйте запустить приложение снова. Если ошибка появится, то сообщите об этом разработчикам.",
                                              "Паспрабуйце запусціць прыкладанне зноў. Калі памылка з'явіцца, то паведаміце пра гэта распрацоўнікам.");
            _btnTxtRerty = new LanguageStrings("Retry again", "Попробовать еще раз", "Паспрабаваць яшчэ раз");
            _txtRertyCheckInternet = new LanguageStrings("Check internet connection", "Проверьте подключение к интернету", "Праверце падлучэнне да інтэрнэту");
            _txtErrServiceConnection = new LanguageStrings("Error connection to service", "Ошибка соединения с сервисом", "Памылка злучэння з сэрвісам");
        }

        public string ImgLogo
        {
            get { return Device.OnPlatform("Image/img_logo.png", "img_logo.png", "Assets/Image/img_logo.png"); }
        }

        public string ImgBackground
        {
            get { return Device.OnPlatform("Image/Splash.png", "img_background.png", "Assets/Image/img_background.png"); }
        }

        private LanguageStrings _titleErrInternet;
        public string TitleErrInternet
        {
            get { return _titleErrInternet.Current; }
        }

        private LanguageStrings _msgErrInternet;
        public string MsgErrInternet
        {
            get { return _msgErrInternet.Current; }
        }

        private LanguageStrings _txtProcessCheckInternet;
        public string TxtProcessCheckInternet
        {
            get { return _txtProcessCheckInternet.Current; }
        }

        private LanguageStrings _txtProcessConnection;
        public string TxtProcessConnection
        {
            get { return _txtProcessConnection.Current; }
        }

        private LanguageStrings _txtProcessLoadingData;
        public string TxtProcessLoadingData
        {
            get { return _txtProcessLoadingData.Current; }
        }

        private LanguageStrings _txtProcessLoadMapData;
        public string TxtProcessLoadMapData
        {
            get { return _txtProcessLoadMapData.Current; }
        }

        private LanguageStrings _txtProcessLoadDiscountsData;
        public string TxtProcessLoadDiscountsData
        {
            get { return _txtProcessLoadDiscountsData.Current; }
        }

        private LanguageStrings _titleErrLoading;
        public string TitleErrLoading
        {
            get { return _titleErrLoading.Current; }
        }

        private LanguageStrings _msgErrLoading;
        public string MsgErrLoading
        {
            get { return _msgErrLoading.Current; }
        }

        private LanguageStrings _btnTxtRerty;
        public string BtnTxtRerty
        {
            get { return _btnTxtRerty.Current; }
        }

        private LanguageStrings _txtRertyCheckInternet;
        public string TxtRertyCheckInternet
        {
            get { return _txtRertyCheckInternet.Current; }
        }

        private LanguageStrings _txtErrServiceConnection;
        public string TxtErrServiceConnection
        {
            get { return _txtErrServiceConnection.Current; }
        }
    }
}
