using System;
using NetSettings.Common.Classes;
using NetSettings.Common.Interfaces;
using NetSettings.WinForms.WinFormControls;

namespace NetSettings.WinForms
{
    public class WinFormGuiProvider : IGuiProvider
    {
        public IGuiElement CreateGuiElement(string guiElementName)
        {
            switch (guiElementName)
            {
                case "textbox":
                    var value = new WinFormControl();
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
            IGuiElement control;
            switch (guiElementName)
            {
                case GuiElementType.Image:
                case GuiElementType.Number:
                case GuiElementType.Text:
                    control = new WinFormTextBox();
                    break;
                case GuiElementType.IFont:
                    if (list.Length == 2)
                    {
                        control = new WinFormFont((string)list[0], (float)list[1]);
                    }
                    else if (list.Length == 3)
                    {
                        control = new WinFormFont((string) list[0], (float) list[1], (FontAppearance) list[2]);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                    break;
                case GuiElementType.GuiElement:
                    control = new WinFormControl();
                    break;
                case GuiElementType.Label:
                    control = new WinFormLabel();
                    break;
                case GuiElementType.Menu: //Label
                    control = new WinFormLabel();
                    break;
                case GuiElementType.Bool:
                    control = new WinFormCheckBox();
                    break;
                case GuiElementType.Button:
                    control = new WinFormButton();
                    break;
                case GuiElementType.Color:
                    control = new WinFormColorControl();
                    break;
                case GuiElementType.Combo:
                    control = new WinFormComboBox();
                    break;
                case GuiElementType.ColorDialog:
                    control = new WinFormColorDialog();
                    break;
                default:
                    throw new NotImplementedException($"gui element {guiElementName} is not known. How did we get here?");
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