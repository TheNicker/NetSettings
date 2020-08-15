using NetSettingsCore.Common;
using NetSettingsCore.Common.Classes;
using NetSettingsCore.Common.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using DrawingPoint = System.Drawing.Point;
using DrawingColor = System.Drawing.Color;
using Point = NetSettingsCore.Common.Classes.Point;

namespace NetSettingsCore.WinForms.WinFormControls
{
    internal class WinFormControl : IControl
    {
        protected Control _control;

        public WinFormControl()
        {
            _control = new Control();
        }

        public virtual Color BackColor { get; set; }

        public Point Location
        {
            get => new Point(_control.Location.X, _control.Location.Y);
            set => _control.Location = new DrawingPoint(value.X, value.Y);
        }

        public IFont Font { get; set; }
        public bool Visible { get => _control.Visible; set => _control.Visible = value; }
        public string Text { get => _control.Text; set => _control.Text = value; }
        public int Width { get => _control.Width; set => _control.Width = value; }
        public int Height { get => _control.Height; set => _control.Height = value; }

        public virtual object Instance => _control;
        private readonly IList<IControl> _visualControls = new List<IControl>();

        public IList VisualControl => (IList)_visualControls;

        public IList<IControl> LogicalControls { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Color ForeColor { set => _control.ForeColor = DrawingColor.FromArgb(value.A, value.R, value.G, value.B); }

        public IControl AddVisualControl(IControl control)
        {
            _visualControls.Add(control);
            _control.Controls.Add((Control)control.Instance);
            return control;
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
    }
}