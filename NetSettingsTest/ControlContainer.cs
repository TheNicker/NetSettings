using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetSettingsTest
{
    public partial class ControlContainer : ScrollableControl
    {
        public ControlContainer()
        {
            DoubleBuffered = true;
            AutoScroll = true;
        }
    }
}
