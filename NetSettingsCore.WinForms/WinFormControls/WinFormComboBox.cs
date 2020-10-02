using System;
using System.Windows.Forms;
using NetSettings.Common.Interfaces;

namespace NetSettings.WinForms.WinFormControls
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

        public event EventHandler SelectedIndexChanged
        {
            add => _comboBox.SelectedIndexChanged += value;
            remove => _comboBox.SelectedIndexChanged -= value;
        }
    }
}