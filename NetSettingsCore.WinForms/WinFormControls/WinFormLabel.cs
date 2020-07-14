using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using NetSettingsCore.Common;
using NetSettingsCore.Common.Interfaces;
using Color = NetSettingsCore.Common.Classes.Color;
using Point = NetSettingsCore.Common.Classes.Point;

namespace NetSettingsCore.WinForms.WinFormControls
{
    public class WinFormLabel : Label, ILabelSingleClick
    {
        private WinFormFont _winFormFont;
        public Color BackColor { get; set; }
        public Point Location { get; set; }

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
        public IList<IControl> LogicalControls { get; }
        public event EventHandler MouseClick;
        public event EventHandler SelectedIndexChanged;
        public event EventHandler MouseDoubleClick;
        public event EventHandler KeyDown;
        public void SetStyle(GuiElementStyles flag, bool value)
        {
            base.SetStyle((ControlStyles)flag, value);
        }

        public Color ForeColor { get; set; }
        
    }
}