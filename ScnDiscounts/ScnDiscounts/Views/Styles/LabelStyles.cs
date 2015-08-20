using Xamarin.Forms;

namespace ScnDiscounts.Views.Styles
{
    public class LabelStyles : BaseStyles
    {
        public static string PageTitleStyle = "pageTitleStyle";
        public static string TitleStyle = "titleStyle";
        public static string DescriptionStyle = "descriptionStyle";
        public static string DescriptionLightStyle = "descriptionLightStyle";
        public static string ListTitleStyle = "listTitleStyle";
        public static string ListPercentStyle = "listPercentStyle";
        public static string ListPercentSymbolStyle = "listPercentSymbolStyle";
        public static string LabelPercentStyle = "labelPercentStyle";
        public static string LabelPercentSymbolStyle = "labelPercentSymbolStyle";
        public static string LinkStyle = "linkStyle";
        public static string MenuStyle = "menuStyle";
        public static string MenuHintStyle = "menuHintStyle";
        public static string MenuDisabledStyle = "menuDisabledStyle";
        public static string CategoryStyle = "categoryStyle";
        public static string DetailTitleStyle = "detailTitleStyle";
        public static string DetailDistanceStyle = "detailDistanceStyle";
        public static string DetailPhoneStyle = "detailPhoneStyle";
        public static string SettingStyle = "settingStyle";
        public static string SettingHintStyle = "settingHintStyle";

