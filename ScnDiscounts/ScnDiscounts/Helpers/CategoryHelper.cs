using System.Collections.Generic;
using Xamarin.Forms;
using ScnDiscounts.Models;

namespace ScnDiscounts.Helpers
{
    public static class CategoryHelper
    {
        public const string ic_pin_cinema = "ic_pin_cinema.png";
        public const string ic_pin_clothing = "ic_pin_clothing.png";
        public const string ic_pin_entertainment = "ic_pin_entertainment.png";
        public const string ic_pin_photo = "ic_pin_photo.png";
        public const string ic_pin_food = "ic_pin_food.png";
        public const string ic_pin_coffee = "ic_pin_coffee.png";
        public const string ic_pin_sport = "ic_pin_sport.png";

        public static string ic_pin_disabled = Device.OnPlatform("MapPins/ic_pin_disabled.png", "ic_pin_disabled.png", "Assets/MapPins/ic_pin_disabled.png");

        static public Dictionary<int, CategoryParam> CategoryList = new Dictionary<int, CategoryParam>
        {
            { 1, new CategoryParam (Color.FromHex("FFD535"), new LanguageStrings("Food", "Еда", "Ежа"), Device.OnPlatform("MapPins/" + ic_pin_food, ic_pin_food, "Assets/MapPins/" + ic_pin_food)) },
            { 2, new CategoryParam (Color.FromHex("593B32"), new LanguageStrings("Coffee", "Кофе", "Кава"), Device.OnPlatform("MapPins/" + ic_pin_coffee, ic_pin_coffee, "Assets/MapPins/" + ic_pin_coffee)) },
            { 3, new CategoryParam (Color.FromHex("F06A14"), new LanguageStrings("Clothing", "Одежда", "Адзенне"), Device.OnPlatform("MapPins/" + ic_pin_clothing, ic_pin_clothing, "Assets/MapPins/" + ic_pin_clothing)) },
            { 4, new CategoryParam (Color.FromHex("8CC453"), new LanguageStrings("Cinema", "Кино", "Кiно"), Device.OnPlatform("MapPins/" + ic_pin_cinema, ic_pin_cinema, "Assets/MapPins/" + ic_pin_cinema)) },
            { 5, new CategoryParam (Color.FromHex("37B6FF"), new LanguageStrings("Photography", "Фотография", "Фатаграфiя"), Device.OnPlatform("MapPins/" + ic_pin_photo, ic_pin_photo, "Assets/MapPins/" + ic_pin_photo)) },
            { 6, new CategoryParam (Color.FromHex("C17CCF"), new LanguageStrings("Entertainment", "Развлечения", "Забавы"), Device.OnPlatform("MapPins/" + ic_pin_entertainment, ic_pin_entertainment, "Assets/MapPins/" + ic_pin_entertainment)) },
            { 7, new CategoryParam (Color.FromHex("FFFF0B"), new LanguageStrings("Sports", "Спорт", "Спорт"), Device.OnPlatform("MapPins/" + ic_pin_sport, ic_pin_sport, "Assets/MapPins/" + ic_pin_sport)) }
        };

        public static string GetName(int typeCode)
        {
            return CategoryList[typeCode].Name;
        }

        public static Color GetColorTheme(int typeCode)
        {
            return CategoryList[typeCode].ColorTheme;
        }
    }

    public struct CategoryParam
    {
        public CategoryParam(Color colorTheme, LanguageStrings name, string icon)
        {
            _colorTheme = colorTheme;
            _name = name;
            _icon = icon;
        }
        private Color _colorTheme;
        public Color ColorTheme { get { return _colorTheme; } }

        private LanguageStrings _name;
        public string Name { get { return _name.Current; } }
        public string DefaultName { get { return _name.Default; } }

        private string _icon;
        public string Icon { get { return _icon; } }
    }
}
