using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using NetSettingsCore.Common;
using NetSettingsCore.Common.Interfaces;
using BorderStyle = NetSettingsCore.Common.BorderStyle;
using DockStyle = NetSettingsCore.Common.DockStyle;
using WinFormDockStyle = System.Windows.Forms.DockStyle;
using Color = NetSettingsCore.Common.Classes.Color;
using Point = NetSettingsCore.Common.Classes.Point;

namespace NetSettingsCore.WinForms.WinFormControls
{
    internal class WinFormTextBox : WinFormControl, ITextBox
    {
        private readonly TextBox _textBox = new TextBox();

        public WinFormTextBox()
        {
            _control = _textBox;
        }

        public bool Multiline { get => _textBox.Multiline; set => _textBox.Multiline = value; }
        public DockStyle Dock { get => (DockStyle)_textBox.Dock; set => _textBox.Dock = (System.Windows.Forms.DockStyle)value; }
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