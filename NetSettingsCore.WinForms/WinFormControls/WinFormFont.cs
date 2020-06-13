using System.Drawing;
using NetSettingsCore.Common;

namespace NetSettingsCore.WinForms.WinFormControls
{
    internal class WinFormFont : IFont
    {
        public Font Instance { get; internal set; }

        public WinFormFont(string familyName, float emSize) :
            this(familyName, emSize, FontAppearance.Regular)
        {
        }

        public WinFormFont(string familyName, float emSize, FontAppearance style)
        {
            Instance = new Font(familyName, emSize, (FontStyle)style);
        }

        internal WinFormFont(Font font)
        {
            Instance = font;
        }

        public float Size => Instance.Size;
        public FontAppearance Appearance { get; set; }
        public string Name { get; set; }
        public string FontFamily { get=>Instance.Name;  }
    }
}