namespace NetSettingsCore.Common
{
    public interface ILabelSingleClick : IControl //TODO: Rename to ILabel?
    {
        bool Visible { get; set; }
        void SetStyle(GuiElementStyles standardDoubleClick, bool value);
        IColor ForeColor { get; set; }
    }
}