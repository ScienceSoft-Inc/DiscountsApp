using System;
using ScnDiscounts.Views;
using ScnDiscounts.Views.Styles;
using Xamarin.Forms;

namespace ScnDiscounts
{
    public class App : Application
    {
        public App()
        {
            new MainStyles();
            new LabelStyles();

            MainPage = new SplashPage();
        }

        #region OnStart
        protected override void OnStart()
        {
            OnStarting(EventArgs.Empty);
        }

        public event EventHandler Starting;

        protected virtual void OnStarting(EventArgs e)
        {
            if (Starting != null) Starting(this, e);
        }
        #endregion

        #region OnSleep
        protected override void OnSleep()
        {
            OnSuspending(EventArgs.Empty);
        }

        public event EventHandler Suspending;

        protected virtual void OnSuspending(EventArgs e)
        {
            if (Suspending != null) Suspending(this, e);
        }
        #endregion

        #region OnResume
        protected override void OnResume()
        {
            OnResuming(EventArgs.Empty);
        }

        public event EventHandler Resuming;

        private void OnResuming(EventArgs e)
        {
            if (Resuming != null) Resuming(this, e);
        }
        #endregion
    }
}
