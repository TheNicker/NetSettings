using System;
using System.Collections;
using System.Windows.Forms;
using NetSettingsCore.WinForms;
using NetSettingsCore.Common;
using NetSettingsCore.Common.Interfaces;

namespace NetSettingsTestCore.Controls
{
    public partial class ControlContainer : ScrollableControl, IControlContainer
    {
        //public IList Controls => Controls;
        public void AddControl(IControl control)
        {
            this.Controls.Add(control.Instance as Control);
        }

        public ControlContainer()
        {
            this.DoubleBuffered = true;
            this.AutoScroll = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint, true);
        }

        public void ScrollY(int delta)
        {
            int scrollPosition = this.VerticalScroll.Value - delta;
            scrollPosition = Math.Min(Math.Max(scrollPosition, this.VerticalScroll.Minimum), this.VerticalScroll.Maximum);
            this.VerticalScroll.Value = scrollPosition;
        }

        public void Reset()
        {
            Controls.Clear();
            ResetPosition();
        }

        public void ResetPosition()
        {
            this.HorizontalScroll.Value = this.VerticalScroll.Value = 0;

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
