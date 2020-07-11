using NetSettingsCore.Common.Interfaces;

namespace NetSettingsCore.Common
{
    public interface ICheckBox : IControl
    {
        bool Checked { get; set; }
    }
}