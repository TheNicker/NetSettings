using System;
using System.Collections;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Media;
using NetSettingsCore.Common;
using BorderStyle = NetSettingsCore.Common.BorderStyle;
using DockStyle = NetSettingsCore.Common.DockStyle;

namespace NetSettingsCore.Avalonia.AvaloniaControls
{
    public class AvaloniaTextBox : TextBox, ITextBox
    {
        private AvaloniaFont _avaloniaFont;
        public new IList Controls => throw new NotImplementedException();
        public IFont Font
        {
            get
            {
                if (_avaloniaFont == null)
                {
                    //_avaloniaFont = new AvaloniaFont(base.Font);
                }

                return _avaloniaFont;

            }
            set
            {
                //base.Font = new AvaloniaFont(value.FontFamily, value.Size, (FontStyle)value.Appearance);
                //_avaloniaFont = new AvaloniaFont(base.Font);
            }
        }

        public event EventHandler MouseClick;
        public event EventHandler SelectedIndexChanged;
        public event EventHandler MouseDoubleClick;
        public event EventHandler KeyDown;
        public event EventHandler DoubleClick;
        public event EventHandler TextChanged;
        public event EventHandler Leave;
        public event EventHandler Click;
        public event EventHandler MouseEnter;
        public event EventHandler MouseLeave;

        public ITextBox Instance { get=>this; }
        public DockStyle Dock { get; set; }
        public BorderStyle BorderStyle { get; set; }
        public bool Multiline { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool ReadOnly { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Visible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IList<Common.IControl> LogicalControls => throw new NotImplementedException();

        int Common.IControl.Width { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int Common.IControl.Height { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IColor BackColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IPoint Location { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}