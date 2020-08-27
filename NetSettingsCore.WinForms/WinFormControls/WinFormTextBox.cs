using System.Windows.Forms;
using NetSettings.Common.Interfaces;
using BorderStyle = NetSettings.Common.Classes.BorderStyle;
using DockStyle = NetSettings.Common.Classes.DockStyle;

namespace NetSettings.WinForms.WinFormControls
{
    internal class WinFormTextBox : WinFormControl, ITextBox
    {
        private readonly TextBox _textBox = new TextBox();

        public WinFormTextBox()
        {
            _control = _textBox;
        }

        public bool Multiline { get => _textBox.Multiline; set => _textBox.Multiline = value; }
        public bool ReadOnly { get => _textBox.ReadOnly; set => _textBox.ReadOnly = value; }
        public BorderStyle BorderStyle { get => (BorderStyle)_textBox.BorderStyle; set => _textBox.BorderStyle = (System.Windows.Forms.BorderStyle)value; }

        //private WinFormFont _winFormFont;

        //public new IFont Font
        //{
        //    get
        //    {
        //        if (_winFormFont == null)
        //        {
        //            _winFormFont = new WinFormFont(base.Font);
        //        }

        //        return _winFormFont;

        //    }
        //    set
        //    {
        //        base.Font = new Font(value.FontFamily, value.Size, (FontStyle)value.Appearance);
        //        _winFormFont = new WinFormFont(base.Font);
        //    }
        //}
    }
}