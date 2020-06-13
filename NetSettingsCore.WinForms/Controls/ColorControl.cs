using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using NetSettingsCore.Common;

namespace NetSettings.WinForms.Controls
{
    public class ColorControl : TextBox, IColorControl
    {
        public ColorControl()
        {
            //this.Font = new Font("Arial", 9, FontStyle.Bold);//TODO: Open this line
            //this.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }

        public Color Color
        {
            get
            {
                return BackColor;
            }
            set
            {
                if (!Updating)
                {
                    BackColor = value;

                    double brightness = Math.Sqrt(.241 * BackColor.R * BackColor.R + .691 * BackColor.G * BackColor.G + .068 * BackColor.B * BackColor.B);
                    ForeColor = brightness < 130 ? ForeColor = Color.White : ForeColor = Color.Black;

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

            var a = Color.AliceBlue;

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public IList Controls => base.Controls;
        public new IFont Font { get; set; }
        public event EventHandler MouseClick;
        public event EventHandler SelectedIndexChanged;
        public event EventHandler MouseDoubleClick;
        public event EventHandler KeyDown;
    }
}
