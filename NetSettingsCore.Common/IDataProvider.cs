using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
//using System.Drawing;
using System.Text;
using NetSettings.View;
//using NetSettings.Data;
//using NetSettings.View;
//using Color = Colourful.RGBColor;
using Color = System.Drawing.Color;

namespace NetSettingsCore.Common
{
    //public interface IDataProvider
    //{
    //    delegate void ItemChangedDelegate(ItemChangedArgs changedArgs);
    //    event ItemChangedDelegate ItemChanged;// = delegate { }; { get; set; }
    //    ItemTree RootTemplate { get; }
    //    Dictionary<string, object> DataBinding { get; set; }
    //    void AddView(IDataView dataView);
    //    void SetValue(ItemChangedArgs itemChangedArgs);
    //    object GetValueOrDefault(string name);
    //    Dictionary<string, object> GenerateDefaultOptionsSet();
    //}

    //public interface IItemTree
    //{
    //}

    //public interface IDataViewParams
    //{
    //    IDataProvider dataProvider { get; set; }
    //    IControlContainer container { get; set; }
    //    IControlContainer descriptionContainer { get; set; }
    //    IDataViewPlacement placement { get; set; }
    //}

    //public interface IDataViewPlacement
    //{
    //    int TitleMaxWidth { get; set; }
    //}

    public interface IControlContainer
    {
        void StartUpdate();
        void Reset();
        void EndUpdate();
        IList Controls { get; }
        void ResetPosition();
    }

    //public interface IDataView
    //{
    //    void Create(DataViewParams dataViewParams);
    //    void SetFilter(Filter filter, bool aCommit);
    //}

    //public interface IFilter
    //{
    //    bool IsEmpty();
    //    string IncludeName { get; set; }
    //}

    public interface IPreviewForm
    {
        void Show();
        string ImageName { get; set; }
        void Hide();
    }

    public interface ITextBox : IControl
    {
        //public ITextBox Instance { get; }
        bool Multiline { get; set; }
        DockStyle Dock { get; set; }
        bool ReadOnly { get; set; }
        BorderStyle BorderStyle { get; set; }
    }

    public interface IGuiElement
    {
    }

    public interface IControl : IGuiElement
    {
        //    public void SetFont(IFont);
        //VisualItem Tag { get; set; }
        bool Visible { get; set; }
        string Text { get; set; }
        //IList<IControl> Controls { get; set; }
        IList Controls { get; }
        int Width { get; set; }
        int Height { get; set; }
        Color BackColor { get; set; }
        Point Location { get; set; }
        IFont Font { get; set; }

        #region Events
        event EventHandler DoubleClick;
        event EventHandler MouseClick;
        event EventHandler TextChanged;
        event EventHandler Leave;
        event EventHandler SelectedIndexChanged;
        event EventHandler MouseDoubleClick;
        event EventHandler KeyDown;
        event EventHandler Click;
        event EventHandler MouseEnter;
        event EventHandler MouseLeave;
        #endregion Events
    }

    //public interface Control : IControl
    //{
    //}

    public interface IFont : IGuiElement
    {
        float Size { get; }
        FontAppearance Appearance { get; set; }
        //string Name { get; set; }
        string FontFamily { get;  }
    }

    public interface IButton : IControl
    {
        FlatStyle FlatStyle { get; set; }
        Color BackColor { get; set; }
    }

    public interface ILabelSingleClick : IControl //TODO: Rename to ILabel?
    {
        bool Visible { get; set; }
        void SetStyle(GuiElementStyles standardDoubleClick, bool value);
        Color ForeColor { get; set; }
    }

    public enum GuiElementStyles
    {
        StandardDoubleClick
    }

    public interface IComboBox : IControl
    {
        object SelectedItem { get; set; }
        //IList<string> Items { get; }
        void AddItem(string item);
    }

    public class ShowFormParams
    {

    }

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

    public enum DockStyle
    {
        Fill
    }

    //public class SaveFileDialog : FileDialog
    //{
    //}

    //public class OpenFileDialog : FileDialog
    //{
    //}

    public enum FontAppearance
    {
        Regular,
        Bold 
    }

    public class ComboBoxDoubleClick
    {
    }

    //public class Label : Control
    //{
    //    public Color ForeColor { get; set; }
    //}

