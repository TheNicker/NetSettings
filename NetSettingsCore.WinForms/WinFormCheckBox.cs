using System;
using System.Collections;
using System.Windows.Forms;
using NetSettingsCore.Common;

namespace NetSettings.Forms
{
    public class WinFormCheckBox : CheckBox, ICheckBox
    {
        public new IList Controls => base.Controls;
        public IFont Font { get; set; }
        public event EventHandler MouseClick;
        public event EventHandler SelectedIndexChanged;
        public event EventHandler MouseDoubleClick;
        public event EventHandler KeyDown;
    }
}