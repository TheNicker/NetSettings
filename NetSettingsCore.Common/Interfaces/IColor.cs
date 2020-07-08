using System;

namespace NetSettingsCore.Common
{
    public interface IColor
    {
        int R { get; set; }
        int G { get; set; }
        int B { get; set; }
        int ToArgb();
        //IColor FromArgb(double r, double g, double b);
        IColor FromArgb(int r, int g, int b);
    }
}