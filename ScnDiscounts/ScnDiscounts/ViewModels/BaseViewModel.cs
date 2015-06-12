//Базовый объект для ViewModel страницы:
//содержит реализацию жизненного цикла станицы;
//содержит реализацию патерна PropertyChanged.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

using ScnDiscounts;
using ScnDiscounts.Views.ContextUI;
using ScnDiscounts.Views;
using ScnDiscounts.Models;

namespace ScnDiscounts.ViewModels
{
    public class BaseViewModel : PropertyChangedPattern
    {
        protected App CurrentApp
        {
            get { return (App)Application.Current; }
        }

        //связанная страница UI
        protected Page ViewPage
        {
            get;
            private set;
        }

        //контекст связанной страницы UI
        public BaseContext Context
        {
            get;
            private set;
        }

        //текст кнопки Назад
        private string backTitle;

        //состояние загрузки (доступность потока) 
        #region IsLoading - property
        private bool _isLoading = false;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                IsNotLoading = !value;

                OnPropertyChanged();
            }
        }
        #endregion

        #region IsNotLoading - property
        private bool _isNotLoading = true;
        public bool IsNotLoading
        {
            get { return _isNotLoading; }
            set
            {
                _isNotLoading = value;
                OnPropertyChanged();
            }
        }

        #endregion

        //визуализация загруки с полной блокировкой окна
        #region IsLoadActivity - property
        private bool _isLoadActivity = false;
        public bool IsLoadActivity
        {
            get { return _isLoadActivity; }
            set
            {
                IsLoading = value;

                NavigationPage.SetHasNavigationBar(ViewPage, !IsLoading);

                ((IBasePage)ViewPage).UpdateLoadingGUI();

                _isLoadActivity = value;
                OnPropertyChanged();
            }
        }
        #endregion

        //визуализация загрузки в статус строке
        #region IsLoadBusy - property
        private bool _isLoadBusy = false;
        public bool IsLoadBusy
        {
            get { return _isLoadBusy; }
            set
            {
                IsLoading = value;

                var btnTitle = IsLoading ? Context.TxtAwait : backTitle;
                NavigationPage.SetBackButtonTitle(ViewPage, btnTitle);

                ((IBasePage)ViewPage).UpdateLoadingGUI();

                _isLoadBusy = value;
                OnPropertyChanged();
            }
        }
        #endregion

        //конструктор
        public BaseViewModel()
        { }

        #region Title - title
        public string Title
        {
            //            var title = (Device.OS == TargetPlatform.WinPhone) ? _context.Title.ToUpper() : _context.Title;
            get { return Context.Title; }
        }
        #endregion

        //установка связанной страницы UI
        public void SetPage(Page page, BaseContext baseContext)
        {
            ViewPage = page;
            Context = baseContext;

            //текст кнопки Назад
            backTitle = NavigationPage.GetBackButtonTitle(ViewPage);

            InitProperty();

            ViewPage.Appearing += ViewPage_Appearing;
            InitLifecycle();
        }
  
        //инициализация свойств
        protected virtual void InitProperty()
        {}

        void ViewPage_Appearing(object sender, EventArgs e)
        {
            ((IBasePage)ViewPage).UpdateLoadingGUI();
            OnResuming(this, EventArgs.Empty);

            OnPropertyChanged("Title"); 
        }

        #region Lifecycle
        protected void InitLifecycle()
        {
            CurrentApp.Resuming += OnResuming;
            CurrentApp.Suspending += OnSuspending;
        }

        protected void ClearLifecycle()
        {
            CurrentApp.Resuming -= OnResuming;
            CurrentApp.Suspending -= OnSuspending;
        }

        protected virtual void OnResuming(object sender, EventArgs e)
        {
        }

        protected virtual void OnSuspending(object sender, EventArgs e)
        {
        }
        #endregion
    }
}
