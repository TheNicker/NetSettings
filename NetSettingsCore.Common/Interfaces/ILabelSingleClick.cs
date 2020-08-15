using NetSettingsCore.Common.Classes;
using NetSettingsCore.Common.Interfaces;

namespace NetSettingsCore.Common
{
    public interface ILabelSingleClick : IControl //TODO: Rename to ILabel?
    {
        void SetStyle(GuiElementStyles standardDoubleClick, bool value);
    }
}