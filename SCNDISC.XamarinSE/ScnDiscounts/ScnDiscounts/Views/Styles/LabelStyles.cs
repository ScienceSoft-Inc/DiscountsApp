using ScnDiscounts.Control;
using ScnDiscounts.Helpers;
using Xamarin.Forms;

namespace ScnDiscounts.Views.Styles
{
    public class LabelStyles : BaseStyles
    {
        public static string PageTitleStyle = "pageTitleStyle";
        public static string TitleStyle = "titleStyle";
        public static string TitleLightStyle = "titleLightStyle";
        public static string DescriptionStyle = "descriptionStyle";
        public static string DescriptionLightStyle = "descriptionLightStyle";
        public static string ListTitleStyle = "listTitleStyle";
        public static string ListPercentStyle = "listPercentStyle";
        public static string LabelPercentStyle = "labelPercentStyle";
        public static string LabelPercentSymbolStyle = "labelPercentSymbolStyle";
        public static string LinkStyle = "linkStyle";
        public static string MenuStyle = "menuStyle";
        public static string MenuDisabledStyle = "menuDisabledStyle";
        public static string CategoryStyle = "categoryStyle";
        public static string DetailTitleStyle = "detailTitleStyle";
        public static string DetailDistanceStyle = "detailDistanceStyle";
        public static string DetailPhoneStyle = "detailPhoneStyle";
        public static string SettingStyle = "settingStyle";
        public static string SettingHintStyle = "settingHintStyle";
        public static string FeedbackLabelStyle = "feedbackLabelStyle";
        public static string FeedbackEntryStyle = "feedbackEntryStyle";
        public static string FeedbackEditorStyle = "feedbackEditorStyle";
        public static string ButtonStyle = "buttonStyle";
        public static string EmptyListLabelStyle = "emptyListLabelStyle";
        public static string SearchBarStyle = "searchBarStyle";
        public static string RatingStyle = "ratingStyle";

