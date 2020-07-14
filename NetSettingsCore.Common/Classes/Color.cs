using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace NetSettingsCore.Common.Classes
{
    public struct Color
    {
        private const long NotDefinedValue = 0;

        internal static bool IsKnownColorSystem(KnownColor knownColor)
            => (knownColor <= KnownColor.WindowText) || (knownColor > KnownColor.YellowGreen);

        private long Value
        {
            get
            {
                if ((state & StateValueMask) != 0)
                {
                    return value;
                }

                // This is the only place we have system colors value exposed
                if (IsKnownColor)
                {
                    return KnownColorTable.KnownColorToArgb((KnownColor)knownColor);
                }

                return NotDefinedValue;
            }
        }


        public byte R => unchecked((byte)(Value >> ARGBRedShift));

        public byte G => unchecked((byte)(Value >> ARGBGreenShift));

        public byte B => unchecked((byte)(Value >> ARGBBlueShift));

        public byte A => unchecked((byte)(Value >> ARGBAlphaShift));

        public bool IsKnownColor => (state & StateKnownColorValid) != 0;

        internal const int ARGBAlphaShift = 24;
        internal const int ARGBRedShift = 16;
        internal const int ARGBGreenShift = 8;
        internal const int ARGBBlueShift = 0;
        private const short StateARGBValueValid = 0x0002;
        private const short StateKnownColorValid = 0x0001;
        private const short StateValueMask = StateARGBValueValid;

        // User supplied name of color. Will not be filled in if
        // we map to a "knowncolor"
        private readonly string name; // Do not rename (binary serialization)

        // Standard 32bit sRGB (ARGB)
        private readonly long value; // Do not rename (binary serialization)

        // Ignored, unless "state" says it is valid
        private readonly short knownColor; // Do not rename (binary serialization)

        // State flags.
        private readonly short state; // Do not rename (binary serialization)

        public static readonly Color Empty = new Color();

        // -------------------------------------------------------------------
        //  static list of "web" colors...
        //
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

        public static Color FromHtml(string htmlColor)
        {
            KnownColor knownColor;
            Color result;
            if (Enum.TryParse(htmlColor, out knownColor))
            {
                result = new Color(knownColor);
            }
            else
            {
                var values = htmlColor.Split(',');
                switch (values.Length)
                {
                    case 4:
                        result = FromArgb(byte.Parse(values[0]), byte.Parse(values[1]), byte.Parse(values[2]), byte.Parse(values[3]));
                        break;
                    case 3:
                        result = FromArgb(255, byte.Parse(values[0]), byte.Parse(values[1]), byte.Parse(values[2]));
                        break;
                    default:
                        throw new ArgumentException();
                }
            }

            return result;
        }

        internal Color(KnownColor knownColor)
        {
            value = 0;
            state = StateKnownColorValid;
            name = null;
            this.knownColor = unchecked((short)knownColor);
        }

        private Color(long value, short state, string name, KnownColor knownColor)
        {
            this.value = value;
            this.state = state;
            this.name = name;
            this.knownColor = unchecked((short)knownColor);
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
            void ThrowOutOfByteRange(int v, string n) =>
                //throw new ArgumentException(SR.Format(SR.InvalidEx2BoundArgument, n, v, byte.MinValue, byte.MaxValue));
                throw new ArgumentException("CheckByte");

            if (unchecked((uint)value) > byte.MaxValue)
                ThrowOutOfByteRange(value, name);
        }
        //int ToArgb();
        ////Color FromArgb(double r, double g, double b);
        //Color FromArgb(int r, int g, int b);
    }

    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    //#if netcoreapp20
    internal
    //#else
    //public
    //#endif
    enum KnownColor
    {
        // This enum is order dependent!!!
        //
        // The value of these known colors are indexes into a color array.
        // Do not modify this enum without updating KnownColorTable.

        // 0 - reserved for "not a known color"

        // "System" colors
        ActiveBorder = 1,
        ActiveCaption,
        ActiveCaptionText,
        AppWorkspace,
        Control,
        ControlDark,
        ControlDarkDark,
        ControlLight,
        ControlLightLight,
        ControlText,
        Desktop,
        GrayText,
        Highlight,
        HighlightText,
        HotTrack,
        InactiveBorder,
        InactiveCaption,
        InactiveCaptionText,
        Info,
        InfoText,
        Menu,
        MenuText,
        ScrollBar,
        Window,
        WindowFrame,
        WindowText,

        // "Web" Colors
        Transparent,
        AliceBlue,
        AntiqueWhite,
        Aqua,
        Aquamarine,
        Azure,
        Beige,
        Bisque,
        Black,
        BlanchedAlmond,
        Blue,
        BlueViolet,
        Brown,
        BurlyWood,
        CadetBlue,
        Chartreuse,
        Chocolate,
        Coral,
        CornflowerBlue,
        Cornsilk,
        Crimson,
        Cyan,
        DarkBlue,
        DarkCyan,
        DarkGoldenrod,
        DarkGray,
        DarkGreen,
        DarkKhaki,
        DarkMagenta,
        DarkOliveGreen,
        DarkOrange,
        DarkOrchid,
        DarkRed,
        DarkSalmon,
        DarkSeaGreen,
        DarkSlateBlue,
        DarkSlateGray,
        DarkTurquoise,
        DarkViolet,
        DeepPink,
        DeepSkyBlue,
        DimGray,
        DodgerBlue,
        Firebrick,
        FloralWhite,
        ForestGreen,
        Fuchsia,
        Gainsboro,
        GhostWhite,
        Gold,
        Goldenrod,
        Gray,
        Green,
        GreenYellow,
        Honeydew,
        HotPink,
        IndianRed,
        Indigo,
        Ivory,
        Khaki,
        Lavender,
        LavenderBlush,
        LawnGreen,
        LemonChiffon,
        LightBlue,
        LightCoral,
        LightCyan,
        LightGoldenrodYellow,
        LightGray,
        LightGreen,
        LightPink,
        LightSalmon,
        LightSeaGreen,
        LightSkyBlue,
        LightSlateGray,
        LightSteelBlue,
        LightYellow,
        Lime,
        LimeGreen,
        Linen,
        Magenta,
        Maroon,
        MediumAquamarine,
        MediumBlue,
        MediumOrchid,
        MediumPurple,
        MediumSeaGreen,
        MediumSlateBlue,
        MediumSpringGreen,
        MediumTurquoise,
        MediumVioletRed,
        MidnightBlue,
        MintCream,
        MistyRose,
        Moccasin,
        NavajoWhite,
        Navy,
        OldLace,
        Olive,
        OliveDrab,
        Orange,
        OrangeRed,
        Orchid,
        PaleGoldenrod,
        PaleGreen,
        PaleTurquoise,
        PaleVioletRed,
        PapayaWhip,
        PeachPuff,
        Peru,
        Pink,
        Plum,
        PowderBlue,
        Purple,
        Red,
        RosyBrown,
        RoyalBlue,
        SaddleBrown,
        Salmon,
        SandyBrown,
        SeaGreen,
        SeaShell,
        Sienna,
        Silver,
        SkyBlue,
        SlateBlue,
        SlateGray,
        Snow,
        SpringGreen,
        SteelBlue,
        Tan,
        Teal,
        Thistle,
        Tomato,
        Turquoise,
        Violet,
        Wheat,
        White,
        WhiteSmoke,
        Yellow,
        YellowGreen,

        // New system color additions in Visual Studio 2005 (.NET Framework 2.0)

        ButtonFace,
        ButtonHighlight,
        ButtonShadow,
        GradientActiveCaption,
        GradientInactiveCaption,
        MenuBar,
        MenuHighlight
    }

    internal static class KnownColorTable
    {
        private static readonly uint[] s_colorTable = new uint[141]
        {
      16777215U,
      4293982463U,
      4294634455U,
      4278255615U,
      4286578644U,
      4293984255U,
      4294309340U,
      4294960324U,
      4278190080U,
      4294962125U,
      4278190335U,
      4287245282U,
      4289014314U,
      4292786311U,
      4284456608U,
      4286578432U,
      4291979550U,
      4294934352U,
      4284782061U,
      4294965468U,
      4292613180U,
      4278255615U,
      4278190219U,
      4278225803U,
      4290283019U,
      4289309097U,
      4278215680U,
      4290623339U,
      4287299723U,
      4283788079U,
      4294937600U,
      4288230092U,
      4287299584U,
      4293498490U,
      4287609995U,
      4282924427U,
      4281290575U,
      4278243025U,
      4287889619U,
      4294907027U,
      4278239231U,
      4285098345U,
      4280193279U,
      4289864226U,
      4294966000U,
      4280453922U,
      4294902015U,
      4292664540U,
      4294506751U,
      4294956800U,
      4292519200U,
      4286611584U,
      4278222848U,
      4289593135U,
      4293984240U,
      4294928820U,
      4291648604U,
      4283105410U,
      4294967280U,
      4293977740U,
      4293322490U,
      4294963445U,
      4286381056U,
      4294965965U,
      4289583334U,
      4293951616U,
      4292935679U,
      4294638290U,
      4292072403U,
      4287688336U,
      4294948545U,
      4294942842U,
      4280332970U,
      4287090426U,
      4286023833U,
      4289774814U,
      4294967264U,
      4278255360U,
      4281519410U,
      4294635750U,
      4294902015U,
      4286578688U,
      4284927402U,
      4278190285U,
      4290401747U,
      4287852763U,
      4282168177U,
      4286277870U,
      4278254234U,
      4282962380U,
      4291237253U,
      4279834992U,
      4294311930U,
      4294960353U,
      4294960309U,
      4294958765U,
      4278190208U,
      4294833638U,
      4286611456U,
      4285238819U,
      4294944000U,
      4294919424U,
      4292505814U,
      4293847210U,
      4288215960U,
      4289720046U,
      4292571283U,
      4294963157U,
      4294957753U,
      4291659071U,
      4294951115U,
      4292714717U,
      4289781990U,
      4286578816U,
      4294901760U,
      4290547599U,
      4282477025U,
      4287317267U,
      4294606962U,
      4294222944U,
      4281240407U,
      4294964718U,
      4288696877U,
      4290822336U,
      4287090411U,
      4285160141U,
      4285563024U,
      4294966010U,
      4278255487U,
      4282811060U,
      4291998860U,
      4278222976U,
      4292394968U,
      4294927175U,
      4282441936U,
      4293821166U,
      4294303411U,
      uint.MaxValue,
      4294309365U,
      4294967040U,
      4288335154U
        };

        //internal static Color ArgbToKnownColor(uint argb)
        //{
        //    for (int index = 1; index < KnownColorTable.s_colorTable.Length; ++index)
        //    {
        //        if ((int)KnownColorTable.s_colorTable[index] == (int)argb)
        //            return Color.FromKnownColor((KnownColor)(index + 27));
        //    }
        //    return Color.FromArgb((int)argb);
        //}



        public static uint KnownColorToArgb(KnownColor color)
        {
            return !Color.IsKnownColorSystem(color) ? KnownColorTable.s_colorTable[(int)(color - 27)] : KnownColorTable.GetSystemColorArgb(color);
        }

        //private static unsafe ReadOnlySpan<byte> SystemColorIdTable
        //{
        //    get
        //    {
        //        return new ReadOnlySpan<byte>((void*)&\u003CPrivateImplementationDetails\u003E.\u003265182B1D4C6931DCFA70DED15DBD386810B80FF, 33);
        //    }
        //}

        public static uint GetSystemColorArgb(KnownColor color)
        {
            throw new NotImplementedException("This was taken from System.Drawing");
            //Debug.Assert(Color.IsKnownColorSystem(color));

            //return color < KnownColor.Transparent
            //    ? s_staticSystemColors[(int)color - (int)KnownColor.ActiveBorder]
            //    : s_staticSystemColors[(int)color - (int)KnownColor.ButtonFace + (int)KnownColor.WindowText];
        }

        //private static int GetSystemColorId(KnownColor color)
        //{
        //    return color >= KnownColor.Transparent ? (int)KnownColorTable.SystemColorIdTable[(int)(color - 168 + 26)] : (int)KnownColorTable.SystemColorIdTable[(int)(color - 1)];
        //}
    }
}
