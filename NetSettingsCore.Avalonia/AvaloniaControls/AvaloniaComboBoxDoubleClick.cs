using System;

namespace NetSettings.Avalonia.AvaloniaControls
{
    internal class AvaloniaComboBoxDoubleClick : AvaloniaComboBox
    {
        DateTime prevClick = DateTime.MinValue;

        //TODO: implement this
        //protected override void OnMouseClick(MouseEventArgs e)
        //{
        //    if (DateTime.Now.AddMilliseconds(-500) < prevClick)
        //    {
        //        prevClick = DateTime.MinValue;
        //        OnMouseDoubleClick(e);
        //    }
        //    else
        //        prevClick = DateTime.Now;

        //    base.OnMouseClick(e);
        //}
    }
}