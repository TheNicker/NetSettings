using System.Windows.Forms;
using NetSettings.Forms;
using NetSettingsCore.WinForms.WinFormControls;

namespace NetSettings.Controls
{
    //TODO: Can this be changed to internal?
    //TODO: Can this be deleted? See WinFormLabel
    public class WinFormLabelSingleClick : WinFormLabel
    {
        public WinFormLabelSingleClick(bool aAllowDoubleClick)
        {
            SetStyle(ControlStyles.StandardDoubleClick, aAllowDoubleClick);
        }
    }
}
