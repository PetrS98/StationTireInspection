using System;
using System.Collections.Generic;
using System.Text;

namespace StationTireInspection.Classes
{
    public enum Language { CZ, ENG, DE}

    public static class Translator
    {
        private static Language language = Language.ENG;
        public static event EventHandler<Language> LanguageChanged;

        public static Language Language
        {
            get 
            { 
                return language; 
            }
            set
            {
                language = value;
                LanguageChanged?.Invoke(null, value);
            }
        }

    }
}
