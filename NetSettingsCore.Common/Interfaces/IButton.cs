namespace NetSettingsCore.Common
{
    public interface IButton : IControl
    {
        FlatStyle FlatStyle { get; set; }
        IColor BackColor { get; set; }
    }
}