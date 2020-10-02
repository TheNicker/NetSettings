using NetSettings.Common.Classes;

namespace NetSettings.Common.Interfaces
{
    public interface IGuiProvider
    {
        //IControl CreateGuiElement(string guiElementName);
        IGuiElement CreateGuiElement(GuiElementType guiElementName); //TODO: Add a params[] parameter that will be passed to the constructor
        IGuiElement CreateGuiElement(GuiElementType guiElementName, params object[] list); //TODO: Add a params[] parameter that will be passed to the constructor
        IGuiElement CreateGuiElement(object guiElementName); //TODO: Add a params[] parameter that will be passed to the constructor
        IGuiElement CreateGuiElement(object guiElementName, params object[] list); //TODO: Add a params[] parameter that will be passed to the constructor
        //Type getElementType(object textBox);
        //IControl getElement();
        void ShowPreviewForm(ShowFormParams parameters);
        //IForm GetPreviewForm();
    }
}