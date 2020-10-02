using CustomControls.WinForms.Controls;
using NetSettings.Common.Classes;
using NetSettings.Common.Interfaces;
using DrawingColor = System.Drawing.Color;

namespace NetSettings.WinForms.WinFormControls
{
    internal class WinFormColorControl : WinFormControl, IColorControl
    {
        private readonly ColorControl _colorControl = new ColorControl();

        public WinFormColorControl()
        {
            _control = _colorControl;
        }

        public override Color BackColor
        {
            set => _colorControl.BackColor = DrawingColor.FromArgb(value.A, value.R, value.G, value.B);
        }
    }
}