using NetSettings.Common.Classes;

namespace NetSettings.Common.Interfaces
{
    public interface IFont : IGuiElement
    {
        float Size { get; }
        FontAppearance Appearance { get; set; }
        //string Name { get; set; }
        string FontFamily { get;  }
    }
}