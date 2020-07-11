using System;
using System.Collections;
using System.Collections.Generic;
using Avalonia.Controls;
using NetSettingsCore.Avalonia.AvaloniaControls;
using NetSettingsCore.Common;
using NetSettingsCore.Common.Classes;
using IControl = NetSettingsCore.Common.Interfaces.IControl;

namespace NetSettings.Forms
{
    public class AvaloniaComboBox : ComboBox, IComboBox
    {
        private LogicalControls _logicalControls;

        public AvaloniaComboBox()
        {
            _logicalControls = new LogicalControls(base.LogicalChildren);
        }

        public IList Controls => throw new NotImplementedException("Avalonia:ChecBox:Controls");
        public IFont Font { get; set; }
        public bool Visible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Text { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IList<IControl> LogicalControls => _logicalControls;

        public Color BackColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Point Location { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int IControl.Width { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int IControl.Height { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler MouseClick;
        public event EventHandler MouseDoubleClick;
        public event EventHandler KeyDown;
        public event EventHandler DoubleClick;
        public event EventHandler TextChanged;
        public event EventHandler Leave;
        public event EventHandler SelectedIndexChanged;
        public event EventHandler Click;
        public event EventHandler MouseEnter;
        public event EventHandler MouseLeave;

        public void AddItem(string item)
        {
            throw new NotImplementedException();
            //_logicalControls.Add(item);
        }
    }
}