using System;
using System.Collections.Generic;
using System.Text;
using ScnDiscounts.Helpers;
using ScnDiscounts.Views.ContentUI;
using Xamarin.Forms;
using ScnDiscounts.Views.Styles;

namespace ScnDiscounts.Models
{
    public class FilterCategoryItem : NotifyPropertyChanged
    {
        public FilterCategoryItem()
        {
        }

        //service field
        private int _id = 0;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        //service field
        private string _paramName = "";
        public string ParamName
        {
            get { return _paramName; }
            set { _paramName = value; }
        }

        public string Icon
        {
            get
            {
                if (IsToggle)
                {
                    if (CategoryHelper.CategoryList.ContainsKey(_id))
                        return CategoryHelper.CategoryList[_id].Icon;
                    else
                        return "";
                }
                else
                    return CategoryHelper.ic_pin_disabled;
            }
        }

        public string Name
        {
            get 
            {
                if (CategoryHelper.CategoryList.ContainsKey(_id))
                    return CategoryHelper.CategoryList[_id].Name;
                else
                    return "empty";
            }
        }

        public Style NameStyle
        {
            get
            {
                if (IsToggle)
                    return (Style)App.Current.Resources[LabelStyles.MenuStyle];
                else
                    return (Style)App.Current.Resources[LabelStyles.MenuDisabledStyle];
            }
        }

        private bool _isToggle = true;
        public bool IsToggle
        {
            get { return _isToggle; }
            set 
            {
                _isToggle = value;
                OnPropertyChanged();
                OnPropertyChanged("ToggleValue");
                OnPropertyChanged("Icon");
                OnPropertyChanged("NameStyle");
            }
        }

        public string TurnOnValue
        {
            get
            {
                var content = new RootContentUI();
                return content.TxtTurnOn;
            }
        }

        public string TurnOffValue
        {
            get
            {
                var content = new RootContentUI();
                return content.TxtTurnOff;
            }
        }
    }
}
