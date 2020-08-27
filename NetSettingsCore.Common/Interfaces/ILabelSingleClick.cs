using NetSettings.Common.Classes;

namespace NetSettings.Common.Interfaces
{
    public interface ILabelSingleClick : IControl //TODO: Rename to ILabel?
    {
        void SetStyle(GuiElementStyles standardDoubleClick, bool value);
    }
}