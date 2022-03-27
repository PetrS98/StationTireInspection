using System;
using System.Collections.Generic;
using System.Text;

namespace StationTireInspection.Classes
{
    public class StringHelper
    {
        public static bool CheckIfTextIsNumber(string Text)
        {
            try
            {
                int.Parse(Text);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SearchTextInString(string Text, string SearchKey) 
        {
            return Text.Contains(SearchKey);
        }

    }
}
