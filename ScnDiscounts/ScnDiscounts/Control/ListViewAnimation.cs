using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ScnDiscounts.Control
{
    public class ListViewAnimation : ListView
    {
        public ListViewAnimation()
            :base()
        {
            animationList = new List<View>();
            ItemTapped += ListViewAnimation_ItemTapped;
        }

        void ListViewAnimation_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var index = 0;
            foreach (var item in ItemsSource)
            {
                index++;
                if (item == e.Item)
                    break;
            }

            if (index >= 5)
            {
                topAnimateLine = index - 5;
                bottomAnimateLine = index + 5;
            }
            else
            {
                topAnimateLine = 0;
                bottomAnimateLine = 10;
            }
        }

        private List<View> animationList;
        public void AnimationListAdd(View view)
        {
            if (!isListShowed)
                view.RotationX = 90;

            animationList.Add(view);
        }

        public event EventHandler AnimationFinished;
        private void OnAnimationFinish()
        {
            if (AnimationFinished != null) AnimationFinished(this, EventArgs.Empty);
        }

        private int topAnimateLine = 0;
        private int bottomAnimateLine = 10;

        private bool isListShowed = false;
        async public Task ShowAnimation()
        {
            isListShowed = false;
            for (var i = 0; i < animationList.Count; i++)
            {
                var item = animationList[i];
                
                if (item.RotationX != 0)
                {

                    if ((topAnimateLine <= i) && (i <= bottomAnimateLine))
                    {
                        item.RotateXTo(0, 120, Easing.SinIn);
                        await Task.Delay(40);
                    }
                    else
                        item.RotationX = 0;
                }
            }
            isListShowed = true;
            
            OnAnimationFinish();
        }

        private bool isListHidden = false;
        async public void HideAnimation()
        {
            isListHidden = false;

            for (var i = 0; i < animationList.Count; i++)
            {
                var item = animationList[i];

                if ((topAnimateLine <= i) && (i <= bottomAnimateLine))
                {
                    item.RotateXTo(90, 100, Easing.SinIn);
                    await Task.Delay(40);
                }
                else
                    item.RotationX = 90;
            }
            isListHidden = true;

            OnAnimationFinish();
        }
    }
}
