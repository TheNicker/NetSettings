using NetSettingsCore.Common.Classes;
using NetSettingsCore.Common.Interfaces;

namespace NetSettingsCore.Common
{
    public interface IButton : IControl
    {
        FlatStyle FlatStyle { get; set; }
    }
}