using ScnDiscounts.Models;
using System.Reflection;

namespace ScnDiscounts.Views.ContentUI
{
    public class AboutContentUI : RootContentUI
    {
        public AboutContentUI()
        {
            TxtVersionValue = new AssemblyName(Assembly.GetExecutingAssembly().FullName).Version.ToString();

            TxtPhoneValue = "+375 (17) 293-37-36";
            TxtEmailValue = "contact@scnsoft.com";
            TxtEmailAlternateValue = "feedback@scnsoft.com";
            TxtHttpValue = @"https://www.scnsoft.com/";

            title = new LanguageStrings("About", "О программе");
            _titleVersion = new LanguageStrings("Version", "Версия");
            _titleDeveloper = new LanguageStrings("Developer", "Разработчик");

            _txtTitle = new LanguageStrings(
                "ScienceSoft Corporate Discount Program",
                "Корпоративная дисконтная программа для сотрудников компании ScienceSoft");

            _txtDescription1 = new LanguageStrings(
                "This app contains the full list of ScienceSoft discount partners who provide their services on special conditions for ScienceSoft employees.\n\n" +
                "In most cases, the main term of discount usage is demonstration of a corporate ScienceSoft ID (badge). If any special conditions or additional offers appear to be, they will be noted in partner’s description.\n\n" +
                "With the help of the app you can:",
                "Это приложение содержит список всех компаний-партнеров, которые предоставляют скидки на свои услуги и товары сотрудникам компании ScienceSoft (Научсофт).\n\n" +
                "В большинстве случаев для того, чтобы воспользоваться скидкой нужно предъявить корпоративный бейдж. Дополнительные условия (при их наличии) будут указаны в описании компании-партнёра.\n\n" +
                "Приложение даёт возможность:");

            _txtDescriptionBullet1 = new LanguageStrings(
                "search discounts by name",
                "искать скидки по имени");
            _txtDescriptionBullet2 = new LanguageStrings(
                "filter them by category",
                "сортировать скидки по категориям");
            _txtDescriptionBullet3 = new LanguageStrings(
                "see the discount map",
                "отображать скидки на карте");
            _txtDescriptionBullet4 = new LanguageStrings(
                "evaluate partners’ services",
                "оценивать качество услуг и товаров");

            _txtDescription2 = new LanguageStrings("\nHave fun!\n", "\nПриятного использования!\n");

            _txtDescription3 = new LanguageStrings(
                "If you have any improvements or recommendations to add, feel free to send a message to ",
                "Хотите что-то уточнить? Напишите нам: ");
        }

        public string Icon => "ic_menu_about.png";

        public string TxtDescriptionBulletSymbol { get; } = "\u2022";
        public string TxtPhoneSymbol { get; } = "\u260E";
        public string TxtEmailSymbol { get; } = "\uD83D\uDCE7";
        public string TxtHttpSymbol { get; } = "\uD83C\uDF10";

        public string TxtVersionValue { get; }
        public string TxtPhoneValue { get; }
        public string TxtEmailValue { get; }
        public string TxtEmailAlternateValue { get; }
        public string TxtHttpValue { get; }

        private readonly LanguageStrings _titleVersion;
        public string TitleVersion => _titleVersion.Current;

        private readonly LanguageStrings _titleDeveloper;
        public string TitleDeveloper => _titleDeveloper.Current;

        private readonly LanguageStrings _txtTitle;
        public string TxtTitle => _txtTitle.Current;

        private readonly LanguageStrings _txtDescription1;
        public string TxtDescription1 => _txtDescription1.Current;

        private readonly LanguageStrings _txtDescription2;
        public string TxtDescription2 => _txtDescription2.Current;

        private readonly LanguageStrings _txtDescription3;
        public string TxtDescription3 => _txtDescription3.Current;

        private readonly LanguageStrings _txtDescriptionBullet1;
        public string TxtDescriptionBullet1 => _txtDescriptionBullet1.Current;

        private readonly LanguageStrings _txtDescriptionBullet2;
        public string TxtDescriptionBullet2 => _txtDescriptionBullet2.Current;

        private readonly LanguageStrings _txtDescriptionBullet3;
        public string TxtDescriptionBullet3 => _txtDescriptionBullet3.Current;

        private readonly LanguageStrings _txtDescriptionBullet4;
        public string TxtDescriptionBullet4 => _txtDescriptionBullet4.Current;
    }
}
