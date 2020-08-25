using NetSettingsCore.Common;
using NetSettingsCore.Common.Classes;
using NetSettingsCore.Common.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using DrawingColor = System.Drawing.Color;
using DrawingPoint = System.Drawing.Point;
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

        public virtual Color BackColor
        {
            get
            {
                var color = _control.BackColor;
                return Color.FromArgb(color.A, color.R, color.G, color.B);
            }
            set => _control.BackColor = DrawingColor.FromArgb(value.A, value.R, value.G, value.B);
        }

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

        public static T Cast<T>(object o)
        {
            return (T)o;
        }

        //TODO: Open all the events
        public event EventHandler MouseClick
        {
            add => _control.MouseClick += (object sender, MouseEventArgs e) => value(sender, e);
            remove => _control.MouseClick -= (object sender, MouseEventArgs e) => value(sender, e);
        }

        public event EventHandler MouseDoubleClick
        {
            add => _control.MouseDoubleClick += (object sender, MouseEventArgs e) => value(sender, e);
            remove => _control.MouseDoubleClick -= (object sender, MouseEventArgs e) => value(sender, e);
        }
        public event EventHandler KeyDown
        {
            add => _control.KeyDown += (object sender, KeyEventArgs e) => value(sender, e);
            remove => _control.KeyDown -= (object sender, KeyEventArgs e) => value(sender, e);
        }
        public event EventHandler DoubleClick
        {
            add => _control.DoubleClick += value;
            remove => _control.DoubleClick -= value;
        }
        public event EventHandler TextChanged
        {
            add => _control.TextChanged += value;
            remove => _control.TextChanged -= value;
        }
        public event EventHandler Leave
        {
            add => _control.Leave += value;
            remove => _control.Leave -= value;
        }
        public event EventHandler Click
        {
            add => _control.Click += value;
            remove => _control.Click -= value;
        }
        public event EventHandler MouseEnter
        {
            add => _control.MouseEnter += value;
            remove => _control.MouseEnter -= value;
        }

        public event EventHandler MouseLeave
        {
            add => _control.MouseLeave += value;
            remove => _control.MouseLeave -= value;
        }
    }
}