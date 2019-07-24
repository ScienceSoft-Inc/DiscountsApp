using ScnDiscounts.Helpers;
using ScnDiscounts.Models;
using ScnDiscounts.Views.ContentUI;
using ScnPage.Plugin.Forms;
using System;

namespace ScnDiscounts.ViewModels
{
    public class FeedbackViewModel : BaseViewModel
    {
        private FeedbackContentUI contentUI => (FeedbackContentUI) ContentUI;

        public new string Title => contentUI.Title;

        private string _name = AppParameters.Config.FeedbackName;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();

                    OnPropertyChanged(nameof(IsValidFeedback));
                }
            }
        }

        private string _comment;

        public string Comment
        {
            get => _comment;
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    OnPropertyChanged();

                    OnPropertyChanged(nameof(IsValidFeedback));
                }
            }
        }

        public bool IsValidFeedback => !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Comment);

        private bool _isSubmitting;

        public bool IsSubmitting
        {
            get => _isSubmitting;
            set
            {
                if (_isSubmitting != value)
                {
                    _isSubmitting = value;
                    OnPropertyChanged();
                }
            }
        }

        public async void BtnSubmit_Clicked(object sender, EventArgs eventArgs)
        {
            IsSubmitting = true;

            var name = Name.SafeTrim();
            var comment = Comment.SafeTrim();

            var isSuccess = await AppData.Discount.SendFeedback(name, comment);
            if (isSuccess)
            {
                await ViewPage.DisplayAlert(null, contentUI.MsgSubmitConfirmation, contentUI.TxtOk);

                Comment = null;
            }
            else
                await ViewPage.DisplayAlert(contentUI.TxtError, contentUI.MsgSubmitError, contentUI.TxtOk);

            AppParameters.Config.FeedbackName = name;
            AppParameters.Config.SaveValues();

            IsSubmitting = false;
        }
    }
}
