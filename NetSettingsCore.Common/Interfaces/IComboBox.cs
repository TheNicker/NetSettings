using NetSettingsCore.Common.Interfaces;

namespace NetSettingsCore.Common
{
    public interface IComboBox : IControl
    {
        object SelectedItem { get; set; }
        //IList<string> Items { get; }
        void AddItem(string item);
    }
}