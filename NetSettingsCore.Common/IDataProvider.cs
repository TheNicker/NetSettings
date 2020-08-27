//using System.Drawing;
//using System.Drawing;
//using NetSettings.Data;
//using NetSettings.View;
//using Color = Colourful.RGBColor;
//using Color = Color;

namespace NetSettings.Common
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

    //public interface Control : IControl
    //{
    //}

    //public class SaveFileDialog : FileDialog
    //{
    //}

    //public class OpenFileDialog : FileDialog
    //{
    //}



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

    //public class Button : Control, IButton
    //{
    //    public FlatStyle FlatStyle { get; set; }
    //    public Color BackColor { get; set; }
    //}


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
}
