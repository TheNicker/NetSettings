using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetSettings.Controls
{
    class MouseEnterLeave
    {
        int i = 0;
        public event EventHandler MouseEnter = delegate { };
        public event EventHandler MouseLeave = delegate { };

        public MouseEnterLeave(Control aTarget)
        {
            AddEvents(aTarget);
        }

        private void AddEvents(Control aTarget)
        {
            if (aTarget != null)
            {
                aTarget.MouseEnter += aTarget_MouseEnter;
                aTarget.MouseLeave += aTarget_MouseLeave;
            }
            foreach (Control control in aTarget.Controls)
                AddEvents(control);


        }

        void aTarget_MouseLeave(object sender, EventArgs e)
        {

            if (--i < 0)
                i = 0;
            if (i == 0)
                MouseLeave(sender, e);

        }

        void aTarget_MouseEnter(object sender, EventArgs e)
        {
            if (++i == 1)
                MouseEnter(sender, e);
        }
    }
}
