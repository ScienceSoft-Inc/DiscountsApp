using ScnDiscounts.Models;

namespace ScnDiscounts.Views.ContentUI
{
    public class SettingsContentUI : RootContentUI
    {
        public SettingsContentUI()
        {
            title = new LanguageStrings("Settings", "Настройки");

            _txtLanguage = new LanguageStrings("Language", "Язык");
            _txtLanguageSel = new LanguageStrings("Choose language", "Выберите язык");
            _txtUpdateDb = new LanguageStrings("Update discounts database", "Обновить базу скидок");
            _txtProcessLoadingCompleted = new LanguageStrings("Discounts database has been updated successfully",
                "База скидок была успешно обновлена");
            _titleErrLoading = new LanguageStrings("Error updating data", "Ошибка обновления данных");
            _txtProcessLoadingError = new LanguageStrings(
                "Try to update discounts database once again. If the error appears, contact the developers.",
                "Попробуйте обновить базу скидок ещё раз. Если ошибка появится, то сообщите об этом разработчикам.");
        }

        public string Icon => "ic_settings.png";

        private readonly LanguageStrings _txtLanguageSel;
        public string TxtLanguageSel => _txtLanguageSel.Current;

        private readonly LanguageStrings _txtLanguage;
        public string TxtLanguage => _txtLanguage.Current;

        private readonly LanguageStrings _txtUpdateDb;
        public string TxtUpdateDb => _txtUpdateDb.Current;

        private readonly LanguageStrings _txtProcessLoadingCompleted;
        public string TxtProcessLoadingCompleted => _txtProcessLoadingCompleted.Current;

        private readonly LanguageStrings _txtProcessLoadingError;
        public string TxtProcessLoadingError => _txtProcessLoadingError.Current;

        private readonly LanguageStrings _titleErrLoading;
        public string TitleErrLoading => _titleErrLoading.Current;
    }
}
