using System;
using ScnDiscounts.Helpers;
using Xamarin.Forms;
using ScnDiscounts.Control;

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

            //map source
            private string MapSourceParamName = "MapSource";
            private MapTile.TileSourceEnum _mapSource = MapTile.TileSourceEnum.tsNative;
            public MapTile.TileSourceEnum MapSource
            {
                get { return _mapSource; }
                set { _mapSource = value; }
            }

            public void Init()
            {
                LoadValue();
            }

            public void SaveValue()
            {
                Application.Current.Properties[SystemLangParamName] = SystemLang.ToString();
                Application.Current.Properties[MapSourceParamName] = MapSource.ToString();
            }

            public void LoadValue()
            {
                if (Application.Current.Properties.ContainsKey(SystemLangParamName))
                {
                    string lang = Application.Current.Properties[SystemLangParamName] as string;
                    LanguageHelper.LangTypeEnum tmpLang;
                    if (Enum.TryParse(lang, out tmpLang))
                        SystemLang = tmpLang;
                }

                if (Application.Current.Properties.ContainsKey(MapSourceParamName))
                {
                    string map = Application.Current.Properties[MapSourceParamName] as string;
                    MapTile.TileSourceEnum tmpMap;
                    if (Enum.TryParse(map, out tmpMap))
                        MapSource = tmpMap;
                }
            }
        }

        static public ConfigContainer Config = new ConfigContainer();
    }
}
