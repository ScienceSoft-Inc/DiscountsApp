using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using ScnDiscounts.Control;
using ScnDiscounts.Droid.Renderers;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(SwitchExtended), typeof(SwitchExtendedRenderer))]

namespace ScnDiscounts.Droid.Renderers
{
    public class SwitchExtendedRenderer : SwitchRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Switch> e)
        {
            base.OnElementChanged(e);
            var view = (SwitchExtended)Element;

            if (Control != null)
            {
                Control.TextOn = view.TextOn;
                Control.TextOff = view.TextOff;
            }
          
            /*if (e.OldElement != null)
            {
                this.Element.Toggled -= ElementToggled;
                return;
            }

            if (this.Element == null)
            {
                return;
            }

            var switchControl = new Android.Widget.Switch(Forms.Context)
            {
                TextOn = this.Element.TextOn,
                TextOff = this.Element.TextOff
            };

            switchControl.CheckedChange += ControlValueChanged;
            this.Element.Toggled += ElementToggled;

            this.SetNativeControl(switchControl);*/
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
           /* if (this.Element == null)
            {
                return;
            }

            var switchControl = new Android.Widget.Switch(Forms.Context)
            {
                TextOn = this.Element.TextOn,
                TextOff = this.Element.TextOff
            };*/
            base.OnElementPropertyChanged(sender, e);

            var view = (SwitchExtended)Element;

            if (e.PropertyName == "TextOn")
                Control.TextOn = view.TextOn;

            if (e.PropertyName == "TextOff")
                Control.TextOff = view.TextOff;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Control.CheckedChange -= this.ControlValueChanged;
                this.Element.Toggled -= ElementToggled;
            }

            base.Dispose(disposing);
        }

        private void ElementToggled(object sender, ToggledEventArgs e)
        {
            this.Control.Checked = this.Element.IsToggled;
        }

        private void ControlValueChanged(object sender, EventArgs e)
        {
            this.Element.IsToggled = this.Control.Checked;
        }
    }
}