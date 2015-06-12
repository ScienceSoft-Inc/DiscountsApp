using Xamarin.Forms;

namespace ScnDiscounts.Views.StyleControl
{
    public class EntryStyle
    {
        //entry стандартное поле ввода под заголовком
        private Style _input;
        public Style Input
        {
            get { return _input; }
            set { _input = value; }
        }

        public EntryStyle()
        {
            #region InputStyle
            _input = new Style(typeof(Entry));

            _input.Setters.Add(new Setter { Property = Entry.TranslationXProperty, Value = -10 });
            _input.Setters.Add(new Setter { Property = Entry.TranslationYProperty, Value = -10 });
            #endregion
        }
    }
}
