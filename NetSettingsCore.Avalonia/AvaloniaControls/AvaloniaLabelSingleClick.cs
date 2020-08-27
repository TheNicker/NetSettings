using System;
using NetSettings.Common.Classes;
using NetSettings.Common.Interfaces;

namespace NetSettings.Avalonia.AvaloniaControls
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