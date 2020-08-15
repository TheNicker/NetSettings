using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using NetSettingsCore.Common;
using NetSettingsCore.Common.Classes;
using NetSettingsCore.Common.Interfaces;

namespace NetSettingsCore.WinForms.WinFormControls
{
    internal class WinFormComboBox : WinFormControl, IComboBox
    {
        private readonly ComboBox _comboBox = new ComboBox();

        public WinFormComboBox()
        {
            _control = _comboBox;
        }

        public object SelectedItem { get => _comboBox.SelectedItem; set => _comboBox.SelectedItem = value; }

        public void AddItem(string item)
        {
            _ = _comboBox.Items.Add(item);
        }
    }
}