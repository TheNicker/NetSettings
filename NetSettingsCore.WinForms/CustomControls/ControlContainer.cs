using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetSettingsCore.Common;
using System.Collections;

namespace NetSettings.Controls
{
    public partial class ControlContainer : ScrollableControl, IControlContainer
    {
        IList IControlContainer.Controls => Controls;

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

        public void Reset()
        {
            Controls.Clear();
            ResetPosition();
        }

        public void ResetPosition()
        {
            HorizontalScroll.Value = VerticalScroll.Value = 0;

        }

        public void StartUpdate()
        {
            this.SuspendLayout();
            ControlHelper.SuspendDrawing(this);
        }
        public void EndUpdate()
        {
            this.ResumeLayout();
            ControlHelper.ResumeDrawing(this);
        }
    }
}
