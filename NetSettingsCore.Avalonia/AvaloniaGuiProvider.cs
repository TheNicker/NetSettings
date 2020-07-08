using System;
using NetSettings.Forms;
using NetSettings.View;
using NetSettingsCore.Avalonia.AvaloniaControls;
//using NetSettingsCore.Avalonia.AvaloniaControls;
using NetSettingsCore.Common; //using NetSettings.WinForms.Controls;

//using NetSettingsCore.WinForms.WinFormControls;

namespace NetSettingsCore.Avalonia
{
    public class AvaloniaGuiProvider : IGuiProvider
    {
        public IGuiElement CreateGuiElement(string guiElementName)
        {
            switch (guiElementName)
            {
                case "textbox":
                    var value = new AvaloniaControl();
                    return value;
                default:
                    throw new NotImplementedException("gui element creation is yet to be implemented.");
            }
        }

        public IGuiElement CreateGuiElement(GuiElementType guiElementName)
        {
            return CreateGuiElement(guiElementName, null);
        }

        public IGuiElement CreateGuiElement(GuiElementType guiElementName, params object[] list)
        {
            IGuiElement control = null; //TODO: Remove the null
            switch (guiElementName)
            {
                case GuiElementType.Image:
                case GuiElementType.Number:
                case GuiElementType.Text:
                    control = new AvaloniaTextBox();
                    break;
                case GuiElementType.IFont:
                    if (list.Length == 2)
                    {
                        //control = new AvaloniaFont((string)list[0], (float)list[1]);
                    }
                    else if (list.Length == 3)
                    {
                        //control = new AvaloniaFont((string) list[0], (float) list[1], (FontAppearance) list[2]);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                    break;
                case GuiElementType.GuiElement:
                    control = new AvaloniaControl();
                    break;
                case GuiElementType.Label:
                    //control = new AvaloniaLabel();
                    break;
                case GuiElementType.Menu: //Label
                    //control = new AvaloniaLabel();
                    break;
                case GuiElementType.Bool:
                    //control = new AvaloniaCheckBox();
                    break;
                case GuiElementType.Button:
                    control = new AvaloniaButton();
                    break;
                case GuiElementType.Color:
                    //control = new ColorControl();
                    break;
                case GuiElementType.Combo:
                    //control = new AvaloniaComboBox();
                    break;
                default:
                    throw new NotImplementedException("gui element creation is yet to be implemented.");
            }

            return control;
        }

        public IGuiElement CreateGuiElement(object guiElementName)
        {
            return CreateGuiElement(guiElementName, null);
        }

        public IGuiElement CreateGuiElement(object guiElementName, params object[] list)
        {
            return CreateGuiElement((GuiElementType) guiElementName);
        }

        public void ShowPreviewForm(ShowFormParams parameters)
        {
            throw new NotImplementedException();
        }
    }
}