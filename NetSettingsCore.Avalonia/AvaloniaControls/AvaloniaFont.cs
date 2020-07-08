using System;
using NetSettingsCore.Common;
using FontStyle = Avalonia.Media.FontStyle;
using Font = SharpDX.DirectWrite.Font;

namespace NetSettingsCore.Avalonia.AvaloniaControls
{
    internal class AvaloniaFont : IFont
    {
        public Font Instance { get; internal set; }

        public AvaloniaFont(string familyName, float emSize) :
            this(familyName, emSize, FontAppearance.Regular)
        {
        }

        public AvaloniaFont(string familyName, float emSize, FontAppearance style)
        {
            throw new NotImplementedException();
            //Instance = new AvaloniaFont(familyName, emSize, (FontStyle)style);
        }

        internal AvaloniaFont(Font font)
        {
            Instance = font;
        }

        public float Size => throw new NotImplementedException();//Instance.Size);
        public FontAppearance Appearance { get; set; }
        public string Name { get; set; }
        public string FontFamily
        {
            get => throw new NotImplementedException();//Instance.Name);
        }
    }
}