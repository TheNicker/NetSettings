using NetSettingsCore.Common.Interfaces;

namespace NetSettingsCore.Common
{
    public interface ITextBox : IControl
    {
        //public ITextBox Instance { get; }
        bool Multiline { get; set; }
        DockStyle Dock { get; set; }
        bool ReadOnly { get; set; }
        BorderStyle BorderStyle { get; set; }
    }
}