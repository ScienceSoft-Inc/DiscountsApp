using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ScnDiscounts.Control
{
    public class BoxViewGesture : BoxView
    {
        public BoxViewGesture(View owner)
        {
            Owner = owner;
        }

        public View Owner { get; private set; }

        public object GestureSender()
        {
            if (Owner == null)
                return this;

            if ((Owner is ViewGesture) && ((Owner as ViewGesture).Content != null))
                return (Owner as ViewGesture).Content;
            else
                return this;
        }

        #region TapGesture
        public event EventHandler Tap;
        public void OnTap()
        {
            if (Tap != null) Tap(GestureSender(), EventArgs.Empty);
        }

        public event EventHandler TapBegan;
        public void OnTapBegan()
        {
            if (TapBegan != null) TapBegan(GestureSender(), EventArgs.Empty);
        }

        public event EventHandler TapEnded;
        public void OnTapEnded()
        {
            if (TapEnded != null) TapEnded(GestureSender(), EventArgs.Empty);
        }

        public event EventHandler TapMoved;
        public void OnTapMoved()
        {
            if (TapMoved != null)
                TapMoved(GestureSender(), EventArgs.Empty);
            else
                OnTapEnded();
        }
        #endregion

        #region LongTapGesture
        public event EventHandler LongTap;
        public void OnLongTap()
        {
            if (LongTap != null) LongTap(GestureSender(), EventArgs.Empty);
        }

        public event EventHandler LongTapBegan;
        public void OnLongTapBegan()
        {
            if (LongTapBegan != null) LongTapBegan(GestureSender(), EventArgs.Empty);
        }

        public event EventHandler LongTapEnded;
        public void OnLongTapEnded()
        {
            if (LongTapEnded != null) LongTapEnded(GestureSender(), EventArgs.Empty);
        }

        public event EventHandler LongTapMoved;
        public void OnLongTapMoved()
        {
            if (LongTapMoved != null) LongTapMoved(GestureSender(), EventArgs.Empty);
        }
        #endregion

        #region SwipeGesture
        public event EventHandler Swipe;
        public void OnSwipe()
        {
            if (Swipe != null) Swipe(GestureSender(), EventArgs.Empty);
        }

        public event EventHandler SwipeBegan;
        public void OnSwipeBegan()
        {
            if (SwipeBegan != null) SwipeBegan(GestureSender(), EventArgs.Empty);
        }

        public event EventHandler SwipeEnded;
        public void OnSwipeEnded()
        {
            if (SwipeEnded != null) SwipeEnded(GestureSender(), EventArgs.Empty);
        }

        public event EventHandler SwipeMoved;
        public void OnSwipeMoved()
        {
            if (SwipeMoved != null) SwipeMoved(GestureSender(), EventArgs.Empty);
        }

        #endregion
    }
}
