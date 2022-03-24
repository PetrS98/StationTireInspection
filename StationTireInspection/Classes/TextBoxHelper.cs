using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace StationTireInspection.Classes
{
    public class TextBoxHelper
    {
        public static bool TbInputIsNumber(TextBox textBox)
        {
            return int.TryParse(textBox.Text, out int result);
        }

        public static bool TbInputIsText(TextBox textBox)
        {
            for (int i = 0; i < textBox.Text.Length; i++)
            {
                if (IsLetter(textBox.Text[i])) continue;
                return false;
            }

            if (textBox.Text == null || textBox.Text == "")
            {
                return false;
            }

            return true;
        }

        private static bool IsLetter(char c)
        {
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_".IndexOf(c) != -1;
            //return "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzĚŠČŘŽÝÁÍÉÚŮÓěščřžýáíéúůóß".IndexOf(c) != -1;
        }
    }
}
