using System.Windows.Forms;

namespace NetSettings.Controls
{
    public class LabelSingleClick : Label //TODO: Can this be changed to internal?
    {
        public LabelSingleClick(bool aAllowDoubleClick)
        {
            SetStyle(ControlStyles.StandardDoubleClick, aAllowDoubleClick);
        }
    }
}
