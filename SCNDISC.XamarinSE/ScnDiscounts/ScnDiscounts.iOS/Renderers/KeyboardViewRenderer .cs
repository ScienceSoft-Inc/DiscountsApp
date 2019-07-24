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
        private Thickness? _oldMargin;

        private NSObject _keyboardShowObserver;
        private NSObject _keyboardHideObserver;
        private NSObject _keyboardChangeObserver;

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

            if (_keyboardChangeObserver == null)
                _keyboardChangeObserver = UIKeyboard.Notifications.ObserveWillChangeFrame(OnKeyboardChanged);
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

            if (_keyboardChangeObserver != null)
            {
                _keyboardChangeObserver.Dispose();
                _keyboardChangeObserver = null;
            }
        }

        private void OnKeyboardShow(object sender, UIKeyboardEventArgs args)
        {
            if (Element != null)
            {
                var key = new NSString(UIKeyboard.FrameEndUserInfoKey);
                var result = (NSValue) args.Notification.UserInfo.ObjectForKey(key);
                var keyboardSize = result.RectangleFValue.Size;

                var oldMargin = Element.Margin;
                _oldMargin = oldMargin;

                Element.Margin = new Thickness(oldMargin.Left, oldMargin.Top, oldMargin.Right,
                    oldMargin.Bottom + keyboardSize.Height);
            }
        }

        private void OnKeyboardHide(object sender, UIKeyboardEventArgs args)
        {
            if (Element != null && _oldMargin.HasValue)
            {
                Element.Margin = _oldMargin.Value;
                _oldMargin = null;
            }
        }

        private void OnKeyboardChanged(object sender, UIKeyboardEventArgs args)
        {
            if (Element != null && _oldMargin.HasValue)
            {
                Element.Margin = _oldMargin.Value;
                _oldMargin = null;
            }
        }
    }
}