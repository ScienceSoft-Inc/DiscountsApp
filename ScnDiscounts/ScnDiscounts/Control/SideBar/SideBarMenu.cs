using System;
using System.Collections.Generic;

namespace ScnDiscounts.Control.SideBar
{
    public class SideBarMenu : SideBarPanel
    {
        private SideBarMenuItem activeMenu;
        public SideBarMenuItem ActiveMenu
        {
            get { return activeMenu; }
            set { activeMenu = value; }
        }

        private List<SideBarMenuItem> menuList;
    }

    public class SideBarMenuItem
    {
        #region Item icon
        private string icon = "";
        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }
        #endregion

        #region Item title
        private string title = "";
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        #endregion

        #region Item hint
        private string hint = "";
        public string Hint
        {
            get { return hint; }
            set { hint = value; }
        }
        #endregion

        #region Item page
        private Type contentPage;
        public Type ContentPage
        {
            get { return contentPage; }
            set { contentPage = value; }
        }
        #endregion
    }

}
