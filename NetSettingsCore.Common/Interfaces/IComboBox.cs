using System;

namespace NetSettingsCore.Common.Interfaces
{
    public interface IComboBox : IControl
    {
        object SelectedItem { get; set; }
        //IList<string> Items { get; }
        void AddItem(string item);

        public event EventHandler SelectedIndexChanged;
    }
}