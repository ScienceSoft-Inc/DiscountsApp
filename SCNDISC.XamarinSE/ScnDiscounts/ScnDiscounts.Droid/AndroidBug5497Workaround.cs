using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;

namespace ScnDiscounts.Droid
{
    public class AndroidBug5497Workaround
    {
        // For more information, see https://code.google.com/p/android/issues/detail?id=5497
        // To use this class, simply invoke assistActivity() on an Activity that already has its content view set.

        // CREDIT TO Joseph Johnson (http://stackoverflow.com/users/341631/joseph-johnson) for publishing the original Android solution on stackoverflow.com

        public static AndroidBug5497Workaround AssistActivity(Activity activity) => new AndroidBug5497Workaround(activity);

        private readonly View _childOfContent;
        private int _usableHeightPrevious;
        private readonly FrameLayout.LayoutParams _frameLayoutParams;

        private AndroidBug5497Workaround(Activity activity)
        {
            var content = activity.FindViewById<FrameLayout>(Android.Resource.Id.Content);
            _childOfContent = content.GetChildAt(0);

            var vto = _childOfContent.ViewTreeObserver;
            vto.GlobalLayout += PossiblyResizeChildOfContent;

            _frameLayoutParams = (FrameLayout.LayoutParams)_childOfContent.LayoutParameters;
        }

        private void PossiblyResizeChildOfContent(object sender, EventArgs e)
        {
            var usableHeightNow = ComputeUsableHeight();
            if (usableHeightNow != _usableHeightPrevious)
            {
                var usableHeightSansKeyboard = _childOfContent.RootView.Height;
                var heightDifference = usableHeightSansKeyboard - usableHeightNow;

                _frameLayoutParams.Height = usableHeightSansKeyboard - heightDifference;

                _childOfContent.RequestLayout();
                _usableHeightPrevious = usableHeightNow;
            }
        }

        private int ComputeUsableHeight()
        {
            var r = new Rect();
            _childOfContent.GetWindowVisibleDisplayFrame(r);
            return Build.VERSION.SdkInt < BuildVersionCodes.Lollipop ? r.Bottom - r.Top : r.Bottom;
        }
    }
}
