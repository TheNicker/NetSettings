using System;
using System.Collections;
using System.Collections.Generic;
using Avalonia.Controls;
using NetSettingsCore.Common;
using IControl = NetSettingsCore.Common.IControl;

namespace NetSettingsCore.Avalonia.AvaloniaControls
{
    public class AvaloniaLabel : TextBlock, ILabelSingleClick
    {
        private AvaloniaFont _winFormFont;
        public IPoint Location { get; set; }

        public new IFont Font
        {
            get
            {
                throw new NotImplementedException();
                if (_winFormFont == null)
                {
                    //_winFormFont = new AvaloniaFont(base.Font);
                }

                return _winFormFont;

            }
            set
            {
                //base.Font = new Font(value.FontFamily, value.Size, (FontStyle)value.Appearance);
                //_winFormFont = new AvaloniaFont(base.Font);
                throw new NotImplementedException();
            }
        }

        public event EventHandler DoubleClick;

        public bool Visible { get; set; }
        public new IList Controls { get => throw new NotImplementedException(); }
        public IList<IControl> LogicalControls { get; }
        public int Width { get; set; }
        public int Height { get; set; }
        public IColor BackColor { get; set; }
        public event EventHandler MouseClick;
        public event EventHandler TextChanged;
        public event EventHandler Leave;
        public event EventHandler SelectedIndexChanged;
        public event EventHandler MouseDoubleClick;
        public event EventHandler KeyDown;
        public event EventHandler Click;
        public event EventHandler MouseEnter;
        public event EventHandler MouseLeave;

        public void SetStyle(GuiElementStyles flag, bool value)
        {
            //base.SetStyle((ControlStyles)flag, value);
            throw new NotImplementedException();
        }

        public IColor ForeColor { get; set; }
        IColor ILabelSingleClick.ForeColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        IColor IControl.BackColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}