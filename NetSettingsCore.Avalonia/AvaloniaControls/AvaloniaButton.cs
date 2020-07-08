using System;
using System.Collections;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using NetSettingsCore.Common;
using FlatStyle = NetSettingsCore.Common.FlatStyle;

namespace NetSettings.Forms
{
    internal class AvaloniaButton : Button, IButton
    {
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
        //public new IFont Font { get; set; }
        ////public new IList<IGuiElement> Controls { get => base.Controls; set; }
        //public new IList Controls => base.Controls;
        //public FlatStyle FlatStyle
        //{
        //    get => throw new NotImplementedException(); set => throw new NotImplementedException();
        //}

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
        //public IList Controls { get; }
        //public IFont Font { get; set; }
        //public event EventHandler MouseClick;
        //public event EventHandler SelectedIndexChanged;
        //public event EventHandler MouseDoubleClick;
        //public event EventHandler KeyDown;
        //public FlatStyle FlatStyle { get; set; }
        public new FlatStyle FlatStyle
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public IList Controls => throw new NotImplementedException();
        public IFont Font { get; set; }
        public IColor BackColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Visible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Text { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IPoint Location { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IList<NetSettingsCore.Common.IControl> LogicalControls => throw new NotImplementedException();//base.LogicalChildren;

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