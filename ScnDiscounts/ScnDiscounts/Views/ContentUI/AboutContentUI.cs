using System.Reflection;
using ScnDiscounts.Helpers;
using Xamarin.Forms;

namespace ScnDiscounts.Views.ContentUI
{
    public class AboutContentUI : RootContentUI
    {
        public AboutContentUI()
        {
            //TxtVersionValue = "1.0.0.0";
            TxtVersionValue = (new AssemblyName(Assembly.GetExecutingAssembly().FullName)).Version.ToString();
            TxtPhoneValue = "+375(17)293 37 36";
            TxtEmailValue = "contact@scnsoft.com";
            TxtHttpValue = @"http://www.scnsoft.com";

            _title = new PropertyLang("About", "О программе", "Аб праграме");
            _titleVersion = new PropertyLang("Version", "Версия");
            _titleDeveloper = new PropertyLang("Developer", "Разработчик");
            _txtPhone = new PropertyLang("Phone", "Тел.");
            _txtEmail = new PropertyLang("Email", "Эл.ящик");
            _txtDescription = new PropertyLang(
                "The app is a demo of Xamarin.Forms cross platform mobile brought by ScienceSoft developer. As exemplified with discounts apps (listing and plotting map discounts locations for ScienceSoft employees) one can see common UI structure and controls utilized across platforms. The app is built for 3 targets (Android, iOS, Windows Phone) and reuses 85% of C# code (~2000 SLOC out of 2400).\n\n" +
                    "Though tricky and non-trivial UI scenarios and platform specific capabilities may require creating platform specific code we can testify the current state of affairs with Xamarin.Forms:",
                "Данное приложение является демонстрацией кросс платформенной разработки с использованием Xamarin.Forms выполненная разработчиками ScienceSoft. На примере скидочного приложения (хранящего список и отображающего на карте организации со скидками для сотрудников ScienceSoft) можно увидеть использование на различных платформах общей структуры пользовательского интерфейса и элементов управления. Приложения скомпилировано для 3 платформ (Android, iOS, Windows Phone) и повторно использует 85% строк кода на C# (~2000 SLOC из 2400).\n\n" +
                    "Реализация не тривиальных UX сценариев и использование платформ-специфичных возможностей может потребовать создания платформ-специфичного кода.\n" + 
                    "Однако исходя из текущего положения вещей с Xamarin.Froms мы можем заявить, что:");
            TxtDescriptionBulletSymbol = "\u2022";
            _txtDescriptionBullet1 = new PropertyLang(
                "It abstracts much of a hard work;",
                "Платформа абстрагирует от большой порции рутинной работы;");
            _txtDescriptionBullet2 = new PropertyLang(
                "Gives rich customization capabilities to implement UI cases (and avoid using custom \"renders\");",
                "Дает богатые возможности по настройке внешнего вида приложения (убирая необходимость разрабатывать собственные \"рендеры\");");
            _txtDescriptionBullet3 = new PropertyLang(
                "Perfectly fits data driven and enterprise application development saving effort and time.",
                "Замечательно подходит для разработки корпоративных приложений, и приложений для работы с данными и формами (сохраняя большое количество времени и усилий).");
            TxtDescriptionLink = @"https://github.com/ScienceSoft-Inc/XamarinDiscountsApp";
        }

        public string Icon
        {
            get { return Device.OnPlatform("Icon/about.png", "ic_menu_about.png", "Assets/Icon/about.png"); }
        }

        public string ImgLogo
        {
            get { return Device.OnPlatform("Image/img_logo.png", "img_logo.png", "Assets/Image/img_logo.png"); }
        }

        private PropertyLang _titleVersion;
        public string TitleVersion
        {
            get { return _titleVersion.ActualValue(); }
        }
        
        public string TxtVersionValue { get; private set; }

        private PropertyLang _titleDeveloper;
        public string TitleDeveloper
        {
            get { return _titleDeveloper.ActualValue(); }
        }

        private PropertyLang _txtPhone;
        public string TxtPhone
        {
            get { return _txtPhone.ActualValue(); }
        }
        
        public string TxtPhoneValue { get; private set; }

        private PropertyLang _txtEmail;
        public string TxtEmail
        {
            get { return _txtEmail.ActualValue(); }
        }
        
        public string TxtEmailValue { get; private set; }

        public string TxtHttpValue { get; private set; }

        private PropertyLang _txtDescription;
        public string TxtDescription
        {
            get { return _txtDescription.ActualValue(); }
        }

        public string TxtDescriptionBulletSymbol { get; private set; }

        private PropertyLang _txtDescriptionBullet1;
        public string TxtDescriptionBullet1
        {
            get { return _txtDescriptionBullet1.ActualValue(); }
        }

        private PropertyLang _txtDescriptionBullet2;
        public string TxtDescriptionBullet2
        {
            get { return _txtDescriptionBullet2.ActualValue(); }
        }

        private PropertyLang _txtDescriptionBullet3;
        public string TxtDescriptionBullet3
        {
            get { return _txtDescriptionBullet3.ActualValue(); }
        }

        public string TxtDescriptionLink { get; private set; }
    }
}
