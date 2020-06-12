using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
