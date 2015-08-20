using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ScnDiscounts.Control
{
    public class SwitchExtended : Switch
    {
        #region TextOnProperty
        public static readonly BindableProperty TextOnProperty =
            BindableProperty.Create<SwitchExtended, string>(p => p.TextOn, "On");

        public string TextOn
        {
            get { return (string)GetValue(TextOnProperty); }
            set { SetValue(TextOnProperty, value); }
        }
        #endregion

        #region TextOffProperty
        public static readonly BindableProperty TextOffProperty =
            BindableProperty.Create<SwitchExtended, string>(p => p.TextOff, "Off");

        public string TextOff
        {
            get { return (string)GetValue(TextOffProperty); }
            set { SetValue(TextOffProperty, value); }
        }
        #endregion
    }
}
