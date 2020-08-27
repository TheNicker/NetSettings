using NetSettings.Common.Classes;

namespace NetSettings.Common.Interfaces
{
    public interface IButton : IControl
    {
        FlatStyle FlatStyle { get; set; }
    }
}