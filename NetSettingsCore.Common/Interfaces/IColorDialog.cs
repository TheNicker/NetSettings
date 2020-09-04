using NetSettings.Common.Classes;

namespace NetSettings.Common.Interfaces
{
    public interface IColorDialog : IGuiElement
    {
        Color Color { get; set; }
        bool FullOpen { get; set; }
        DialogResult ShowDialog();
    }
}