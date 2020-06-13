using System;
using System.Windows.Forms;

namespace NetSettings.WinForms.Controls
{
    public class ComboBoxDoubleClick : ComboBox
    {
        DateTime prevClick = DateTime.MinValue;

        protected override void OnMouseClick(MouseEventArgs e)
        {
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
