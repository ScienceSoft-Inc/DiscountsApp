using ScnDiscounts.Helpers;
using Xamarin.Forms;

namespace ScnDiscounts.Views.ContentUI
{
    public class SplashContentUI : RootContentUI
    {
        public SplashContentUI()
        {
            _title = new PropertyLang("ScienceSoft", "ScienceSoft", "ScienceSoft");
            _titleErrInternet = new PropertyLang("Internet is disabled", "Отсутствует интернет", "Aдсутнічае інтэрнэт");
            _msgErrInternet = new PropertyLang("Check connection. Application can't function without the Internet.",
                                                "Проверьте подключение. Приложение не может работать без интернета.",
                                                "Праверце падлучэнне. Прыкладанне не можа працаваць без інтэрнэту.");
            _txtProcessCheckInternet = new PropertyLang("Checking Internet available", "Проверка доступа в интернет", "Праверка доступу ў інтэрнэт");
            _txtProcessConnection = new PropertyLang("Connection to server", "Подключение к серверу", "Падключэнне да сервера");
            _txtProcessLoadMapData = new PropertyLang("Loading map data", "Загрузка информации для карты", "Загрузка інфармацыі для мапы");
            _txtProcessLoadDiscountsData = new PropertyLang("Loading discounts data", "Загрузка информации о скидках", "Загрузка інфармацыі пра зніжкі");
            
            _titleErrLoading = new PropertyLang("Error loading data", "Ошибка загрузки данных", "Памылка загрузкі інфармацыі");
            _msgErrLoading = new PropertyLang("Try to launch application again. If the error appears, contact the developers.",
                                              "Попробуйте запустить приложение снова. Если ошибка появится, то сообщите об этом разработчикам.",
                                              "Паспрабуйце запусціць прыкладанне зноў. Калі памылка з'явіцца, то паведаміце пра гэта распрацоўнікам.");
            _btnTxtRerty = new PropertyLang("Retry again", "Попробовать еще раз", "Паспрабаваць яшчэ раз");
            _txtRertyCheckInternet = new PropertyLang("Check internet connection", "Проверьте подключение к интернету", "Праверце падлучэнне да інтэрнэту");
            _txtErrServiceConnection = new PropertyLang("Error connection to service", "Ошибка соединения с сервисом", "Памылка злучэння з сэрвісам");
        }

        public string ImgLogo
        {
            get { return Device.OnPlatform("Image/img_logo.png", "img_logo.png", "Assets/Image/img_logo.png"); }
        }

        public string ImgBackground
        {
            get { return Device.OnPlatform("Image/Splash.png", "img_background.png", "Assets/Image/img_background.png"); }
        }

        private PropertyLang _titleErrInternet;
        public string TitleErrInternet
        {
            get { return _titleErrInternet.ActualValue(); }
        }

        private PropertyLang _msgErrInternet;
        public string MsgErrInternet
        {
            get { return _msgErrInternet.ActualValue(); }
        }

        private PropertyLang _txtProcessCheckInternet;
        public string TxtProcessCheckInternet
        {
            get { return _txtProcessCheckInternet.ActualValue(); }
        }

        private PropertyLang _txtProcessConnection;
        public string TxtProcessConnection
        {
            get { return _txtProcessConnection.ActualValue(); }
        }

        private PropertyLang _txtProcessLoadMapData;
        public string TxtProcessLoadMapData
        {
            get { return _txtProcessLoadMapData.ActualValue(); }
        }

        private PropertyLang _txtProcessLoadDiscountsData;
        public string TxtProcessLoadDiscountsData
        {
            get { return _txtProcessLoadDiscountsData.ActualValue(); }
        }

        private PropertyLang _titleErrLoading;
        public string TitleErrLoading
        {
            get { return _titleErrLoading.ActualValue(); }
        }

        private PropertyLang _msgErrLoading;
        public string MsgErrLoading
        {
            get { return _msgErrLoading.ActualValue(); }
        }

        private PropertyLang _btnTxtRerty;
        public string BtnTxtRerty
        {
            get { return _btnTxtRerty.ActualValue(); }
        }

        private PropertyLang _txtRertyCheckInternet;
        public string TxtRertyCheckInternet
        {
            get { return _txtRertyCheckInternet.ActualValue(); }
        }

        private PropertyLang _txtErrServiceConnection;
        public string TxtErrServiceConnection
        {
            get { return _txtErrServiceConnection.ActualValue(); }
        }
    }
}
