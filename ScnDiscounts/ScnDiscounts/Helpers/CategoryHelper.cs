using System.Collections.Generic;
using Xamarin.Forms;

namespace ScnDiscounts.Helpers
{
    public static class CategoryHelper
    {
        public const string ic_pin_blue = "ic_pin_blue.png";
        public const string ic_pin_green = "ic_pin_green.png";
        public const string ic_pin_grey = "ic_pin_grey.png";
        public const string ic_pin_orange = "ic_pin_orange.png";
        public const string ic_pin_purple = "ic_pin_purple.png";
        public const string ic_pin_rose = "ic_pin_rose.png";
        public const string ic_pin_yellow = "ic_pin_yellow.png";


        static public Dictionary<int, CategoryParam> CategoryList = new Dictionary<int, CategoryParam>
        {
            { 1, new CategoryParam (Color.FromHex("ddca00"), new PropertyLang("Food", "Еда", "Ежа"), Device.OnPlatform("MapPins/" + ic_pin_yellow, ic_pin_yellow, "Assets/MapPins/" + ic_pin_yellow)) },
            { 2, new CategoryParam (Color.FromHex("3875d7"), new PropertyLang("Coffee", "Кофе", "Кава"), Device.OnPlatform("MapPins/" + ic_pin_blue, ic_pin_blue, "Assets/MapPins/" + ic_pin_blue)) },
            { 3, new CategoryParam (Color.FromHex("FF6600"), new PropertyLang("Clothing", "Одежда", "Адзенне"), Device.OnPlatform("MapPins/" + ic_pin_orange, ic_pin_orange, "Assets/MapPins/" + ic_pin_orange)) },
            { 4, new CategoryParam (Color.FromHex("51a123"), new PropertyLang("Cinema", "Кино", "Кiно"), Device.OnPlatform("MapPins/" + ic_pin_green, ic_pin_green, "Assets/MapPins/" + ic_pin_green)) },
            { 5, new CategoryParam (Color.FromHex("666"), new PropertyLang("Photography", "Фотография", "Фатаграфiя"), Device.OnPlatform("MapPins/" + ic_pin_grey, ic_pin_grey, "Assets/MapPins/" + ic_pin_grey)) },
            { 6, new CategoryParam (Color.FromHex("8A21B9"), new PropertyLang("Entertainment", "Развлечения", "Забавы"), Device.OnPlatform("MapPins/" + ic_pin_purple, ic_pin_purple, "Assets/MapPins/" + ic_pin_purple)) },
            { 7, new CategoryParam (Color.FromHex("F42494"), new PropertyLang("Sports", "Спорт", "Спорт"), Device.OnPlatform("MapPins/" + ic_pin_rose, ic_pin_rose, "Assets/MapPins/" + ic_pin_rose)) }
        };

       // static public CategoryParam GetCategoryBy

    }

    public struct CategoryParam
    {
        public CategoryParam(Color colorTheme, PropertyLang name, string icon)
        {
            _colorTheme = colorTheme;
            _name = name;
            _icon = icon;
        }
        private Color _colorTheme;
        public Color ColorTheme { get { return _colorTheme; } }

        private PropertyLang _name;
        public string Name { get { return _name.ActualValue(); } }

        private string _icon;
        public string Icon { get { return _icon; } }
    }
}
