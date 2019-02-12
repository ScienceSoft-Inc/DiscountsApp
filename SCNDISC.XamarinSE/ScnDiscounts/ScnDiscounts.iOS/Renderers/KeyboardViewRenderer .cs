using Foundation;
using ScnDiscounts.Control;
using ScnDiscounts.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(KeyboardView), typeof(KeyboardViewRenderer))]
namespace ScnDiscounts.iOS.Renderers
{
    public class KeyboardViewRenderer : ViewRenderer
    {
        private NSObject _keyboardShowObserver;
        private NSObject _keyboardHideObserver;

        private Thickness _oldMargin;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
                RegisterForKeyboardNotifications();

            if (e.OldElement != null)
                UnregisterForKeyboardNotifications();
        }

        private void RegisterForKeyboardNotifications()
        {
            if (_keyboardShowObserver == null)
                _keyboardShowObserver = UIKeyboard.Notifications.ObserveWillShow(OnKeyboardShow);

            if (_keyboardHideObserver == null)
                _keyboardHideObserver = UIKeyboard.Notifications.ObserveWillHide(OnKeyboardHide);
        }

        private void UnregisterForKeyboardNotifications()
        {
            if (_keyboardShowObserver != null)
            {
                _keyboardShowObserver.Dispose();
                _keyboardShowObserver = null;
            }

            if (_keyboardHideObserver != null)
            {
                _keyboardHideObserver.Dispose();
                _keyboardHideObserver = null;
            }
        }

        private void OnKeyboardShow(object sender, UIKeyboardEventArgs args)
        {
            if (Element != null)
            {
                var key = new NSString(UIKeyboard.FrameEndUserInfoKey);
                var result = (NSValue) args.Notification.UserInfo.ObjectForKey(key);
                var keyboardSize = result.RectangleFValue.Size;

                _oldMargin = Element.Margin;
                Element.Margin = new Thickness(_oldMargin.Left, _oldMargin.Top, _oldMargin.Right,
                    _oldMargin.Bottom + keyboardSize.Height);
            }
        }

        private void OnKeyboardHide(object sender, UIKeyboardEventArgs args)
        {
            if (Element != null)
                Element.Margin = _oldMargin;
        }
    }
}