using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using NetSettingsCore.Common;

namespace NetSettings.Forms
{
    public class WinFormComboBox : ComboBox, IComboBox
    {
        public IList Controls => base.Controls;
        public IFont Font { get; set; }
        public event EventHandler MouseClick;
        public event EventHandler MouseDoubleClick;
        public event EventHandler KeyDown;

        public void AddItem(string item)
        {
            Items.Add(item);
        }
    }
}