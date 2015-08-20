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
            _titleVersion = new LanguageStrings("Version", "Версия", "Версія");
            _titleDeveloper = new LanguageStrings("Developer", "Разработчик", "Распрацоўшчык");
            _txtPhone = new LanguageStrings("Phone", "Тел.", "Тэл.");
            _txtEmail = new LanguageStrings("Email", "Эл.ящик");

            //It's important:
            //- apple rejects application which contain some  word like "demo", "test", "prereliase" etc.;
            //- apple rejects application which contain names other platforms.
            if (Device.OS == TargetPlatform.iOS)
            {
                _txtDescription = new LanguageStrings(
                    "This discount kit is available for ScienceSoft employees only. To get the discount you need to show company’s official pass in case of personal appeal or to say the keyword in case of phone call. One can find keywords in the description page of each organization that provides discounts for ScienceSoft.\n\n" +
                        "This is a Xamarin.Forms mobile app brought by ScienceSoft. In discount application one can see common UI structure and controls. Application lists and plots on a map discount locations for ScienceSoft employees. The app reuses 85% of C# code (~2000 SLOC out of 2400).\n\n" +
                        "Though tricky and non-trivial UI scenarios and platform specific capabilities may require creating platform specific code we can testify the current state of affairs with Xamarin.Forms:",
                    "Данным набором скидок могут пользоваться исключительно сотрудники компании ScienceSoft. Для получения скидки нужно предъявить бейдж сотрудника компании в случае личного обращения и назвать кодовое слова в случае обращения по телефону. Кодовые слова находятся в разделе описания организаций, предоставляющих скидки.\n\n" +
                        "Данное приложение разработано с использованием Xamarin.Forms и выполненно разработчиками ScienceSoft. В скидочном приложении, хранящем список и отображающим на карте организации со скидками для сотрудников ScienceSoft, можно наблюдать работу общей структуры пользовательского интерфейса и элементов управления на различных платформах. Приложения повторно использует 85% строк кода на C# (~2000 SLOC из 2400).\n\n" +
                        "Реализация нетривиальных UX сценариев и использование платформ-специфичных возможностей может потребовать создания платформ-специфичного кода.\n" +
                        "Однако исходя из текущего положения вещей с Xamarin.Froms мы можем заявить, что платформа:",
                    "Дадзеным наборам зніжак могуць карыстацца выключна супрацоўнікі кампаніі ScienceSoft. Для таго, каб атрымаць зніжку, вам неабходна паказаць бэйдж супрацоўніка кампаніі ў выпадку асабістага звароту і назваць кодавае слова ў выпадку звароту па тэлефоне. Кодавыя словы знаходзяцца ў раздзеле апісання арганізацый, якія прадастаўляюць зніжкі.\n\n" +
                        "Дадзенае прылажэнне распрацавалі супрацоўнікі кампаніі ScienceSoft з выкарыстаннем Xamarin.Froms. У зніжкавым прылажэнні, якое ўтрымлівае спіс і адлюстроўвае на мапе арганізацыі са зніжкамі для супрацоўнікаў ScienceSoft, можна назіраць выкарыстанне на розных платформах агульнай структуры карыстальніцкага інтэрфейсу і элементаў кіравання. Прылажэнне паўторна выкарыстоўвае 85% радкоў коду на C# (~2000 SLOC з 2400).\n\n" +
                        "Рэалізацыя нетрывіяльных UX-сцэнароў і выкарыстанне платформ-спецыфічных магчымасцяў можа патрабаваць стварэння платформ-спецыфічнага коду.\n" +
                        "Аднак згодна з бягучым станам рэчаў з Xamarin.Forms мы можам заявіць, што платформа:");
                _txtDescriptionBullet1 = new LanguageStrings(
                    "The platform abstracts much of a hard work;",
                    "избавляет от большой порции рутинной работы;",
                    "пазбаўляе ад вялікага аб’ёму руціннай працы;");
                _txtDescriptionBullet2 = new LanguageStrings(
                    "The platform gives rich customization capabilities to implement UI cases and avoid using custom \"renders\";",
                    "дает богатые возможности по настройке внешнего вида приложения, исключая необходимость разрабатывать собственные \"рендеры\";",
                    "дае багатыя магчымасці па наладцы вонкавага выгляду прылажэння, пазбаўляючы неабходнасці распрацоўваць уласныя \"рэндары\";");
                _txtDescriptionBullet3 = new LanguageStrings(
                    "The platform perfectly fits data driven and enterprise application development saving effort and time.",
                    "отлично подходит для разработки корпоративных приложений и приложений для работы с данными и формами, сохраняя большое количество времени и усилий.",
                    "выдатна пасуе для распрацоўкі карпаратыўных прылажэнняў і прылажэнняў для працы з дадзенымі і формамі, ашчаджаючы вялікую колькасць часу і намаганняў.");
            }
            else
            {
                _txtDescription = new LanguageStrings(
                    "This discount kit is available for ScienceSoft employees only. To get the discount you need to show company’s official pass in case of personal appeal or to say the keyword in case of phone call. One can find keywords in the description page of each organization that provides discounts for ScienceSoft.\n\n" +
                        "The app is a demo of Xamarin.Forms cross platform mobile development brought by ScienceSoft. As exemplified with discounts apps (listing and plotting on a map discounts locations for ScienceSoft employees) one can see common UI structure and controls utilized across platforms. The app is built for 3 targets (Android, iOS, Windows Phone) and reuses 85% of C# code (~2000 SLOC out of 2400).\n\n" +
                        "Though tricky and non-trivial UI scenarios and platform specific capabilities may require creating platform specific code we can testify the current state of affairs with Xamarin.Forms:",
                    "Данным набором скидок могут пользоваться исключительно сотрудники компании ScienceSoft. Для получения скидки нужно предъявить бейдж сотрудника компании в случае личного обращения и назвать кодовое слова в случае обращения по телефону. Кодовые слова находятся в разделе описания организаций, предоставляющих скидки.\n\n" +
                        "Данное приложение является демонстрацией кроссплатформенной разработки с использованием Xamarin.Forms и выполнено разработчиками ScienceSoft. На примере скидочного приложения, хранящего список и отображающего на карте организации со скидками для сотрудников ScienceSoft, можно увидеть использование на различных платформах общей структуры пользовательского интерфейса и элементов управления. Приложение скомпилировано для 3 платформ – Android, iOS, Windows Phone – и повторно использует 85% строк кода на C# (~2000 SLOC из 2400).\n\n" +
                        "Реализация нетривиальных UX сценариев и использование платформ-специфичных возможностей может потребовать создания платформ-специфичного кода.\n" +
                        "Однако исходя из текущего положения вещей с Xamarin.Froms мы можем заявить, что платформа:",
                    "Дадзеным наборам зніжак могуць карыстацца выключна супрацоўнікі кампаніі ScienceSoft. Для таго, каб атрымаць зніжку, вам неабходна паказаць бэйдж супрацоўніка кампаніі ў выпадку асабістага звароту і назваць кодавае слова ў выпадку звароту па тэлефоне. Кодавыя словы знаходзяцца ў раздзеле апісання арганізацый, якія прадастаўляюць зніжкі.\n\n" +
                        "Дадзенае прылажэнне з’яўляецца дэманстрацыяй кросплатформеннай распрацоўкі з выкарыстаннем Xamarin.Forms і распрацаванае кампаніяй ScienceSoft. На прыкладзе зніжкавага прылажэння, якое ўтрымлівае спіс і адлюстроўвае на мапе арагнізацыі са зніжкамі для супрацоўнікаў ScienceSoft, можна назіраць выкарыстанне на розных платформах агульнай структуры карыстальніцкага інтэрфейсу і элементаў кіравання. Прылажэнне скампіляванае для 3-х платформ – iOS, Android, Windows Phone – і паўторна выкарыстоўвае 85% радкоў коду на C# (~2000 SLOC з 2400).\n\n" +
                        "Рэалізацыя нетрывіяльных UX-сцэнароў і выкарыстанне платформ-спецыфічных магчымасцяў можа патрабаваць стврэння платформ-спецыфічнага коду.\n" +
                        "Аднак згодна з бягучым станам рэчаў з Xamarin.Forms мы можам заявіць, што платформа:");
                _txtDescriptionBullet1 = new LanguageStrings(
                    "The platform abstracts much of a hard work;",
                    "избавляет от большой порции рутинной работы;",
                    "пазбаўляе ад вялікага аб’ёму руціннай працы;");
                _txtDescriptionBullet2 = new LanguageStrings(
                    "The platform gives rich customization capabilities to implement UI cases and avoid using custom \"renders\";",
                    "дает богатые возможности по настройке внешнего вида приложения, исключая необходимость разрабатывать собственные \"рендеры\";",
                    "дае багатыя магчымасці па наладцы вонкавага выгляду прылажэння, знішчаючы неабходнасць распрацоўваць уласныя \"рэндары\";");
                _txtDescriptionBullet3 = new LanguageStrings(
                    "The platform perfectly fits data driven and enterprise application development saving effort and time.",
                    "отлично подходит для разработки корпоративных приложений и приложений для работы с данными и формами, сохраняя большое количество времени и усилий.",
                    "выдатна пасуе для распрацоўкі карпаратыўных прылажэнняў і прылажэнняў для працы з дадзенымі і формамі, ашчаджаючы вялікую колькасць часу і намаганняў.");
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