    //public class CheckBox : Control
    //{
    //    public bool Checked { get; set; }
    //}

    //public class ColorControl : Control
    //{
    //    public Color Color { get; set; }
    //}

    public interface IColorDialog : IControl
    {
        object Color { get; set; }
        bool FullOpen { get; set; }
        DialogResult ShowDialog();
    }

    public enum DialogResult
    {
        OK
    }

    public class IFileDialog
    {
        private IFileDialog dialog;
        public string Filter { get; set; }
        public string InitialDirectory { get; set; }
        public string FileName { get; set; }


        public DialogResult ShowDialog()
        {
            throw new NotImplementedException();
        }
    }

    //internal class ComboBox : Control
    //{
    //    public object SelectedItem { get; set; }
    //    public IList<string> Items { get; }

    //    ComboBox()
    //    {
    //        Items = new List<string>();
    //    }
    //}

    //public class Control : IControl
    //{
    //    private IControl elem;
    //    Control(IControl elem)
    //    {
    //        this.elem = elem;
    //    }

    //    //public VisualItem Tag { get; set; } //TODO: Rename to Item
    //    public IList<Control> Controls { get; set; }
    //    public bool Visible { get; set; }
    //    public int Width { get; set; }
    //    public int Height { get; set; }
    //    public Color BackColor { get; set; }
    //    public Point Location { get; set; }
    //    public string Text { get; set; }

    //    #region Events
    //    public event EventHandler DoubleClick;
    //    public event EventHandler MouseClick;
    //    public event EventHandler TextChanged;
    //    public event EventHandler Leave;
    //    public event EventHandler SelectedIndexChanged;
    //    public event EventHandler MouseDoubleClick;
    //    public event EventHandler KeyDown;
    //    public event EventHandler Click;
    //    public event EventHandler MouseEnter;
    //    public event EventHandler MouseLeave;
    //    #endregion Events
    //}

    //public class Font : IFont
    //{
    //    //private string labelFont;
    //    //private FontAppearance regular;

    //    public Font(string labelFont, int size, FontAppearance regular = FontAppearance.Regular)
    //    {
    //        this.FontFamily = labelFont;
    //        this.Size = size;
    //        this.Appearance = regular;
    //    }

    //    public string FontFamily { get; set; }
    //    //public VisualItem Tag { get; set; }
    //    public bool Visible { get; set; }
    //    public string Text { get; set; }
    //    public int Size { get; set; }
    //    public FontAppearance Appearance { get; set; }
    //    public string Name { get; set; }
    //}

    //public class TextBox : Control, ITextBox
    //{
    //    public ITextBox Instance { get; set; }
    //    public bool Multiline { get ; set; }
    //    public DockStyle Dock { get; set; }
    //    public bool ReadOnly { get; set; }
    //    public BorderStyle BorderStyle { get; set; }
    //    public IFont Font { get; set; }

    //    public TextBox(IControl ddd)
    //    {
    //        Instance = (ITextBox)ddd;
    //    }
    //}

    //public class ControlContainer : Control
    //{
    //    private IControlContainer a;
    //    public IList<Control> Controls { get; set; }

    //    public void Reset()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void StartUpdate()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void EndUpdate()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void ResetPosition()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class LabelSingleClick : Control
    //{
    //    private bool v;

    //    public LabelSingleClick(bool v)
    //    {
    //        this.v = v;
    //    }

    //    public IFont Font { get; set; }
    //}

    public enum BorderStyle
    {
        FixedSingle
    }

    //public class Button : Control, IButton
    //{
    //    public FlatStyle FlatStyle { get; set; }
    //    public Color BackColor { get; set; }
    //}



    public enum FlatStyle
    {
        Popup
    }
    //class WinFormGuiProvider : IGuiProvider
    //{
    //    IControl CreateGuiElement(string guiElementName)
    //    {
    //        switch (guiElementName)
    //        {
    //            case "font":
    //                return IFont.GetType();

    //        }
    //    }
    //}

    public interface IColorControl : IControl
    {
        Color Color { get; set; }
    }

    public interface IComboBoxDoubleClick
    {
    }

    public interface ICheckBox : IControl
    {
        bool Checked { get; set; }
    }
}
