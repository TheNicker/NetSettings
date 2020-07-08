using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using NetSettingsCore.Common;
using NetSettingsCore.WinForms.SystemDrawingItems;
using DrawingColor = System.Drawing.Color;

namespace NetSettingsCore.WinForms.Controls
{
    public class ColorControl : TextBox, IColorControl
    {
        private IColor _color;
        private IColor _backColor;

        public ColorControl()
        {
            //this.Font = new Font("Arial", 9, FontStyle.Bold);//TODO: Open this line
            //this.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }

        public IColor Color
        {
            get => BackColor;
            set
            {
                if (!Updating)
                {
                    BackColor = value;

                    var brightness = Math.Sqrt(.241 * BackColor.R * BackColor.R + .691 * BackColor.G * BackColor.G + .068 * BackColor.B * BackColor.B);
                    ForeColor = brightness < 130 ? ForeColor = DrawingColor.White : ForeColor = DrawingColor.Black;

                    RefreshName();
                }
            }
        }

        private void RefreshName()
        {
            if (!Updating && !DisableAutoColorName)
            {
                Updating = true;
                Text = "#" + BackColor.ToArgb().ToString("X").Substring(2, 6);
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
                DisableAutoColorName = true;

            var a = DrawingColor.AliceBlue;

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public IList Controls => base.Controls;
        public IColor BackColor
        {
            get => _backColor;
            set { _backColor = new MyColor(value); }
        }
        public IPoint Location { get; set; }
        public new IFont Font { get; set; }

        public IList<IControl> LogicalControls => throw new NotImplementedException();

        public event EventHandler MouseClick;
        public event EventHandler SelectedIndexChanged;
        public event EventHandler MouseDoubleClick;
        public event EventHandler KeyDown;
    }
}
