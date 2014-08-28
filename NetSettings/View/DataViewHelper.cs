using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace NetSettings.View
{
    enum ColorRepresentanion { None, Hex, CommaSeperated}
    public class DataViewHelper
    {

        public static bool TryGetColor(string colorName,out Color color)
        {
            ColorRepresentanion colorRep = ColorRepresentanion.None;
            color = Color.Empty;
            bool result = false;
            string text = colorName;

            string[] numbers = null;

            if (text.StartsWith("#") || text.StartsWith("$") || IsHexLetters(text)) //100% hex
                colorRep = ColorRepresentanion.Hex;
            else if (text.Contains(','))
            {
                colorRep = ColorRepresentanion.CommaSeperated;
                numbers = text.Split(',');
            }
            else
                colorRep = ColorRepresentanion.Hex;

            //else
            //{
            //    if (numbers.Length > 1) //100% RGB
            //    {
            //        colorRep = ColorRepresentanion.CommaSeperated;
            //    }
            //    else /// check further
            //    {
            //        int num;
            //        if (numbers.Length == 1 && int.TryParse(numbers[0], out num) && num > 255)
            //            colorRep = ColorRepresentanion.Hex;

            //    }
            //}

            switch (colorRep)
            {
                case ColorRepresentanion.None:
                    result = false;
                    break;
                case ColorRepresentanion.Hex:
                    string hexRepresentation = GetHexNumber(text);
                    color = ColorTranslator.FromHtml(hexRepresentation);
                    result = true;
                    break;
                case ColorRepresentanion.CommaSeperated:
                      int []rgb = new int[3];
                
                for (int i = 0 ; i <numbers.Length; i++ )
                {
                    string s1 = GetNumbers(numbers[i]);
                    if (int.TryParse(s1,out rgb[i]))
                    {
                        rgb[i] = rgb[i].Clamp(0, 255);
                    }
                    else
                       rgb[i] = -1;
                }


                if (rgb[0] != -1)
                {

                    color = Color.FromArgb(rgb[0], rgb[1] != -1 ? rgb[1] : 0, rgb[2] != -1 ? rgb[2] : 0);
                    result = true;
                }
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }

        private static bool IsHexLetters(string s)
        {
            string resultString = null;
            try
            {
                Regex regexObj = new Regex(@"([a-f]|[A-F])");
                resultString = regexObj.Match(s).Value;// Replace(s, "");
                
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            return !String.IsNullOrEmpty(resultString);
        }

        private static string GetHexNumber(string s)
        {
            string resultString = null;
            try
            {
                Regex regexObj = new Regex(@"([a-f]+|[A-F]+|[0-9]+)+");
                resultString = regexObj.Match(s).Value;
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            if (resultString.Length > 6)
                resultString = resultString.Substring(0, 6);

            if (resultString.Length < 6)
                resultString = resultString.PadRight(6, '0');


            return "#" + resultString;
        }


        private static string GetNumbers(string s)
        {
            string resultString = null;
            try
            {
                Regex regexObj = new Regex(@"[^\d]");
                resultString = regexObj.Replace(s, "");
            }
            catch (ArgumentException ex)
            {
                    // Syntax error in the regular expression
            }
            return resultString;
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
