using System.Windows.Forms;

namespace NetSettings.Controls
{
    internal class LabelSingleClick : Label
    {
        public LabelSingleClick(bool aAllowDoubleClick)
        {
            SetStyle(ControlStyles.StandardDoubleClick, aAllowDoubleClick);
        }
    }
}
