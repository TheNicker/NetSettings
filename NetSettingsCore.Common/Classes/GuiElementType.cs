namespace NetSettings.View
{
    public enum GuiElementType
    {
        //{"text", typeof(ITextBox)},
        //{"bool", typeof(ICheckBox)
        //},
        //{"menu", typeof(ILabel)},
        //{"combo", typeof(IComboBoxDoubleClick)},
        //{"image", typeof(ITextBox)},
        //{"number", typeof(ITextBox)},
        //{"color", typeof(IColorControl)}
        Text,
        Number,
        Image,
        //ITextBox,
        Bool,
        //ICheckBox,
        Menu,
        //ILabel,
        Combo,
        //ComboBoxDoubleClick,
        Color,
        //ColorControl,
        IFont,
        GuiElement,
        Label,
        //ILabelSingleClick,
        Button,
        ColorDialog,
        SaveFileDialog,
        OpenFileDialog
    }
}