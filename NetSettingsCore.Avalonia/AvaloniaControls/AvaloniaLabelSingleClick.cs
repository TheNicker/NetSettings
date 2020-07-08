
using System;
using NetSettings.Forms;
using NetSettingsCore.Avalonia.AvaloniaControls;

namespace NetSettings.Controls
{
    public class AvaloniaLabelSingleClick : AvaloniaLabel //TODO: Can this be changed to internal?
    {
        public AvaloniaLabelSingleClick(bool aAllowDoubleClick)
        {
            //SetStyle(ControlStyles.StandardDoubleClick, aAllowDoubleClick);
            throw new NotImplementedException();
        }
    }
}
