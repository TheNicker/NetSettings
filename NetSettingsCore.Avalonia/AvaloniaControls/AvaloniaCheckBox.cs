using System;
using System.Collections;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using NetSettingsCore.Avalonia.AvaloniaControls;
using NetSettingsCore.Common;
using IControl = NetSettingsCore.Common.IControl;

namespace NetSettings.Forms
{
    public class AvaloniaCheckBox : CheckBox, ICheckBox
    {
        private LogicalControls _logicalControls;

        public AvaloniaCheckBox()
        {
            _logicalControls = new LogicalControls(base.LogicalChildren);
        }

        public new IList Controls => throw new NotImplementedException("Avalonia:CheckBox:Controls");
        public IFont Font { get; set; }
        public bool Visible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Text { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IList<IControl> LogicalControls => _logicalControls;

        public IColor BackColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IPoint Location { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Checked { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int NetSettingsCore.Common.IControl.Width { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int NetSettingsCore.Common.IControl.Height { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler MouseClick;
        public event EventHandler SelectedIndexChanged;
        public event EventHandler MouseDoubleClick;
        public event EventHandler KeyDown;
        public event EventHandler DoubleClick;
        public event EventHandler TextChanged;
        public event EventHandler Leave;
        public event EventHandler MouseEnter;
        public event EventHandler MouseLeave;

        event EventHandler NetSettingsCore.Common.IControl.Click
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }
    }
}