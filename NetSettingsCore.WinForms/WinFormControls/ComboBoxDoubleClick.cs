using System;

namespace NetSettings.WinForms.WinFormControls
{
    //TODO: Can this be deleted? See WinFormLabel
    //TODO: Can this be changed to internal?
    internal class ComboBoxDoubleClick : WinFormComboBox 
    {
        DateTime prevClick = DateTime.MinValue;

        public ComboBoxDoubleClick()
        {
            throw new NotImplementedException("Do we need the event below?");
        }
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
