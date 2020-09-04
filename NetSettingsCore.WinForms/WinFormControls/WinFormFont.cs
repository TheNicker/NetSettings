using NetSettings.Common.Classes;
using NetSettings.Common.Interfaces;
using System;
using System.Drawing;

namespace NetSettings.WinForms.WinFormControls
{
    internal class WinFormFont : IFont
    {
        private Font _font;

        public float Size => _font.Size;
        public FontAppearance Appearance => Enum.Parse<FontAppearance>(_font.Style.ToString());
        public string FontFamily => _font.FontFamily.ToString();
        public MeasureUnit Unit => Enum.Parse<MeasureUnit>(_font.Unit.ToString());
        public object Native => _font;

        public WinFormFont(string familyName, float emSize) :
            this(familyName, emSize, FontAppearance.Regular)
        {
        }

        public WinFormFont(string familyName, float emSize, FontAppearance style)
        {
            var fontStyle = Enum.Parse<FontStyle>(style.ToString(), true);
            _font = new Font(familyName, emSize, fontStyle);
        }

        internal WinFormFont(Font font)
        {
            _font = font ?? throw new NullReferenceException();
        }
    }
}