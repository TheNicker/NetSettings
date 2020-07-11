using System;
using System.Collections;
using System.Windows.Forms;
using NetSettingsCore.WinForms;
using NetSettingsCore.Common;

namespace NetSettingsTestCore.Controls
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
