using NetSettings.Common.Classes;

namespace NetSettings.Common.Interfaces
{
    public interface IFont : INativeGuiElement
    {
        float Size { get; }
        FontAppearance Appearance { get;  }
        string FontFamily { get;  }
        MeasureUnit Unit { get; }
    }

    public interface INativeGuiElement : IGuiElement
    {
        object Native { get; }
    }
}