using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using NetSettingsCore.Common;

namespace NetSettingsCore.WinForms.SystemDrawingItems
{
    class MyColor : IColor //TODO: Do we need this class?
    {
        public Color Color { get; set; }

        public MyColor(IColor color)
        {
            this.Color = Color.FromArgb(color.R, color.G, color.B);
        }

        public MyColor(int r, int g, int b)
        {
            this.Color = Color.FromArgb(r, g, b);
        }

        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public int ToArgb()
        {
            throw new NotImplementedException();
        }

        //public IColor FromArgb(double r, double g, double b)
        //{
        //    //return Color.FromArgb(r, g, b);
        //    //throw new NotImplementedException();
        //    return FromArgb((int) r, (int) g, (int) b);
        //}

        public IColor FromArgb(int r, int g, int b)
        {
            var result = new MyColor(r,g,b);
            result.Color = Color.FromArgb(r, g, b);
            return result;
        }
    }
}