        public override void Load()
        {
            #region Style PageTitle
            Style pageTitleStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value =  Device.OnPlatform("Arial", "Arial", "Arial") },
                    new Setter { Property = Label.FontSizeProperty, Value = Device.OnPlatform(18, 21, 25) },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromRgb(255, 255, 255) },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.TailTruncation }
                }
            };
            #endregion

            #region Style Title
            Style titleStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value =  Device.OnPlatform("Arial", "Arial", "Arial") },
                    new Setter { Property = Label.FontSizeProperty, Value = Device.OnPlatform(14, 14, 18) },
                    new Setter { Property = Label.FontAttributesProperty, Value = FontAttributes.Bold },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromHex("444") },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.TailTruncation }
                }
            };
            #endregion

            #region Style Description
            Style descriptionStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value =  Device.OnPlatform("Arial", "Arial", "Arial") },
                    new Setter { Property = Label.FontSizeProperty, Value = Device.OnPlatform(13, 13, 17) },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromHex("444") },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.TailTruncation }
                }
            };
            #endregion

            #region Style Description
            Style descriptionLightStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value =  Device.OnPlatform("Arial", "Arial", "Arial") },
                    new Setter { Property = Label.FontSizeProperty, Value = Device.OnPlatform(13, 13, 17) },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromRgb(255, 255, 255) },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.WordWrap }
                }
            };
            #endregion

            #region Style ListTitle
            Style listTitleStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value =  Device.OnPlatform("Arial", "Arial", "Arial") },
                    new Setter { Property = Label.FontSizeProperty, Value = Device.OnPlatform(14, 15, 19) },
                    new Setter { Property = Label.FontAttributesProperty, Value = FontAttributes.Bold },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromHex("444") },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.TailTruncation }
                }
            };
            #endregion

            #region Style ListPercent
            Style listPercentStyle = new Style(typeof(Label))
            {
                Setters = 
                {
                    new Setter { Property = Label.FontFamilyProperty, Value =  Device.OnPlatform("Arial", "Arial", "Arial") },
                    new Setter { Property = Label.FontSizeProperty, Value = Device.OnPlatform(16, 18, 22) },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromRgb(206, 58, 24) },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.NoWrap }
                }
            };
            #endregion

            #region Style ListPercentSymbol
            Style listPercentSymbolStyle = new Style(typeof(Label))
            {
                Setters = 
                {
                    new Setter { Property = Label.TextProperty, Value = "%" },
                    new Setter { Property = Label.FontFamilyProperty, Value =  Device.OnPlatform("Arial", "Arial", "Arial") },
                    new Setter { Property = Label.FontAttributesProperty, Value = FontAttributes.Bold },
                    new Setter { Property = Label.FontSizeProperty, Value = Device.OnPlatform(12, 12, 16) },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromRgb(206, 58, 24) },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.NoWrap },
                    new Setter { Property = Label.TranslationYProperty, Value = -2 }
                }
            };
            #endregion

            #region Style LabelPercent
            Style labelPercentStyle = new Style(typeof(Label))
            {
                Setters = 
                {
                    new Setter { Property = Label.FontFamilyProperty, Value =  Device.OnPlatform("Arial", "Arial", "Arial") },
                    new Setter { Property = Label.FontSizeProperty, Value = Device.OnPlatform(21, 21, 25) },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromRgb(255, 255, 255) },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.NoWrap }
                }
            };
            #endregion

            #region Style LabelPercentSymbol
            Style labelPercentSymbolStyle = new Style(typeof(Label))
            {
                Setters = 
                {
                    new Setter { Property = Label.TextProperty, Value = "%" },
                    new Setter { Property = Label.FontFamilyProperty, Value =  Device.OnPlatform("Arial", "Arial", "Arial") },
                    new Setter { Property = Label.FontSizeProperty, Value = Device.OnPlatform(11, 11, 15) },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromRgb(255, 255, 255) },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.NoWrap },
                    new Setter { Property = Label.TranslationYProperty, Value = -2 }
                }
            };
            #endregion

            #region Style LinkStyle
            Style linkStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value =  Device.OnPlatform("Arial", "Arial", "Arial") },
                    new Setter { Property = Label.FontSizeProperty, Value = Device.OnPlatform(13, 13, 17) },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromHex("005EB8") },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.TailTruncation }
                }
            };
            #endregion

            #region Style MenuStyle
            Style menuStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value =  Device.OnPlatform("Arial", "Arial", "Arial") },
                    new Setter { Property = Label.FontSizeProperty, Value = Device.OnPlatform(17, 17, 22) },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromHex("EEE") },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.NoWrap }
                }
            };
            #endregion

            #region Style MenuHint
            Style menuHintStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value =  Device.OnPlatform("Arial", "Arial", "Arial") },
                    new Setter { Property = Label.FontSizeProperty, Value = Device.OnPlatform(13, 13, 18) },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromHex("EEE") },
                    new Setter { Property = Label.OpacityProperty, Value = 0.5 },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.NoWrap }
                }
            };
            #endregion

            #region Style Menu disabled
            Style menuDisabledStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value =  Device.OnPlatform("Arial", "Arial", "Arial") },
                    new Setter { Property = Label.FontSizeProperty, Value = Device.OnPlatform(17, 17, 22) },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromHex("7F7F7F") },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.NoWrap }
                }
            };
            #endregion

            #region Style Categoty
            Style categoryStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value =  Device.OnPlatform("Arial", "Arial", "Arial") },
                    new Setter { Property = Label.FontAttributesProperty, Value = FontAttributes.Bold },
                    new Setter { Property = Label.FontSizeProperty, Value = Device.OnPlatform(9, 11, 15) },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromRgb(255, 255, 255) },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.NoWrap }
                }
            };
            #endregion

            #region Style DetailTitle
            Style detailTitleStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value =  Device.OnPlatform("Arial", "Arial", "Arial") },
                    new Setter { Property = Label.FontSizeProperty, Value = Device.OnPlatform(18, 21, 25) },
                    new Setter { Property = Label.FontAttributesProperty, Value = FontAttributes.Bold },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromRgb(68, 68, 68) },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.TailTruncation }
                }
            };
            #endregion

            #region Style DetailDistance
            Style detailDistanceStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value =  Device.OnPlatform("Arial", "Arial", "Arial") },
                    new Setter { Property = Label.FontSizeProperty, Value = Device.OnPlatform(42, 42, 46) },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromRgb(60, 68, 68) },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.NoWrap }
                }
            };
            #endregion

            #region Style DetailPhone
            Style detailPhoneStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value =  Device.OnPlatform("Arial", "Arial", "Arial") },
                    new Setter { Property = Label.FontSizeProperty, Value = Device.OnPlatform(19, 22, 26) },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromRgb(0, 94, 184) },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.NoWrap }
                }
            };
            #endregion

            #region Style Setting
            Style settingStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value =  Device.OnPlatform("Arial", "Arial", "Arial") },
                    new Setter { Property = Label.FontSizeProperty, Value = Device.OnPlatform(17, 17, 21) },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromRgb(255, 255, 255) },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.NoWrap }
                }
            };
            #endregion

            #region Style SettingHint
            Style settingHintStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value =  Device.OnPlatform("Arial", "Arial", "Arial") },
                    new Setter { Property = Label.FontSizeProperty, Value = Device.OnPlatform(13, 13, 17) },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromRgb(255, 255, 255) },
                    new Setter { Property = Label.OpacityProperty, Value = 0.5 },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.NoWrap }
                }
            };
            #endregion

            Resources.Add(PageTitleStyle, pageTitleStyle);
            Resources.Add(TitleStyle, titleStyle);
            Resources.Add(DescriptionStyle, descriptionStyle);
            Resources.Add(DescriptionLightStyle, descriptionLightStyle);
            Resources.Add(ListTitleStyle, listTitleStyle);
            Resources.Add(ListPercentStyle, listPercentStyle);
            Resources.Add(ListPercentSymbolStyle, listPercentSymbolStyle);
            Resources.Add(LabelPercentStyle, labelPercentStyle);
            Resources.Add(LabelPercentSymbolStyle, labelPercentSymbolStyle);
            Resources.Add(LinkStyle, linkStyle);
            Resources.Add(MenuStyle, menuStyle);
            Resources.Add(MenuHintStyle, menuHintStyle);
            Resources.Add(MenuDisabledStyle, menuDisabledStyle);
            Resources.Add(CategoryStyle, categoryStyle);
            Resources.Add(DetailTitleStyle, detailTitleStyle);
            Resources.Add(DetailDistanceStyle, detailDistanceStyle);
            Resources.Add(DetailPhoneStyle, detailPhoneStyle);
            Resources.Add(SettingStyle, settingStyle);
            Resources.Add(SettingHintStyle, settingHintStyle);
        }
    }
}
