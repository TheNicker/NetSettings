using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetSettings
{
    public partial class ControlContainer : ScrollableControl
    {
        public ControlContainer()
        {
            DoubleBuffered = true;
            AutoScroll = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
             ControlStyles.AllPaintingInWmPaint, true);
        }


        public void ScrollY(int delta)
        {
            int scrollPosition = VerticalScroll.Value - delta;
            scrollPosition = Math.Min(Math.Max(scrollPosition, VerticalScroll.Minimum), VerticalScroll.Maximum);
            VerticalScroll.Value = scrollPosition;
        }

        internal void Reset()
        {
            Controls.Clear();
            ResetPosition();
        }

        internal void ResetPosition()
        {
            HorizontalScroll.Value = VerticalScroll.Value = 0;

        }

        internal void StartUpdate()
        {
            this.SuspendLayout();
            ControlHelper.SuspendDrawing(this);
        }
        internal void EndUpdate()
        {
            this.ResumeLayout();
            ControlHelper.ResumeDrawing(this);
        }
    }
}
