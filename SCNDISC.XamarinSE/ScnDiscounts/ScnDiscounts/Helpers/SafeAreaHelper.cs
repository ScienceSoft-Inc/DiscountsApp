using ScnDiscounts.DependencyInterface;
using ScnDiscounts.Models;
using ScnDiscounts.ValueConverters;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace ScnDiscounts.Helpers
{
    public class SafeAreaHelper : NotifyPropertyChanged
    {
        [Flags]
        public enum CustomSafeAreaFlags
        {
            None = 0,
            Left = 1,
            Top = 2,
            Right = 4,
            Bottom = 8,
            Horizontal = Left | Right,
            Vertical = Top | Bottom,
            All = -1
        }

        protected static readonly CustomSafeAreaConverter CustomSafeAreaConverter = new CustomSafeAreaConverter();

        private Thickness _pagePadding;

        public Thickness PagePadding
        {
            get => _pagePadding;
            set
            {
                if (_pagePadding != value)
                {
                    _pagePadding = (Thickness) CustomSafeAreaConverter.Convert(value, typeof(Thickness), PageSafeArea,
                        CultureInfo.CurrentCulture);

                    OnPropertyChanged();

                    SafeArea = value;
                }
            }
        }

        private Thickness _safeArea;

        public Thickness SafeArea
        {
            get => _safeArea;
            set
            {
                if (_safeArea != value)
                {
                    _safeArea = value;
                    OnPropertyChanged();
                }
            }
        }

        public CustomSafeAreaFlags PageSafeArea { get; protected set; }

        public void UseSafeArea(Page page, CustomSafeAreaFlags pageSafeArea)
        {
            PageSafeArea = pageSafeArea;

            var binding = new Binding(nameof(PagePadding), BindingMode.TwoWay, source: this);
            page.SetBinding(Page.PaddingProperty, binding);

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page.SetUseSafeArea(
                        page.On<Xamarin.Forms.PlatformConfiguration.iOS>(), true);

                    if (!DependencyService.Get<IPhoneService>().HasSafeAreaSupport)
                    {
                        page.SizeChanged += (sender, args) =>
                            page.Padding = DependencyService.Get<IPhoneService>().SafeAreaInsets;
                    }
                    break;
                case Device.Android:
                    page.SizeChanged += (sender, args) =>
                        page.Padding = DependencyService.Get<IPhoneService>().SafeAreaInsets;
                    break;
            }
        }

        public void UseSafeArea(View view, CustomSafeAreaFlags viewSafeArea)
        {
            var binding = new Binding(nameof(SafeArea), BindingMode.OneWay, CustomSafeAreaConverter,
                viewSafeArea, source: this);

            view.SetBinding(View.MarginProperty, binding);
        }
    }
}
