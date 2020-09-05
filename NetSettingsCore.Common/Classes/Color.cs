using System;

namespace NetSettings.Common.Classes
{
    public readonly struct Color
    {
        #region Fields / Data Members
        private const long NotDefinedValue = 0;
        private const int ARGBAlphaShift = 24;
        private const int ARGBRedShift = 16;
        private const int ARGBGreenShift = 8;
        private const int ARGBBlueShift = 0;
        private const short StateARGBValueValid = 0x0002;
        private const short StateKnownColorValid = 0x0001;
        private const short StateValueMask = StateARGBValueValid;

        // User supplied name of color. Will not be filled in if
        // we map to a "knowncolor"
        private readonly string _name; // Do not rename (binary serialization)

        // Standard 32bit sRGB (ARGB)
        private readonly long _value; // Do not rename (binary serialization)

        // Ignored, unless "state" says it is valid
        private readonly short _knownColor; // Do not rename (binary serialization)

        // State flags.
        private readonly short _state; // Do not rename (binary serialization)

        public static readonly Color Empty = new Color();
        #endregion Fields / Data Members

        #region Properties
        private long Value =>
            // This is the only place we have system colors value exposed
            (_state & StateValueMask) != 0 ? _value : IsKnownColor ? KnownColorTable.KnownColorToArgb((KnownColor)_knownColor) : NotDefinedValue;

        public byte R => unchecked((byte)(Value >> ARGBRedShift));

        public byte G => unchecked((byte)(Value >> ARGBGreenShift));

        public byte B => unchecked((byte)(Value >> ARGBBlueShift));

        public byte A => unchecked((byte)(Value >> ARGBAlphaShift));

        public bool IsKnownColor => (_state & StateKnownColorValid) != 0;

        #region static list of "web" colors...
        public static Color Transparent => new Color(KnownColor.Transparent);

        public static Color AliceBlue => new Color(KnownColor.AliceBlue);

        public static Color AntiqueWhite => new Color(KnownColor.AntiqueWhite);

        public static Color Aqua => new Color(KnownColor.Aqua);

        public static Color Aquamarine => new Color(KnownColor.Aquamarine);

        public static Color Azure => new Color(KnownColor.Azure);

        public static Color Beige => new Color(KnownColor.Beige);

        public static Color Bisque => new Color(KnownColor.Bisque);

        public static Color Black => new Color(KnownColor.Black);

        public static Color BlanchedAlmond => new Color(KnownColor.BlanchedAlmond);

        public static Color Blue => new Color(KnownColor.Blue);

        public static Color BlueViolet => new Color(KnownColor.BlueViolet);

        public static Color Brown => new Color(KnownColor.Brown);

        public static Color BurlyWood => new Color(KnownColor.BurlyWood);

        public static Color CadetBlue => new Color(KnownColor.CadetBlue);

        public static Color Chartreuse => new Color(KnownColor.Chartreuse);

        public static Color Chocolate => new Color(KnownColor.Chocolate);

        public static Color Coral => new Color(KnownColor.Coral);

        public static Color CornflowerBlue => new Color(KnownColor.CornflowerBlue);

        public static Color Cornsilk => new Color(KnownColor.Cornsilk);

        public static Color Crimson => new Color(KnownColor.Crimson);

        public static Color Cyan => new Color(KnownColor.Cyan);

        public static Color DarkBlue => new Color(KnownColor.DarkBlue);

        public static Color DarkCyan => new Color(KnownColor.DarkCyan);

        public static Color DarkGoldenrod => new Color(KnownColor.DarkGoldenrod);

        public static Color DarkGray => new Color(KnownColor.DarkGray);

        public static Color DarkGreen => new Color(KnownColor.DarkGreen);

        public static Color DarkKhaki => new Color(KnownColor.DarkKhaki);

        public static Color DarkMagenta => new Color(KnownColor.DarkMagenta);

        public static Color DarkOliveGreen => new Color(KnownColor.DarkOliveGreen);

        public static Color DarkOrange => new Color(KnownColor.DarkOrange);

        public static Color DarkOrchid => new Color(KnownColor.DarkOrchid);

        public static Color DarkRed => new Color(KnownColor.DarkRed);

        public static Color DarkSalmon => new Color(KnownColor.DarkSalmon);

        public static Color DarkSeaGreen => new Color(KnownColor.DarkSeaGreen);

        public static Color DarkSlateBlue => new Color(KnownColor.DarkSlateBlue);

        public static Color DarkSlateGray => new Color(KnownColor.DarkSlateGray);

        public static Color DarkTurquoise => new Color(KnownColor.DarkTurquoise);

        public static Color DarkViolet => new Color(KnownColor.DarkViolet);

        public static Color DeepPink => new Color(KnownColor.DeepPink);

        public static Color DeepSkyBlue => new Color(KnownColor.DeepSkyBlue);

        public static Color DimGray => new Color(KnownColor.DimGray);

        public static Color DodgerBlue => new Color(KnownColor.DodgerBlue);

        public static Color Firebrick => new Color(KnownColor.Firebrick);

        public static Color FloralWhite => new Color(KnownColor.FloralWhite);

        public static Color ForestGreen => new Color(KnownColor.ForestGreen);

        public static Color Fuchsia => new Color(KnownColor.Fuchsia);

        public static Color Gainsboro => new Color(KnownColor.Gainsboro);

        public static Color GhostWhite => new Color(KnownColor.GhostWhite);

        public static Color Gold => new Color(KnownColor.Gold);

        public static Color Goldenrod => new Color(KnownColor.Goldenrod);

        public static Color Gray => new Color(KnownColor.Gray);

        public static Color Green => new Color(KnownColor.Green);

        public static Color GreenYellow => new Color(KnownColor.GreenYellow);

        public static Color Honeydew => new Color(KnownColor.Honeydew);

        public static Color HotPink => new Color(KnownColor.HotPink);

        public static Color IndianRed => new Color(KnownColor.IndianRed);

        public static Color Indigo => new Color(KnownColor.Indigo);

        public static Color Ivory => new Color(KnownColor.Ivory);

        public static Color Khaki => new Color(KnownColor.Khaki);

        public static Color Lavender => new Color(KnownColor.Lavender);

        public static Color LavenderBlush => new Color(KnownColor.LavenderBlush);

        public static Color LawnGreen => new Color(KnownColor.LawnGreen);

        public static Color LemonChiffon => new Color(KnownColor.LemonChiffon);

        public static Color LightBlue => new Color(KnownColor.LightBlue);

        public static Color LightCoral => new Color(KnownColor.LightCoral);

        public static Color LightCyan => new Color(KnownColor.LightCyan);

        public static Color LightGoldenrodYellow => new Color(KnownColor.LightGoldenrodYellow);

        public static Color LightGreen => new Color(KnownColor.LightGreen);

        public static Color LightGray => new Color(KnownColor.LightGray);

        public static Color LightPink => new Color(KnownColor.LightPink);

        public static Color LightSalmon => new Color(KnownColor.LightSalmon);

        public static Color LightSeaGreen => new Color(KnownColor.LightSeaGreen);

        public static Color LightSkyBlue => new Color(KnownColor.LightSkyBlue);

        public static Color LightSlateGray => new Color(KnownColor.LightSlateGray);

        public static Color LightSteelBlue => new Color(KnownColor.LightSteelBlue);

        public static Color LightYellow => new Color(KnownColor.LightYellow);

        public static Color Lime => new Color(KnownColor.Lime);

        public static Color LimeGreen => new Color(KnownColor.LimeGreen);

        public static Color Linen => new Color(KnownColor.Linen);

        public static Color Magenta => new Color(KnownColor.Magenta);

        public static Color Maroon => new Color(KnownColor.Maroon);

        public static Color MediumAquamarine => new Color(KnownColor.MediumAquamarine);

        public static Color MediumBlue => new Color(KnownColor.MediumBlue);

        public static Color MediumOrchid => new Color(KnownColor.MediumOrchid);

        public static Color MediumPurple => new Color(KnownColor.MediumPurple);

        public static Color MediumSeaGreen => new Color(KnownColor.MediumSeaGreen);

        public static Color MediumSlateBlue => new Color(KnownColor.MediumSlateBlue);

        public static Color MediumSpringGreen => new Color(KnownColor.MediumSpringGreen);

        public static Color MediumTurquoise => new Color(KnownColor.MediumTurquoise);

        public static Color MediumVioletRed => new Color(KnownColor.MediumVioletRed);

        public static Color MidnightBlue => new Color(KnownColor.MidnightBlue);

        public static Color MintCream => new Color(KnownColor.MintCream);

        public static Color MistyRose => new Color(KnownColor.MistyRose);

        public static Color Moccasin => new Color(KnownColor.Moccasin);

        public static Color NavajoWhite => new Color(KnownColor.NavajoWhite);

        public static Color Navy => new Color(KnownColor.Navy);

        public static Color OldLace => new Color(KnownColor.OldLace);

        public static Color Olive => new Color(KnownColor.Olive);

        public static Color OliveDrab => new Color(KnownColor.OliveDrab);

        public static Color Orange => new Color(KnownColor.Orange);

        public static Color OrangeRed => new Color(KnownColor.OrangeRed);

        public static Color Orchid => new Color(KnownColor.Orchid);

        public static Color PaleGoldenrod => new Color(KnownColor.PaleGoldenrod);

        public static Color PaleGreen => new Color(KnownColor.PaleGreen);

        public static Color PaleTurquoise => new Color(KnownColor.PaleTurquoise);

        public static Color PaleVioletRed => new Color(KnownColor.PaleVioletRed);

        public static Color PapayaWhip => new Color(KnownColor.PapayaWhip);

        public static Color PeachPuff => new Color(KnownColor.PeachPuff);

        public static Color Peru => new Color(KnownColor.Peru);

        public static Color Pink => new Color(KnownColor.Pink);

        public static Color Plum => new Color(KnownColor.Plum);

        public static Color PowderBlue => new Color(KnownColor.PowderBlue);

        public static Color Purple => new Color(KnownColor.Purple);

        public static Color Red => new Color(KnownColor.Red);

        public static Color RosyBrown => new Color(KnownColor.RosyBrown);

        public static Color RoyalBlue => new Color(KnownColor.RoyalBlue);

        public static Color SaddleBrown => new Color(KnownColor.SaddleBrown);

        public static Color Salmon => new Color(KnownColor.Salmon);

        public static Color SandyBrown => new Color(KnownColor.SandyBrown);

        public static Color SeaGreen => new Color(KnownColor.SeaGreen);

        public static Color SeaShell => new Color(KnownColor.SeaShell);

        public static Color Sienna => new Color(KnownColor.Sienna);

        public static Color Silver => new Color(KnownColor.Silver);

        public static Color SkyBlue => new Color(KnownColor.SkyBlue);

        public static Color SlateBlue => new Color(KnownColor.SlateBlue);

        public static Color SlateGray => new Color(KnownColor.SlateGray);

        public static Color Snow => new Color(KnownColor.Snow);

        public static Color SpringGreen => new Color(KnownColor.SpringGreen);

        public static Color SteelBlue => new Color(KnownColor.SteelBlue);

        public static Color Tan => new Color(KnownColor.Tan);

        public static Color Teal => new Color(KnownColor.Teal);

        public static Color Thistle => new Color(KnownColor.Thistle);

        public static Color Tomato => new Color(KnownColor.Tomato);

        public static Color Turquoise => new Color(KnownColor.Turquoise);

        public static Color Violet => new Color(KnownColor.Violet);

        public static Color Wheat => new Color(KnownColor.Wheat);

        public static Color White => new Color(KnownColor.White);

        public static Color WhiteSmoke => new Color(KnownColor.WhiteSmoke);

        public static Color Yellow => new Color(KnownColor.Yellow);

        public static Color YellowGreen => new Color(KnownColor.YellowGreen);
        #endregion web colors
        #endregion Properties

        #region Methods
        internal static bool IsKnownColorSystem(KnownColor knownColor)
            => (knownColor <= KnownColor.WindowText) || (knownColor > KnownColor.YellowGreen);

        private static int _parse_col4(string color, int offset)
        {
            char character = color[offset];

            if (character >= '0' && character <= '9')
            {
                return character - '0';
            }
            else if (character >= 'a' && character <= 'f')
            {
                return character + (10 - 'a');
            }
            else if (character >= 'A' && character <= 'F')
            {
                return character + (10 - 'A');
            }
            return -1;
        }

        private static int _parse_col8(string color, int offset)
        {
            return _parse_col4(color, offset) * 16 + _parse_col4(color, offset + 1);
        }

        private static Color FromHtml(string color)
        {
            if (color.Length == 0)
            {
                return new Color();
            }
            if (color[0] == '#')
            {
                color = color.Substring(1);
            }

            // If enabled, use 1 hex digit per channel instead of 2.
            // Other sizes aren't in the HTML/CSS spec but we could add them if desired.
            var alpha = color.Length switch
            {
                8 => true,
                6 => false,
                4 => true,
                3 => false,
                _ => throw new Exception("Invalid color code: " + color + ".")
            };

            var isShorthand = color.Length < 5;
            //double r, g, b, a = 1.0;
            int r, g, b, a = 255;
            if (isShorthand)
            {
                //r = _parse_col4(color, 0) / 15.0;
                //g = _parse_col4(color, 1) / 15.0;
                //b = _parse_col4(color, 2) / 15.0;
                //if (alpha)
                //{
                //    a = _parse_col4(color, 3) / 15.0;
                //}
                r = _parse_col4(color, 0);
                g = _parse_col4(color, 1);
                b = _parse_col4(color, 2);
                if (alpha)
                {
                    a = _parse_col4(color, 3);
                }
            }
            else
            {
                //r = _parse_col8(color, 0) / 255.0;
                //g = _parse_col8(color, 2) / 255.0;
                //b = _parse_col8(color, 4) / 255.0;
                //if (alpha)
                //{
                //    a = _parse_col8(color, 6) / 255.0;
                //}
                r = _parse_col8(color, 0);
                g = _parse_col8(color, 2);
                b = _parse_col8(color, 4);
                if (alpha)
                {
                    a = _parse_col8(color, 6);
                }
            }

            if (r < 0)
            {
                throw new Exception("Invalid color code: " + color + ".");
            }

            if (g < 0)
            {
                throw new Exception("Invalid color code: " + color + ".");
            }

            if (b < 0)
            {
                throw new Exception("Invalid color code: " + color + ".");
            }

            if (a < 0)
            {
                throw new Exception("Invalid color code: " + color + ".");
            }

            //ERR_FAIL_COND_V_MSG(r < 0, Color(), "Invalid color code: " + p_rgba + ".");
            //ERR_FAIL_COND_V_MSG(g < 0, Color(), "Invalid color code: " + p_rgba + ".");
            //ERR_FAIL_COND_V_MSG(b < 0, Color(), "Invalid color code: " + p_rgba + ".");
            //ERR_FAIL_COND_V_MSG(a < 0, Color(), "Invalid color code: " + p_rgba + ".");

            return Color.FromArgb(a, r, g, b);
        }

        private bool IsHtmlColorValid(string color)
        {
            if (color.Length == 0)
            {
                return false;
            }
            if (color[0] == '#')
            {
                color = color.Substring(1);
            }

            // Check if the amount of hex digits is valid.
            int len = color.Length;
            if (!(len == 3 || len == 4 || len == 6 || len == 8))
            {
                return false;
            }

            // Check if each hex digit is valid.
            for (int i = 0; i < len; i++)
            {
                if (_parse_col4(color, i) == -1)
                {
                    return false;
                }
            }

            return true;
        }

        public static Color Parse(string htmlColor)
        {
            Color result;
            if (Enum.TryParse(htmlColor, out KnownColor knownColor))
            {
                result = new Color(knownColor);
            }
            else if (htmlColor.Split(',').Length > 1)
            {
                throw new NotImplementedException("");
                if (false)
                {
                    var values = htmlColor.Split(',');
                    switch (values.Length)
                    {
                        case 1:
                            var value = values[0];
                            if (value.Length == 7 && value.StartsWith("#"))
                            {
                                //"#E3D51C"
                            }

                            break;
                        case 3:
                            result = FromArgb(255, byte.Parse(values[0]), byte.Parse(values[1]), byte.Parse(values[2]));
                            break;
                        case 4:
                            result = FromArgb(byte.Parse(values[0]), byte.Parse(values[1]), byte.Parse(values[2]),
                                byte.Parse(values[3]));
                            break;
                        default:
                            throw new ArgumentException();
                    }
                }
            }
            else
            {
                result = FromHtml(htmlColor);
            }

            return result;
        }

        private Color(KnownColor knownColor)
        {
            _value = 0;
            _state = StateKnownColorValid;
            _name = null;
            this._knownColor = unchecked((short)knownColor);
        }

        private Color(long value, short state, string name, KnownColor knownColor)
        {
            this._value = value;
            this._state = state;
            this._name = name;
            this._knownColor = unchecked((short)knownColor);
        }

        public static Color FromArgb(int red, int green, int blue) => FromArgb(byte.MaxValue, red, green, blue);

        private static Color FromArgb(uint argb) => new Color(argb, StateARGBValueValid, null, (KnownColor)0);

        public static Color FromArgb(int alpha, int red, int green, int blue)
        {
            CheckByte(alpha, nameof(alpha));
            CheckByte(red, nameof(red));
            CheckByte(green, nameof(green));
            CheckByte(blue, nameof(blue));

            return FromArgb(
                (uint)alpha << ARGBAlphaShift |
                (uint)red << ARGBRedShift |
                (uint)green << ARGBGreenShift |
                (uint)blue << ARGBBlueShift
            );
        }

        private static void CheckByte(int value, string name)
        {
            if (unchecked((uint)value) > byte.MaxValue)
            {
                throw new ArgumentException(
                    $"Check the value of the byte. Accepted values: {byte.MinValue} - {byte.MaxValue}. Actual values: value: {value}, name: {name}.");
            }
        }
        #endregion Methods
    }
}