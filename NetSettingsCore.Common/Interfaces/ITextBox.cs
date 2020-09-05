using NetSettings.Common.Classes;

namespace NetSettings.Common.Interfaces
{
    public interface ITextBox : IControl
    {
        bool Multiline { get; set; }
        bool ReadOnly { get; set; }
        BorderStyle BorderStyle { get; set; }
    }
}