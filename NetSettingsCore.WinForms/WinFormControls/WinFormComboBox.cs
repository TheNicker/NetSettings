using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using NetSettingsCore.Common;
using NetSettingsCore.Common.Classes;
using NetSettingsCore.Common.Interfaces;

namespace NetSettingsCore.WinForms.WinFormControls
{
    public class WinFormComboBox : ComboBox, IComboBox
    {
        public IList Controls => base.Controls;
        public IList<IControl> LogicalControls { get; }
        public Color BackColor { get; set; }
        public Point Location { get; set; }
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