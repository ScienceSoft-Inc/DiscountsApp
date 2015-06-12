using System;
using ScnDiscounts.Helpers;
using Xamarin.Forms;

namespace ScnDiscounts.Models
{
    static public class AppParameters
    {
        public class ConfigContainer
        {
            //system (phone) language
            private string SystemLangParamName = "SystemLang";
            private LanguageHelper.LangTypeEnum _systemLang = LanguageHelper.LangTypeEnum.ltEn;
            public LanguageHelper.LangTypeEnum SystemLang 
            { 
                get { return _systemLang; }
                set { _systemLang = value; } 
            }

            //push notification
            /*private bool _isGetPushNotification = false;
            public bool IsGetPushNotification 
            {
                get { return _isGetPushNotification; }
                set { _isGetPushNotification = value; }
            }*/

            public void Init()
            {
                LoadValue();
            }

            public void SaveValue()
            {
                Application.Current.Properties["SystemLangParamName"] = SystemLang.ToString();
            }

            public void LoadValue()
            {
                if (Application.Current.Properties.ContainsKey("SystemLangParamName"))
                {
                    string lang = Application.Current.Properties["SystemLangParamName"] as string;
                    LanguageHelper.LangTypeEnum tmpLang;
                    if (Enum.TryParse(lang, out tmpLang))
                        SystemLang = tmpLang;
                }
            }
        }

        static public ConfigContainer Config = new ConfigContainer();
    }
}
