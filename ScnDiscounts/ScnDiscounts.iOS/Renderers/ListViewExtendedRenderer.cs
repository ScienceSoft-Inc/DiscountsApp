using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ScnDiscounts.Control;
using ScnDiscounts.iOS.Renderers;

[assembly: ExportRenderer(typeof(ListViewExtended), typeof(ListViewExtendedRenderer))]

namespace ScnDiscounts.iOS.Renderers
{
    public class ListViewExtendedRenderer : ListViewRenderer 
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);
            
            if ((Control == null) || (Element == null))
                return;

            var listViewExtended = (ListViewExtended)Element;
            var tableView = Control as UITableView;
            tableView.ScrollEnabled = listViewExtended.IsScrollable;
        }
    }
}