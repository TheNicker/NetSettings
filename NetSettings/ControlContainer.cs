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
