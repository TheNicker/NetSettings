
using System;
using NetSettings.Forms;
using NetSettingsCore.Avalonia.AvaloniaControls;
using NetSettingsCore.Common;

namespace NetSettings.Controls
{
    internal class AvaloniaLabelSingleClick : AvaloniaControl, ILabelSingleClick //TODO: Can this be changed to internal?
    {
        public AvaloniaLabelSingleClick(bool aAllowDoubleClick)
        {
            //SetStyle(ControlStyles.StandardDoubleClick, aAllowDoubleClick);
            throw new NotImplementedException();
        }

        public void SetStyle(GuiElementStyles standardDoubleClick, bool value)
        {
            throw new NotImplementedException();
        }
    }
}
