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
using Xamarin.Forms.Platform.Android;
using ScnDiscounts.Control;
using ScnDiscounts.Droid.Renderers;
using ScnPage.Plugin.Forms;

[assembly: ExportRenderer(typeof(ImageExtended), typeof(ImageExtendedRenderer))]

namespace ScnDiscounts.Droid.Renderers
{
    public class ImageExtendedRenderer : ImageRenderer
    {
        Page page;
        NavigationPage navigPage;

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                if (GetContainingViewCell(e.NewElement) != null)
                {
                    page = GetContainingPage(e.NewElement);
                    if (page.Parent is TabbedPage)
                    {
                        page.Disappearing += PageContainedInTabbedPageDisapearing;
                        return;
                    }

                    navigPage = GetContainingNavigationPage(page);
                    if (navigPage != null)
                    {
                        navigPage.Popped += OnPagePopped;
                        if (page is BaseContentPage)
                            (page as BaseContentPage).Disposing += OnPageDisposed;
                    }
                }
                else if ((page = GetContainingTabbedPage(e.NewElement)) != null)
                {
                    page.Disappearing += PageContainedInTabbedPageDisapearing;
                }
            }
        }

        void PageContainedInTabbedPageDisapearing(object sender, EventArgs e)
        {
            this.Dispose(true);
            page.Disappearing -= PageContainedInTabbedPageDisapearing;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private void OnPagePopped(object s, NavigationEventArgs e)
        {
            if (e.Page == page)
            {
                this.Dispose(true);
                navigPage.Popped -= OnPagePopped;
            }
        }

        private void OnPageDisposed(object sender, EventArgs e)
        {
            this.Dispose(true);
            (page as BaseContentPage).Disposing -= OnPageDisposed;
        }

        private Page GetContainingPage(Xamarin.Forms.Element element)
        {
            Element parentElement = element.ParentView;

            if (typeof(Page).IsAssignableFrom(parentElement.GetType()))
                return (Page)parentElement;
            else
                return GetContainingPage(parentElement);
        }

        private ViewCell GetContainingViewCell(Xamarin.Forms.Element element)
        {
            Element parentElement = element.Parent;

            if (parentElement == null)
                return null;

            if (typeof(ViewCell).IsAssignableFrom(parentElement.GetType()))
                return (ViewCell)parentElement;
            else
                return GetContainingViewCell(parentElement);
        }

        private TabbedPage GetContainingTabbedPage(Element element)
        {
            Element parentElement = element.Parent;

            if (parentElement == null)
                return null;

            if (typeof(TabbedPage).IsAssignableFrom(parentElement.GetType()))
                return (TabbedPage)parentElement;
            else
                return GetContainingTabbedPage(parentElement);
        }

        private NavigationPage GetContainingNavigationPage(Element element)
        {
            Element parentElement = element.Parent;

            if (parentElement == null)
                return null;

            if (typeof(NavigationPage).IsAssignableFrom(parentElement.GetType()))
                return (NavigationPage)parentElement;
            else
                return GetContainingNavigationPage(parentElement);
        }
    }
}