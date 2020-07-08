using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using NetSettingsCore.Common;
using FlatStyle = NetSettingsCore.Common.FlatStyle;

namespace NetSettings.Forms
{
    internal class WinFormButton : Button, IButton
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
            get => (FlatStyle)base.FlatStyle;
            set => base.FlatStyle = (System.Windows.Forms.FlatStyle)value;
        }
        public new IList Controls => base.Controls;
        public IColor BackColor { get; set; }
        public IPoint Location { get; set; }
        public IFont Font { get; set; }

        public IList<IControl> LogicalControls => throw new NotImplementedException();

        public event EventHandler MouseClick;
        public event EventHandler SelectedIndexChanged;
        public event EventHandler MouseDoubleClick;
        public event EventHandler KeyDown;
    }
}