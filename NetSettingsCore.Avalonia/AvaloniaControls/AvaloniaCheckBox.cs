using Avalonia.Controls;
using NetSettings.Forms;
using NetSettingsCore.Common;

namespace NetSettingsCore.Avalonia.AvaloniaControls
{
    internal class AvaloniaCheckBox : AvaloniaControl, ICheckBox
    {
        //private LogicalControls _logicalControls;

        public AvaloniaCheckBox()
        {
            //_logicalControls = new LogicalControls(base.LogicalChildren);
            _control = new CheckBox();
        }

        public bool Checked { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        //public new IList Controls => throw new NotImplementedException("Avalonia:CheckBox:Controls");
        //public IFont Font { get; set; }
        //public bool Visible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //public string Text { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //public IList<IControl> LogicalControls => _logicalControls;

        //public Color BackColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //public Point Location { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //public bool Checked { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //int IControl.Width { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //int IControl.Height { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //public event EventHandler MouseClick;
        //public event EventHandler SelectedIndexChanged;
        //public event EventHandler MouseDoubleClick;
        //public event EventHandler KeyDown;
        //public event EventHandler DoubleClick;
        //public event EventHandler TextChanged;
        //public event EventHandler Leave;
        //public event EventHandler MouseEnter;
        //public event EventHandler MouseLeave;

        //event EventHandler IControl.Click
        //{
        //    add
        //    {
        //        throw new NotImplementedException();
        //    }

        //    remove
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
    }
}