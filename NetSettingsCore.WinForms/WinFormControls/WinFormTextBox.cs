using System;
using System.Windows.Forms;
using NetSettings.Common.Interfaces;
using BorderStyle = NetSettings.Common.Classes.BorderStyle;
using WinFormBorderStyle = System.Windows.Forms.BorderStyle;

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

        public BorderStyle BorderStyle
        {
            get => Enum.Parse<BorderStyle>(_textBox.BorderStyle.ToString());
            set => Enum.Parse<WinFormBorderStyle>(value.ToString());
        }

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