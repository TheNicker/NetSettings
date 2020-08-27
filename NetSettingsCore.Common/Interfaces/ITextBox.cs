using NetSettings.Common.Classes;

namespace NetSettings.Common.Interfaces
{
    public interface ITextBox : IControl
    {
        //public ITextBox Instance { get; }
        bool Multiline { get; set; }
        bool ReadOnly { get; set; }
        BorderStyle BorderStyle { get; set; }
    }
}