using ScnDiscounts.Models;

namespace ScnDiscounts.Views.ContentUI
{
    public class FeedbackContentUI : RootContentUI
    {
        public FeedbackContentUI()
        {
            title = new LanguageStrings("Feedback", "Напишите нам");

            _txtName = new LanguageStrings("Your name", "Ваше имя");
            _txtComment = new LanguageStrings("Comments and proposals", "Комментарии и предложения");
            _txtSubmit = new LanguageStrings("Submit", "Отправить");
            _msgSubmitConfirmation = new LanguageStrings("Your message has been successfully submitted", "Ваше сообщение было успешно отправлено");
            _msgSubmitError = new LanguageStrings("Your message has not been submitted", "Ваше сообщение не было отправлено");
        }

        public string Icon => "ic_feedback.png";

        private readonly LanguageStrings _txtName;
        public string TxtName => _txtName.Current;

        private readonly LanguageStrings _txtComment;
        public string TxtComment => _txtComment.Current;

        private readonly LanguageStrings _txtSubmit;
        public string TxtSubmit => _txtSubmit.Current;

        private readonly LanguageStrings _msgSubmitConfirmation;
        public string MsgSubmitConfirmation => _msgSubmitConfirmation.Current;

        private readonly LanguageStrings _msgSubmitError;
        public string MsgSubmitError => _msgSubmitError.Current;
    }
}
