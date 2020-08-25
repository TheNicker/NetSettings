using NetSettingsCore.Common;
using NetSettingsCore.Common.Interfaces;
using System;

namespace NetSettings.View
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
                aTarget.MouseEnter += delegate(object sender, EventArgs e) { aTarget_MouseEnter(aTarget, e); };
                aTarget.MouseLeave += delegate (object sender, EventArgs e) { aTarget_MouseLeave(aTarget, e); };
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

        private void aTarget_MouseLeave(object sender, EventArgs e)
        {
            //TODO: Why do we need the lines below?
            //if (--i < 0)
            //{
            //    i = 0;
            //}

            //if (i == 0)
            //{
            MouseLeave(sender, e);
            //}
        }

        private void aTarget_MouseEnter(object sender, EventArgs e)
        {
            //TODO: Why do we need the lines below?
            //if (++i == 1)
            //{
            //    if (LastEntered != null)
            //    {
            //        MouseLeave(LastEntered, e);
            //    }

            //    LastEntered = sender as IControl;
            MouseEnter(sender, e);
            //}
        }
    }
}
