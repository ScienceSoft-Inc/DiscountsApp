using System;
using Xamarin.Forms;

namespace ScnDiscounts.Control
{
    public class LabelExtended : Label
    {
        public LabelExtended()
        {
        }

        #region TagProperty
        public static readonly BindableProperty TagProperty =
            BindableProperty.Create<LabelExtended, string>(p => p.Tag, "");

        public string Tag
        {
            get { return (string)GetValue(TagProperty); }
            set { SetValue(TagProperty, value); }
        }
        #endregion

        #region IsUnderlineProperty
        public static readonly BindableProperty IsUnderlineProperty =
            BindableProperty.Create<LabelExtended, bool>(p => p.IsUnderline, false);

        public bool IsUnderline
        {
            get { return (bool)GetValue(IsUnderlineProperty); }
            set { SetValue(IsUnderlineProperty, value); }
        }
        #endregion

        #region IsWrappedProperty
        public static readonly BindableProperty IsWrappedProperty =
            BindableProperty.Create<LabelExtended, bool>(p => p.IsWrapped, false);

        public bool IsWrapped
        {
            get { return (bool)GetValue(IsWrappedProperty); }
            set { SetValue(IsWrappedProperty, value); }
        }
        #endregion

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case "Tag":
                    break;
                case "IsWrapped":
                    LineBreakMode = LineBreakMode.WordWrap;
                    break;
            }
        }
    }
}
