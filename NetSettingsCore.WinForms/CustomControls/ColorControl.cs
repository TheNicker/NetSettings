using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using NetSettingsCore.Common;
using NetSettingsCore.Common.Interfaces;
using DrawingColor = System.Drawing.Color;
using Color = NetSettingsCore.Common.Classes.Color;
using Point = NetSettingsCore.Common.Classes.Point;

namespace NetSettingsCore.WinForms.CustomControls
{
    internal class ColorControl : TextBox, IColorControl
    {
        //private IColor _color;
        //private IColor _backColor;

        public ColorControl()
        {
            //this.Font = new Font("Arial", 9, FontStyle.Bold);//TODO: Open this line
            //this.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }

        private void RefreshName()
        {
            if (!Updating && !DisableAutoColorName)
            {
                Updating = true;
                Text = "#" + base.BackColor.ToArgb().ToString("X").Substring(2, 6);
                Updating = false;
            }

        }

        public bool DisableAutoColorName { get; set; }

        public bool Updating { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle t = ClientRectangle;
            t.Width -= 1;
            t.Height -= 1;
            e.Graphics.DrawRectangle(Pens.Black, t);
        }

        protected override void OnLeave(EventArgs e)
        {
            DisableAutoColorName = false;
            RefreshName();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (DisableAutoColorName)
                {
                    DisableAutoColorName = false;
                    //RefreshControlValue((colorControl.Tag as VisualItem));
                }
            }
            else
            {
                DisableAutoColorName = true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public IList Controls => base.Controls;
        public new Color BackColor
        {
            //get => base.BackColor;
            set {
                if (!Updating)
                {
                    base.BackColor = DrawingColor.FromArgb(value.A, value.R, value.G, value.B);

                    var brightness = Math.Sqrt(.241 * base.BackColor.R * base.BackColor.R + .691 * base.BackColor.G * base.BackColor.G + .068 * base.BackColor.B * base.BackColor.B);
                    ForeColor = brightness < 130 ? ForeColor = DrawingColor.White : ForeColor = DrawingColor.Black;

                    RefreshName();
                }
            }
        }
        public new Point Location { get; set; }
        public new IFont Font { get; set; }

        public IList<IControl> LogicalControls => throw new NotImplementedException();


        public event EventHandler MouseClick;
        public event EventHandler SelectedIndexChanged;
        public event EventHandler MouseDoubleClick;
        public event EventHandler KeyDown;
    }
}
