//using NetSettings.View;

using System;
using System.Collections;
using System.Collections.Generic;
using NetSettingsCore.Common;
using NetSettingsCore.Common.Classes;
using NetSettingsCore.Common.Interfaces;
using BorderStyle = NetSettingsCore.Common.BorderStyle;
using DockStyle = NetSettingsCore.Common.DockStyle;

namespace NetSettings.Forms
{
    internal class WinFormControl : System.Windows.Forms.Control, IControl
    {
        public Color BackColor { get; set; }
        public Point Location { get; set; }
        public new IFont Font { get; set; }
        public new IList Controls => base.Controls;
        public IList<IControl> LogicalControls { get; }
        public event EventHandler MouseClick;
        public event EventHandler SelectedIndexChanged;
        public event EventHandler MouseDoubleClick;
        public event EventHandler KeyDown;
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