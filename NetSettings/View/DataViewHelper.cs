using System;
using System.Globalization;
using System.Text.RegularExpressions;
using NetSettings.Common.Classes;

namespace NetSettings.View
{
    enum ColorRepresentanion { None, Hex, CommaSeperated }
    internal class DataViewHelper
    {

        public static bool TryGetColor(string colorName, out Color color)
        {
            color = Color.Empty;
            ColorRepresentanion colorRep = ClassifyColor(colorName);
            return TryParseColor(ref color, colorRep, colorName);
        }

        private static ColorRepresentanion ClassifyColor(string text)
        {
            ColorRepresentanion colorRep = ColorRepresentanion.None;
            if (text.Contains(","))
                colorRep = ColorRepresentanion.CommaSeperated;
            else
                colorRep = ColorRepresentanion.Hex;

            //if (text.StartsWith("#") || text.StartsWith("$") || IsHexLetters(text)) 
            //    colorRep = ColorRepresentanion.Hex;
            //colorRep = ColorRepresentanion.Hex;
            return colorRep;
        }

        private static bool TryParseColor(ref Color color, ColorRepresentanion colorRep, string text)
        {
            bool result;
            switch (colorRep)
            {
                case ColorRepresentanion.None:
                    result = false;
                    break;
                case ColorRepresentanion.Hex:
                    string hexRepresentation = GetHexNumber(text);
                    color = Color.FromHtml(hexRepresentation);
                    result = true;
                    break;
                case ColorRepresentanion.CommaSeperated:
                    result = TryParseCommaSeparatedColor(ref color, text);
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }

        private static bool TryParseCommaSeparatedColor(ref Color color, string text)
        {
            var numbers = text.Split(',');
            var result = false;
            if (numbers.Length <= 3)
            {
                int[] rgb = new int[3];

                var foundValidComponentFound = false;

                for (var i = 0; i < numbers.Length; i++)
                {
                    var s1 = GetNumbers(numbers[i]);
                    foundValidComponentFound |= (rgb[i] = int.TryParse(s1, out rgb[i]) ? rgb[i].Clamp(0, 255) : -1) != -1;
                }

                if (foundValidComponentFound)
                {
                    color = Color.FromArgb(rgb[0] != -1 ? rgb[0] : 0, rgb[1] != -1 ? rgb[1] : 0, rgb[2] != -1 ? rgb[2] : 0);
                    result = true;
                }
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
