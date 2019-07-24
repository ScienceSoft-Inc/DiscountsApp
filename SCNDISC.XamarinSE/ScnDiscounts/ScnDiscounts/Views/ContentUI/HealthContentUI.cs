using ScnDiscounts.Models;

namespace ScnDiscounts.Views.ContentUI
{
    public class HealthContentUI : RootContentUI
    {
        public HealthContentUI()
        {
            title = new LanguageStrings("Health insurance", "Мед. страховка");
        }

        public string Icon => "ic_health.png";
    }
}
