using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSettings.View
{
    public class DataViewHelper
    {

        public static bool TryGetColor(string colorName,out Color color)
        {
            color = Color.Empty;
            bool result = false;
            string text = colorName;
            if (text.StartsWith("#"))
            {
                try
                {
                    int r = GetComponent("r", text);
                    int g = GetComponent("g", text);
                    int b = GetComponent("b", text);
                    if (r != -1)
                        color = Color.FromArgb(r, g != -1 ? g : 0, b != -1 ? b : 0);
                    result = true;
                }
                catch 
                {
                    result = false;
                }

            }
            return result;
        }



        private static int GetComponent(string name, string text)
        {
            CultureInfo provider;
            provider = new CultureInfo("en-US");
            int result = -1;
            int offset = 0;
            if (text.StartsWith("#"))
            {
                switch (name.ToLower())
                {
                    case "r":
                        offset = 0;
                        break;
                    case "g":
                        offset = 2;
                        break;
                    case "b":
                        offset = 4;
                        break;

                }

                if (text.Length > 2 + offset)
                {
                    if (!int.TryParse(text.Substring(1 + offset, 2), NumberStyles.HexNumber, provider, out result))
                        result = -1;
                }
                else if (text.Length > 1 + offset)
                {
                    if (!int.TryParse(text.Substring(1 + offset, 1), NumberStyles.HexNumber, provider, out result))
                        result = -1;
                    else
                        result <<= 4;
                }
            }

            return result;

        }
    }
}
