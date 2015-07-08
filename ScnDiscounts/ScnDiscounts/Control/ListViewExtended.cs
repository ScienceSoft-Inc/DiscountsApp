using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ScnDiscounts.Control
{
    public class ListViewExtended : ListView
    {
        #region IsScrollable
        public static readonly BindableProperty IsScrollableProperty =
            BindableProperty.Create<ListViewExtended, bool>(p => p.IsScrollable, true);

        public bool IsScrollable
        {
            get { return (bool)GetValue(IsScrollableProperty); }
            set { SetValue(IsScrollableProperty, value); }
        }
        #endregion
    }
}
