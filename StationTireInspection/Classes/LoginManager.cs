using System;
using System.Collections.Generic;
using System.Text;

namespace StationTireInspection.Classes
{
    class LoginManager
    {
        public static event EventHandler<bool> LogedChanged;

        private static bool logedIn;

        public static bool LogedIn
        {
            get 
            { 
                return logedIn; 
            }
            set 
            { 
                logedIn = value;
                LogedChanged?.Invoke(null, value);
            }
        }

    }
}
