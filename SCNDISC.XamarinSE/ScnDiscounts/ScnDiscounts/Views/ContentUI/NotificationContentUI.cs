using ScnDiscounts.Models;

namespace ScnDiscounts.Views.ContentUI
{
    public class NotificationContentUI
    {
        public NotificationContentUI()
        {
            TxtDetails = new LanguageStrings("Details", "Подробнее");
            TxtMarkAsRead = new LanguageStrings("Mark as Read", "Отметить прочитанным");
        }

        public LanguageStrings TxtDetails { get; }

        public LanguageStrings TxtMarkAsRead { get; }
    }
}
