using Xamarin.Forms;

namespace ScnDiscounts.Control
{
    public class ButtonExtended : Button
    {
        public static readonly BindableProperty DisabledTextColorProperty =
            BindableProperty.Create(nameof(DisabledTextColor), typeof(Color), typeof(ButtonExtended), Color.Default);

        public Color DisabledTextColor
        {
            get => (Color) GetValue(DisabledTextColorProperty);
            set => SetValue(DisabledTextColorProperty, value);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsEnabledProperty.PropertyName)
                BorderColor = IsEnabled ? TextColor : DisabledTextColor;
        }
    }
}
