using System;
using System.Windows.Forms;
using NetSettings.Common.Interfaces;
using NetSettings.WinForms;

namespace NetSettingsTest.Controls
{
    public partial class ControlContainer : ScrollableControl, IControlContainer
    {
        public void AddControl(IControl control)
        {
            this.Controls.Add(control.Instance as Control);
        }

        public ControlContainer()
        {
            base.DoubleBuffered = true;
            //https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.controlstyles?view=netcore-3.1
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            base.AutoScroll = true;
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
