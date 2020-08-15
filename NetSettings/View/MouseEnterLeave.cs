using System;
using NetSettingsCore.Common;
using NetSettingsCore.Common.Interfaces;

//using System.Windows.Forms;

namespace NetSettings.Controls
{
    public class MouseEnterLeave //TODO: Remove this file as this bug is created by Lior
    {
        int i;
        public event EventHandler MouseEnter = delegate { };
        public event EventHandler MouseLeave = delegate { };
        public static IControl LastEntered;
        public MouseEnterLeave(IGuiElement aTarget)
        {
            AddEvents(aTarget as IControl);
        }

        private void AddEvents(IControl aTarget)
        {
            if (aTarget != null)
            {
                aTarget.MouseEnter += aTarget_MouseEnter;
                aTarget.MouseLeave += aTarget_MouseLeave;
            }

            if (aTarget.VisualControl != null) //TODO: Remove this condition!
            {
                foreach (IControl control in aTarget.VisualControl)
                    AddEvents(control);//TODO: Fix this!
            }

            //TODO: Do we need this lines?
            //foreach (IControl control in aTarget.LogicalControls)
            //    AddEvents(control);
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
            {
                if (LastEntered != null)
                    MouseLeave(LastEntered, e);
                
                LastEntered = sender as IControl;
                MouseEnter(sender, e);
            }
        }
    }
}
