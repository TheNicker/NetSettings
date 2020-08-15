using NetSettingsCore.Common;
using System;
using System.Windows.Forms;

namespace NetSettingsCore.WinForms.WinFormControls
{
    internal class WinFormLabel : WinFormControl, ILabelSingleClick
    {
        private readonly Label _label = new Label();

        public WinFormLabel()
        {
            _control = _label;
        }

        //private WinFormFont _winFormFont;

        //public new IFont Font
        //{
        //    get
        //    {
        //        if (_winFormFont == null)
        //        {
        //            _winFormFont = new WinFormFont(_label.Font);
        //        }

        //        return _winFormFont;

        //    }
        //    set
        //    {
        //        base.Font = new Font(value.FontFamily, value.Size, (FontStyle)value.Appearance);
        //        _winFormFont = new WinFormFont(base.Font);
        //    }
        //}
        //public new IList Controls { get => base.Controls; }

        public void SetStyle(GuiElementStyles flag, bool value)
        {
            throw new NotImplementedException();
            //_label.SetStyle sty.SetStyle((ControlStyles)flag, value);
        }
    }
}