using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using NetSettingsCore.Common;
using NetSettingsCore.WinForms.WinFormControls;

namespace NetSettings.Forms
{
    public class WinFormLabel : Label, ILabelSingleClick
    {
        private WinFormFont _winFormFont;
        public new IFont Font
        {
            get
            {
                if (_winFormFont == null)
                {
                    _winFormFont = new WinFormFont(base.Font);
                }

                return _winFormFont;

            }
            set
            {
                base.Font = new Font(value.FontFamily, value.Size, (FontStyle)value.Appearance);
                _winFormFont = new WinFormFont(base.Font);
            }
        }
        public new IList Controls { get => base.Controls; }
        public event EventHandler MouseClick;
        public event EventHandler SelectedIndexChanged;
        public event EventHandler MouseDoubleClick;
        public event EventHandler KeyDown;
        public void SetStyle(GuiElementStyles flag, bool value)
        {
            base.SetStyle((ControlStyles)flag, value);
        }
    }
}