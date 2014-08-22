﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetSettings
{
    public class ColorControl : Control
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle t = ClientRectangle ;
            t.Width -= 1;
            t.Height -= 1;
            e.Graphics.DrawRectangle(Pens.Black,t);
        }
    }
}
