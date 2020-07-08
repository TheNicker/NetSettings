//using NetSettings.View;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Avalonia.Controls;
using NetSettingsCore.Common;
using BorderStyle = NetSettingsCore.Common.BorderStyle;
using DockStyle = NetSettingsCore.Common.DockStyle;
using IControl = NetSettingsCore.Common.IControl;

namespace NetSettings.Forms
{
    internal class AvaloniaControl : ContentControl, IControl
    {
        public new IFont Font { get; set; }
        public new IList Controls => throw new NotImplementedException("Avalonia:Control:Controls");

        public bool Visible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Text { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IList<IControl> LogicalControls => throw new NotImplementedException();

        public Color BackColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Point Location { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int IControl.Width { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int IControl.Height { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        IColor IControl.BackColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        IPoint IControl.Location { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
        //WinFormControl(System.Windows.Forms.Control control)
        //{
        //    Instance = control;
        //}

        //public VisualItem Tag { get; set; }
        //public ITextBox Instance => this;//{ get; set; }
        //public bool Multiline { get; set; }
        //public new DockStyle Dock { get; set; }
        //public bool ReadOnly { get; set; }
        //public BorderStyle BorderStyle { get; set; }

        //public FlatStyle FlatStyle
        //{
        //    get => throw new NotImplementedException(); set => throw new NotImplementedException();
        //}

        //public new IList<IGuiElement> Controls { get => base.Controls; set; }

        //public new event EventHandler MouseClick;
        //public event EventHandler SelectedIndexChanged;
        //public new event EventHandler MouseDoubleClick;
        //public new event EventHandler KeyDown;
        //public new int Size { get; set; }
        //public FontAppearance Appearance { get; set; }
        //public string FontFamily { get; set; }
        //public void SetStyle(GuiElementStyles standardDoubleClick, bool value)
        //{
        //    base.SetStyle((ControlStyles)standardDoubleClick, value);
        //}

        //public bool Checked { get; set; }
    }
}