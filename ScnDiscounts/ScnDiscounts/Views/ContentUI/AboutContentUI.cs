using System.Reflection;
using ScnDiscounts.Helpers;
using Xamarin.Forms;
using ScnDiscounts.Models;

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

            title = new LanguageStrings("About", "О программе", "Аб праграме");
            _titleVersion = new LanguageStrings("Version", "Версия");
            _titleDeveloper = new LanguageStrings("Developer", "Разработчик");
            _txtPhone = new LanguageStrings("Phone", "Тел.");
            _txtEmail = new LanguageStrings("Email", "Эл.ящик");

            //It's important:
            //- apple rejects application which contain some  word like "demo", "test", "prereliase" etc.;
            //- apple rejects application which contain names other platforms.
            if (Device.OS == TargetPlatform.iOS)
            {
                _txtDescription = new LanguageStrings(
                    "This is a Xamarin.Forms mobile app brought by ScienceSoft developers. In the discounts application (listing and plotting on a map discounts locations for ScienceSoft employees) one can see common UI structure and controls. The app reuses 85% of C# code (~2000 SLOC out of 2400).\n\n" +
                        "Though tricky and non-trivial UI scenarios and platform specific capabilities may require creating platform specific code we can testify the current state of affairs with Xamarin.Forms:",
                    "Данное приложение разработано с использованием Xamarin.Forms и выполненно разработчиками ScienceSoft. В скидочном приложении (хранящем список и отображающим на карте организации со скидками для сотрудников ScienceSoft) можно увидеть использование на различных платформах общей структуры пользовательского интерфейса и элементов управления. Приложения повторно использует 85% строк кода на C# (~2000 SLOC из 2400).\n\n" +
                        "Реализация нетривиальных UX сценариев и использование платформ-специфичных возможностей может потребовать создания платформ-специфичного кода.\n" +
                        "Однако исходя из текущего положения вещей с Xamarin.Froms мы можем заявить, что:");
                _txtDescriptionBullet1 = new LanguageStrings(
                    "It abstracts away much of a hard work;",
                    "Платформа абстрагирует от большой порции рутинной работы;");
                _txtDescriptionBullet2 = new LanguageStrings(
                    "Gives rich customization capabilities to implement UI cases (and avoid using custom \"renders\");",
                    "Дает богатые возможности по настройке внешнего вида приложения (убирая необходимость разрабатывать собственные \"рендеры\");");
                _txtDescriptionBullet3 = new LanguageStrings(
                    "Perfectly fits data driven and enterprise application development saving effort and time.",
                    "Замечательно подходит для разработки корпоративных приложений, и приложений для работы с данными и формами (сохраняя большое количество времени и усилий).");
            }
            else
            {
                _txtDescription = new LanguageStrings(
                    "The app is a demo of Xamarin.Forms cross platform mobile development brought by ScienceSoft. As exemplified with discounts apps (listing and plotting on a map discounts locations for ScienceSoft employees) one can see common UI structure and controls utilized across platforms. The app is built for 3 targets (Android, iOS, Windows Phone) and reuses 85% of C# code (~2000 SLOC out of 2400).\n\n" +
                        "Though tricky and non-trivial UI scenarios and platform specific capabilities may require creating platform specific code we can testify the current state of affairs with Xamarin.Forms:",
                    "Данное приложение является демонстрацией кросс платформенной разработки с использованием Xamarin.Forms выполненная разработчиками ScienceSoft. На примере скидочного приложения (хранящего список и отображающего на карте организации со скидками для сотрудников ScienceSoft) можно увидеть использование на различных платформах общей структуры пользовательского интерфейса и элементов управления. Приложения скомпилировано для 3 платформ (Android, iOS, Windows Phone) и повторно использует 85% строк кода на C# (~2000 SLOC из 2400).\n\n" +
                        "Реализация нетривиальных UX сценариев и использование платформ-специфичных возможностей может потребовать создания платформ-специфичного кода.\n" +
                        "Однако исходя из текущего положения вещей с Xamarin.Froms мы можем заявить, что:");
                _txtDescriptionBullet1 = new LanguageStrings(
                    "It abstracts much of a hard work;",
                    "Платформа абстрагирует от большой порции рутинной работы;");
                _txtDescriptionBullet2 = new LanguageStrings(
                    "Gives rich customization capabilities to implement UI cases (and avoid using custom \"renders\");",
                    "Дает богатые возможности по настройке внешнего вида приложения (убирая необходимость разрабатывать собственные \"рендеры\");");
                _txtDescriptionBullet3 = new LanguageStrings(
                    "Perfectly fits data driven and enterprise application development saving effort and time.",
                    "Замечательно подходит для разработки корпоративных приложений, и приложений для работы с данными и формами (сохраняя большое количество времени и усилий).");
            }

            TxtDescriptionBulletSymbol = "\u2022";
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

        private LanguageStrings _titleVersion;
        public string TitleVersion
        {
            get { return _titleVersion.Current; }
        }
        
        public string TxtVersionValue { get; private set; }

        private LanguageStrings _titleDeveloper;
        public string TitleDeveloper
        {
            get { return _titleDeveloper.Current; }
        }

        private LanguageStrings _txtPhone;
        public string TxtPhone
        {
            get { return _txtPhone.Current; }
        }
        
        public string TxtPhoneValue { get; private set; }

        private LanguageStrings _txtEmail;
        public string TxtEmail
        {
            get { return _txtEmail.Current; }
        }
        
        public string TxtEmailValue { get; private set; }

        public string TxtHttpValue { get; private set; }

        private LanguageStrings _txtDescription;
        public string TxtDescription
        {
            get { return _txtDescription.Current; }
        }

        public string TxtDescriptionBulletSymbol { get; private set; }

        private LanguageStrings _txtDescriptionBullet1;
        public string TxtDescriptionBullet1
        {
            get { return _txtDescriptionBullet1.Current; }
        }

        private LanguageStrings _txtDescriptionBullet2;
        public string TxtDescriptionBullet2
        {
            get { return _txtDescriptionBullet2.Current; }
        }

        private LanguageStrings _txtDescriptionBullet3;
        public string TxtDescriptionBullet3
        {
            get { return _txtDescriptionBullet3.Current; }
        }

        public string TxtDescriptionLink { get; private set; }
    }
}
