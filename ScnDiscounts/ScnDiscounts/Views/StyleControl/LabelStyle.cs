using Xamarin.Forms;

namespace ScnDiscounts.Views.StyleControl
{
    public class LabelStyle
    {
        //label стиль для заголовка окна (для WP)
        private Style _subTitlePage;
        public Style SubTitlePage
        {
            get { return _subTitlePage; }
            set { _subTitlePage = value; }
        }

        //label стиль для подписи
        private Style _caption;
        public Style Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        //label стиль для доп. информации
        private Style _additional;
        public Style Additional
        {
            get { return _additional; }
            set { _additional = value; }
        }

        //label стиль для доп. информации акцентного цвета
        private Style _additionalAccent;
        public Style AdditionalAccent
        {
            get { return _additionalAccent; }
            set { _additionalAccent = value; }
        }

        //label стиль для доп. информации
        private Style _title;
        public Style Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public LabelStyle()
        {
            #region SubTitlePage
            _subTitlePage = new Style(typeof(Label));
            _subTitlePage.BaseResourceKey = Device.Styles.CaptionStyleKey;
            _subTitlePage.Setters.Add(new Setter { Property = Label.FontSizeProperty, Value = 22 });
            _subTitlePage.Setters.Add(new Setter { Property = Label.HeightRequestProperty, Value = 48 });
            _subTitlePage.Setters.Add(new Setter { Property = Label.TranslationXProperty, Value = 10 });
            #endregion
        
            #region CaptionStyle
            _caption = new Style(typeof(Label));
            _caption.Setters.Add(new Setter { Property = Label.TextColorProperty, Value = Color.Default });
            _caption.Setters.Add(new Setter { Property = Label.FontSizeProperty, Value = Device.GetNamedSize(NamedSize.Small, typeof(Label)) });
            //_caption.Setters.Add(new Setter { Property = Label.TranslationXProperty, Value = 10 });
            #endregion

            #region AdditionalStyle
            _additional = new Style(typeof(Label));
            _additional.BaseResourceKey = Device.Styles.BodyStyleKey;
            //_additional.Setters.Add(new Setter { Property = Label.TranslationXProperty, Value = 10 });
            #endregion

            #region AdditionalAccentStyle
            _additionalAccent = new Style(typeof(Label));
            _additionalAccent.BaseResourceKey = Device.Styles.BodyStyleKey;
            //_additionalAccent.Setters.Add(new Setter { Property = Label.TranslationXProperty, Value = 10 });
            _additionalAccent.Setters.Add(new Setter { Property = Label.TextColorProperty, Value = Color.Accent });
            #endregion

            #region Title
            _title = new Style(typeof(Label));
            _title.BaseResourceKey = Device.Styles.SubtitleStyleKey; 
            _title.Setters.Add(new Setter { Property = Label.TextColorProperty, Value = Color.White });
            //_title.Setters.Add(new Setter { Property = Label.TranslationXProperty, Value = 10 });
            #endregion
        }
    }
}
