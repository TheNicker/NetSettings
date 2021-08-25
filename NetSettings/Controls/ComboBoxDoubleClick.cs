using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetSettings
{
    class ComboBoxDoubleClick : ComboBox
    {
        DateTime prevClick = DateTime.MinValue;

        protected override void OnMouseClick(MouseEventArgs e)
        {
            this.DroppedDown = true;
            if (DateTime.Now.AddMilliseconds(-500) < prevClick)
            {
                prevClick = DateTime.MinValue;
                OnMouseDoubleClick(e);
            }
            else
                prevClick = DateTime.Now;

            base.OnMouseClick(e);
        }


    }
}