        public override void Load()
        {
            #region Style PageTitle
            var pageTitleStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value = "Arial" },
                    new Setter { Property = Label.FontSizeProperty, Value = 21 },
                    new Setter { Property = Label.TextColorProperty, Value = Color.White },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.TailTruncation }
                }
            };
            #endregion

            #region Style Title
            var titleStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value = "Arial" },
                    new Setter { Property = Label.FontSizeProperty, Value = 14 },
                    new Setter { Property = Label.FontAttributesProperty, Value = FontAttributes.Bold },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromHex("444") },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.TailTruncation }
                }
            };
            #endregion

            #region Style Title Light
            var titleLightStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value = "Arial" },
                    new Setter { Property = Label.FontSizeProperty, Value = 14 },
                    new Setter { Property = Label.FontAttributesProperty, Value = FontAttributes.Bold },
                    new Setter { Property = Label.TextColorProperty, Value = Color.White },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.WordWrap }
                }
            };
            #endregion

            #region Style Description
            var descriptionStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value = "Arial" },
                    new Setter { Property = Label.FontSizeProperty, Value = 13 },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromHex("444") },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.TailTruncation }
                }
            };
            #endregion

            #region Style Description Light
            var descriptionLightStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value = "Arial" },
                    new Setter { Property = Label.FontSizeProperty, Value = 13 },
                    new Setter { Property = Label.TextColorProperty, Value = Color.White },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.WordWrap }
                }
            };
            #endregion

            #region Style ListTitle
            var listTitleStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value = "Arial" },
                    new Setter { Property = Label.FontSizeProperty, Value = 15 },
                    new Setter { Property = Label.FontAttributesProperty, Value = FontAttributes.Bold },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromHex("444") },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.TailTruncation }
                }
            };
            #endregion

            #region Style ListPercent
            var listPercentStyle = new Style(typeof(Label))
            {
                Setters = 
                {
                    new Setter { Property = Label.FontFamilyProperty, Value = "Arial" },
                    new Setter { Property = Label.FontSizeProperty, Value = 18 },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromRgb(206, 58, 24) },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.NoWrap }
                }
            };
            #endregion

            #region Style LabelPercent
            var labelPercentStyle = new Style(typeof(Span))
            {
                Setters = 
                {
                    new Setter { Property = Span.FontFamilyProperty, Value = "Arial" },
                    new Setter { Property = Span.FontSizeProperty, Value = 21 },
                    new Setter { Property = Span.TextColorProperty, Value = Color.White }
                }
            };
            #endregion

            #region Style LabelPercentSymbol
            var labelPercentSymbolStyle = new Style(typeof(Span))
            {
                Setters = 
                {
                    new Setter { Property = Span.FontFamilyProperty, Value = "Arial" },
                    new Setter { Property = Span.FontSizeProperty, Value = 11 },
                    new Setter { Property = Span.TextColorProperty, Value = Color.White }
                }
            };
            #endregion

            #region Style LinkStyle
            var linkStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value = "Arial" },
                    new Setter { Property = Label.FontSizeProperty, Value = 13 },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromHex("005EB8") },
                    new Setter { Property = Label.TextDecorationsProperty, Value = TextDecorations.Underline }
                }
            };
            #endregion

            #region Style MenuStyle
            var menuStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value = "Arial" },
                    new Setter { Property = Label.FontSizeProperty, Value = 17 },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromHex("EEE") },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.WordWrap }
                }
            };
            #endregion

            #region Style Menu disabled
            var menuDisabledStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value = "Arial" },
                    new Setter { Property = Label.FontSizeProperty, Value = 17 },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromHex("7F7F7F") },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.WordWrap }
                }
            };
            #endregion

            #region Style Categoty
            var categoryStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value = "Arial" },
                    new Setter { Property = Label.FontSizeProperty, Value = 11 },
                    new Setter { Property = Label.FontAttributesProperty, Value = FontAttributes.Bold },
                    new Setter { Property = Label.TextColorProperty, Value = Color.White },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.TailTruncation }
                }
            };
            #endregion

            #region Style DetailTitle
            var detailTitleStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value = "Arial" },
                    new Setter { Property = Label.FontSizeProperty, Value = 21 },
                    new Setter { Property = Label.FontAttributesProperty, Value = FontAttributes.Bold },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromHex("444") },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.WordWrap }
                }
            };
            #endregion

            #region Style DetailDistance
            var detailDistanceStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value = "Arial" },
                    new Setter { Property = Label.FontSizeProperty, Value = 42 },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromHex("444") },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.NoWrap }
                }
            };
            #endregion

            #region Style DetailPhone
            var detailPhoneStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value = "Arial" },
                    new Setter { Property = Label.FontSizeProperty, Value = 22 },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromHex("005EB8") },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.NoWrap }
                }
            };
            #endregion

            #region Style Setting
            var settingStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value = "Arial" },
                    new Setter { Property = Label.FontSizeProperty, Value = 17 },
                    new Setter { Property = Label.TextColorProperty, Value = Color.White },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.NoWrap }
                }
            };
            #endregion

            #region Style SettingHint
            var settingHintStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value = "Arial" },
                    new Setter { Property = Label.FontSizeProperty, Value = 13 },
                    new Setter { Property = Label.TextColorProperty, Value = Color.White },
                    new Setter { Property = VisualElement.OpacityProperty, Value = 0.5 },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.NoWrap }
                }
            };
            #endregion

            #region Style Feedback Label
            var feedbackLabelStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value = "Arial" },
                    new Setter { Property = Label.FontSizeProperty, Value = 17 },
                    new Setter { Property = Label.TextColorProperty, Value = Color.White }
                }
            };
            #endregion

            #region Style Feedback Entry
            var feedbackEntryStyle = new Style(typeof(Entry))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value = "Arial" },
                    new Setter { Property = Label.FontSizeProperty, Value = 15 },
                    new Setter { Property = VisualElement.BackgroundColorProperty, Value = Color.White },
                    new Setter { Property = Entry.TextColorProperty, Value = Color.FromHex("444") }
                }
            };
            #endregion

            #region Style Feedback Editor
            var feedbackEditorStyle = new Style(typeof(Editor))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value = "Arial" },
                    new Setter { Property = Label.FontSizeProperty, Value = 15 },
                    new Setter { Property = VisualElement.BackgroundColorProperty, Value = Color.White },
                    new Setter { Property = Editor.TextColorProperty, Value = Color.FromHex("444") }
                }
            };
            #endregion

            #region Style Button
            var buttonStyle = new Style(typeof(ButtonExtended))
            {
                Setters =
                {
                    new Setter { Property = Button.FontFamilyProperty, Value = "Arial" },
                    new Setter { Property = Button.FontSizeProperty, Value = 14 },
                    new Setter { Property = Button.FontAttributesProperty, Value = FontAttributes.Bold },
                    new Setter { Property = Button.BorderColorProperty, Value = Color.White },
                    new Setter { Property = Button.BorderWidthProperty, Value = 2 },
                    new Setter { Property = Button.CornerRadiusProperty, Value = 5 },
                    new Setter { Property = Button.TextColorProperty, Value = Color.White },
                    new Setter { Property = ButtonExtended.DisabledTextColorProperty, Value = Color.Gray },
                    new Setter { Property = VisualElement.BackgroundColorProperty, Value = Color.FromHex("005EB8") },
                    new Setter { Property = VisualElement.HeightRequestProperty, Value = 40 }
                }
            };
            #endregion

            #region Style Empty List  Label
            var emptyListLabelStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value = "Arial" },
                    new Setter { Property = Label.FontSizeProperty, Value = 15 },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromHex("444") },
                    new Setter { Property = Label.HorizontalTextAlignmentProperty, Value = TextAlignment.Center },
                    new Setter { Property = Label.VerticalTextAlignmentProperty, Value = TextAlignment.Center },
                    new Setter { Property = Label.LineBreakModeProperty, Value = LineBreakMode.WordWrap }
                }
            };
            #endregion

            #region Style Search Bar
            var searchBarStyle = new Style(typeof(SearchBar))
            {
                Setters =
                {
                    new Setter { Property = Label.FontFamilyProperty, Value = "Arial" },
                    new Setter { Property = Label.FontSizeProperty, Value = 15 },
                    new Setter { Property = VisualElement.BackgroundColorProperty, Value = Color.White },
                    new Setter { Property = SearchBar.TextColorProperty, Value = Color.FromHex("444") },
                    new Setter { Property = SearchBar.CancelButtonColorProperty, Value = Color.FromHex("444") },
                    new Setter { Property = SearchBar.PlaceholderColorProperty, Value = Color.Gray }
                }
            };
            #endregion

            #region Style Rating
            var ratingStyle = new Style(typeof(RatingView))
            {
                Setters =
                {
                    new Setter { Property = RatingView.SpacingProperty, Value = 5 },
                    new Setter { Property = RatingView.RatedSourceProperty, Value = new FontImageSource
                    {
                        FontFamily = Functions.OnPlatform("DiscountIcons", "DiscountIcons.ttf#DiscountIcons"),
                        Glyph = "B",
                        Size = 24,
                        Color = Color.FromHex("005EB8")
                    } },
                    new Setter { Property = RatingView.EmptySourceProperty, Value = new FontImageSource
                    {
                        FontFamily = Functions.OnPlatform("DiscountIcons", "DiscountIcons.ttf#DiscountIcons"),
                        Glyph = "B",
                        Size = 24,
                        Color = Color.LightGray
                    } }
                }
            };
            #endregion

            Resources.Add(PageTitleStyle, pageTitleStyle);
            Resources.Add(TitleStyle, titleStyle);
            Resources.Add(TitleLightStyle, titleLightStyle);
            Resources.Add(DescriptionStyle, descriptionStyle);
            Resources.Add(DescriptionLightStyle, descriptionLightStyle);
            Resources.Add(ListTitleStyle, listTitleStyle);
            Resources.Add(ListPercentStyle, listPercentStyle);
            Resources.Add(LabelPercentStyle, labelPercentStyle);
            Resources.Add(LabelPercentSymbolStyle, labelPercentSymbolStyle);
            Resources.Add(LinkStyle, linkStyle);
            Resources.Add(MenuStyle, menuStyle);
            Resources.Add(MenuDisabledStyle, menuDisabledStyle);
            Resources.Add(CategoryStyle, categoryStyle);
            Resources.Add(DetailTitleStyle, detailTitleStyle);
            Resources.Add(DetailDistanceStyle, detailDistanceStyle);
            Resources.Add(DetailPhoneStyle, detailPhoneStyle);
            Resources.Add(SettingStyle, settingStyle);
            Resources.Add(SettingHintStyle, settingHintStyle);
            Resources.Add(FeedbackLabelStyle, feedbackLabelStyle);
            Resources.Add(FeedbackEntryStyle, feedbackEntryStyle);
            Resources.Add(FeedbackEditorStyle, feedbackEditorStyle);
            Resources.Add(ButtonStyle, buttonStyle);
            Resources.Add(EmptyListLabelStyle, emptyListLabelStyle);
            Resources.Add(SearchBarStyle, searchBarStyle);
            Resources.Add(RatingStyle, ratingStyle);
        }
    }
}
