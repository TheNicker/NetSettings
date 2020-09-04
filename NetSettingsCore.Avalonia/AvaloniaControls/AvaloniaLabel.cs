using System;
using NetSettings.Common.Classes;
using NetSettings.Common.Interfaces;

namespace NetSettings.Avalonia.AvaloniaControls
{
    internal class AvaloniaTextBlock : AvaloniaControl, ILabel
    {
        public AvaloniaTextBlock()
        {
        }

        //public void SetStyle(GuiElementStyles standardDoubleClick, bool value)
        //{
        //    throw new NotImplementedException();
        //}
        //private AvaloniaFont _winFormFont;

        //public Point Location { get; set; }

        //public new IFont Font
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //        if (_winFormFont == null)
        //        {
        //            //_winFormFont = new AvaloniaFont(base.Font);
        //        }

        //        return _winFormFont;

        //    }
        //    set
        //    {
        //        //base.Font = new Font(value.FontFamily, value.Size, (FontStyle)value.Appearance);
        //        //_winFormFont = new AvaloniaFont(base.Font);
        //        throw new NotImplementedException();
        //    }
        //}

        //public event EventHandler DoubleClick;

        //public bool Visible { get; set; }
        //public new IList Controls { get => throw new NotImplementedException(); }
        //public IList<IControl> LogicalControls { get; }
        //public int Width { get; set; }
        //public int Height { get; set; }
        //public Color BackColor { get; set; }
        //public event EventHandler MouseClick;
        //public event EventHandler TextChanged;
        //public event EventHandler Leave;
        //public event EventHandler SelectedIndexChanged;
        //public event EventHandler MouseDoubleClick;
        //public event EventHandler KeyDown;
        //public event EventHandler Click;
        //public event EventHandler MouseEnter;
        //public event EventHandler MouseLeave;

        //public void SetStyle(GuiElementStyles flag, bool value)
        //{
        //    //base.SetStyle((ControlStyles)flag, value);
        //    throw new NotImplementedException();
        //}

        //public Color ForeColor { get; set; }
        //Color ILabel.ForeColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //Color IControl.BackColor { set => throw new NotImplementedException(); }
    }
}