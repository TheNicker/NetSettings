//using NetSettings.Controls;
using NetSettings.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
//using System.Windows.Forms;
//using NetSettings.Controls;
//using NetSettings.Forms;
//using NetSettings.WinForms.Controls;
//using System.Windows.Forms;
//using NetSettings.Forms;
using NetSettingsCore.Common;
//using ILabel = System.Windows.Forms.ILabel;

namespace NetSettings.View
{
    public class DataView// : IDataView
    {
        private const string labelFont = "calibri";

        //private Dictionary<string, Type> fStringToType;

        //Controls arrangement
        private Point fCurrentPanelPosition;
        private int fNesting = 0;
        private int fCurrentRow;

        private IControlContainer fDescriptionPanel;
        private ITextBox fDescriptionTextBox;
        private DataViewParams fParams;

        private VisualItem fRootVisualItem;

        private IPreviewForm fPreviewForm;
        private Point fLastCursorPosition;

        private IFont fLabelNormal;
        private IFont fLabelBold;
        private IFont fMenuLabel;

        class Font
        {
            //IFont font = IGuiProvider.CreateGuiElemtn(Font))
            //Font(Size, style)
            //{
                
            //}
        }

        public DataView(IGuiProvider guiProvider)
        {

            //fStringToType = new Dictionary<string, Type>
            //{
            //    {"text", guiProvider.getElementType(GuiElementType.ITextBox ) },
            //    {"bool",  guiProvider.getElementType(GuiElementType.ICheckBox)},
            //    {"menu",  guiProvider.getElementType(GuiElementType.ILabel)},
            //    {"combo", guiProvider.getElementType(GuiElementType.ComboBoxDoubleClick)},
            //    {"image", guiProvider.getElementType(GuiElementType.ITextBox)},
            //    {"number",guiProvider.getElementType(GuiElementType.ITextBox)},
            //    {"color", guiProvider.getElementType(GuiElementType.ColorControl)},
            //    {"font", guiProvider.getElementType(GuiElementType.IFont)}
            //};

            createFont(10, FontStyle.Regular, labelFont);
            var fLabelNormal = guiProvider.CreateGuiElement("font") as IFont;
            fLabelNormal.Size = 10;
            fLabelNormal.Style = FontStyle.Regular;
            fLabelNormal.Name = labelFont;


            fLabelNormal = new Font(labelFont, 10, FontStyle.Regular);

            fLabelNormal = new IFont(labelFont, 10, FontStyle.Regular);
            fLabelBold = new IFont(labelFont, 10, FontStyle.Bold);
            fMenuLabel = new IFont(labelFont, 12, FontStyle.Bold);
            fPreviewForm = new PreviewForm();
        }

        public void Create(DataViewParams aParams)
        {
            VerifyParameters((DataViewParams)aParams);
            fParams = (DataViewParams)aParams;
            fParams.dataProvider.AddView(this);
            CreateVisualItemTree();
            fDescriptionPanel = (ControlContainer)fParams.descriptionContainer;

            if (fDescriptionPanel != null)
            {
                fDescriptionPanel.Reset();
                fDescriptionPanel.StartUpdate();
                ITextBox t = new ITextBox();
                t.Multiline = true;
                t.Dock = DockStyle.Fill;
                t.ReadOnly = true;
                t.BorderStyle = BorderStyle.FixedSingle;
                t.Font = IGuiProvider.cre new IFont("Lucida fax", 10);
                fDescriptionTextBox = t;
                fDescriptionPanel.Controls.Add(t);
                fDescriptionPanel.EndUpdate();
            }

            ReCreateTree();
        }

        private void VerifyParameters(DataViewParams aParams)
        {
            if (aParams.container == null || aParams.dataProvider == null)
                throw new Exception("Missing parameters, can not build data view tree");
        }

        #region Recreate Tree

        private void CreateVisualItemTree()
        {
            fRootVisualItem = new VisualItem
            {
                IsFiltered = true,
                Item = (ItemTree)fParams.dataProvider.RootTemplate
            };
            CreateVisualItemTree(fRootVisualItem);
        }

        private void CreateVisualItemTree(VisualItem rootVisualItem)
        {
            ItemTree item = rootVisualItem.Item;
            if (item.subItems != null)
            {
                rootVisualItem.subItems = new VisualItem[item.subItems.Length];
                int i = 0;//TODO: Can we remove this?
                foreach (var subItem in item.subItems)
                {
                    var visualItem = new VisualItem
                    {
                        Item = subItem,
                        IsFiltered = true
                    };
                    rootVisualItem.subItems[i++] = visualItem;
                    CreateVisualItemTree(visualItem);
                }
            }
        }

        public void ReCreateTree()
        {
            fParams.container.StartUpdate();
            fParams.container.Reset();
            AddControlRecursivly(fRootVisualItem);
            ReArrange();
            fParams.container.EndUpdate();
        }

        private void AddControlRecursivly(VisualItem aRoot, GuiElementType guiElementType)
        {
            VisualItem visualItem = aRoot;
            ItemTree item = aRoot.Item;
            if (!visualItem.IsFiltered)
                return;
            //Type type;
            //if (fStringToType.TryGetValue(item.type, out type))
            GuiElementType.TryParse(item.type, true, out guiElementType);
                AddControl(aRoot, guiElementType);

            //Add children
            if (visualItem.subItems != null)
                foreach (VisualItem subItem in visualItem.subItems)
                    AddControlRecursivly(subItem);
        }

        private void AddControl(VisualItem aVisualItem, GuiElementType aType)
        {
            ItemTree aItem = aVisualItem.Item;

            bool isMenu = aItem.type == "menu";
            bool isBool = aItem.type == "bool";
            //Create parent container
            ItemControlsGroup group = new ItemControlsGroup();
            IGuiElement parent = group.parentContainer = guirpvoider.createelement(GuiElementType.IGuiElement);

            ((ControlContainer)(fParams.container)).Controls.Add(parent);
            parent.Tag = aVisualItem;

            //Add label describing the entry

            LabelSingleClick label = group.label = new LabelSingleClick(!isBool);
            label.IFont = GetLabelFont(aVisualItem);
            label.Text = aItem.displayName;
            label.Tag = aVisualItem;
            parent.AddChildControl(label);
            //parent.Controls.Add(label);

            //Add the  control itself
            IGuiElement control = group.control = guirpvoider.createelement(aType) as IGuiElement; // Activator.CreateInstance(aType) as IGuiElement;
            //parent.Controls.Add(control);
            parent.AddChildControl(control);
            control.Tag = aVisualItem;

            //Add reference from the menu item to the control holding the values.
            aVisualItem.controlsGroup = group;

            //Add a default button 
            if (!isMenu)
            {
                IButton button = group.defaultButton = new IButton();
                parent.Controls.Add(button);
                button.Text = "Default";
                button.Click += button_Click;
                button.Tag = aVisualItem;
                button.FlatStyle = FlatStyle.Popup;
                button.BackColor = System.Drawing.SystemColors.IGuiElement;
                fCurrentRow++;
            }

            MouseEnterLeave l = new MouseEnterLeave(parent);
            l.MouseEnter += l_MouseEnter;
            l.MouseLeave += panel_MouseLeave;
            ProceeControl(aVisualItem);
        }
        private void ProceeControl(VisualItem aVisualItem)
        {
            PrepareControl(aVisualItem);
            RefreshControlValue(aVisualItem);
            ProcessEvents(aVisualItem);
        }

        private static void PrepareControl(VisualItem aVIsualItem)
        {

            IGuiElement label = aVIsualItem.controlsGroup.label;
            IGuiElement actualControl = aVIsualItem.controlsGroup.control;
            ItemTree aItem = aVIsualItem.Item;
            switch (aItem.type)
            {
                case "combo":
                    string[] values = aItem.values.Split(';');
                    foreach (string v in values)
                        (actualControl as IComboBox).Items.Add(v);
                    break;
                case "menu":
                    (label as ILabel).ForeColor = Color.LawnGreen;
                    break;
            }
        }

        private void ProcessEvents(VisualItem aVisualItem)
        {

            IGuiElement t = aVisualItem.controlsGroup.control;
            IGuiElement p = aVisualItem.controlsGroup.parentContainer;
            IGuiElement l = aVisualItem.controlsGroup.label;
            ItemTree aItem = aVisualItem.Item;
            switch (aItem.type)
            {
			//TODO: Remove all the casting in this switch
                case "menu":
                    l.DoubleClick += Menu_DoubleClick;
                    break;
                case "bool":
                    t.MouseClick += CheckBox_MouseClick;
                    p.MouseClick += CheckBox_MouseClick;
                    l.MouseClick += CheckBox_MouseClick;
                    break;
                case "text":
                    t.TextChanged += MenuSettings_TextChanged;
                    t.Leave += DataView_Leave;
                    break;
                case "number":
                    t.TextChanged += MenuSettings_NumberChanged;
                    t.Leave += Number_Leave;
                    break;
                case "combo":
                    t.SelectedIndexChanged += c_SelectedIndexChanged;
                    t.MouseDoubleClick += Combo_MouseDoubleClick;
                    break;
                case "image":
                    t.TextChanged += MenuSettings_TextChanged;
                    t.MouseDoubleClick += MenuSettings_MouseDoubleClick;
                    t.MouseLeave += MenuSettings_MouseLeave;
                    t.MouseEnter += MenuSettings_MouseEnter;
                    break;
                case "color":
                    t.KeyDown += ColorControl_KeyDown;
                    t.TextChanged += ColorControl_TextChanged;
                    t.DoubleClick += MenuSettings_Click;
                    p.Click += MenuSettings_Click;
                    l.Click += MenuSettings_Click;
                    break;
            }

        }

        private void Menu_DoubleClick(object sender, EventArgs e)
        {
            VisualItem item = GetItemFromControl(sender as IGuiElement);
            if (item != null)
                item.Expanded = !item.Expanded;

            ReArrange();
        }
        #endregion

        #region ReArrange Tree
        private void ReArrangeRecurseivly(VisualItem aRoot)
        {

            VisualItem visualItem = aRoot;
            ItemTree item = aRoot.Item;
            ReArrange(visualItem);
            if (visualItem.subItems != null && (visualItem.Expanded == true || (fParams.filter != null && fParams.filter.IsEmpty() == false)))
                foreach (VisualItem subItem in visualItem.subItems)
                    ReArrangeRecurseivly(subItem);

        }

        public void ReArrange()
        {
            fCurrentPanelPosition = new Point();
            fCurrentRow = 0;
            fParams.container.ResetPosition();
            fParams.container.StartUpdate();

            HideAllControls();
            ApplyFilterRecursively(fRootVisualItem);
            ReArrangeRecurseivly(fRootVisualItem);

            fParams.container.EndUpdate();
        }

        private void HideAllControls()
        {
            HideAllControlsRecursively(fRootVisualItem);
        }

        private void HideAllControlsRecursively(VisualItem aRoot)
        {
            if (aRoot.controlsGroup != null)
                aRoot.controlsGroup.Visible = false;

            if (aRoot.subItems != null)
                foreach (VisualItem subItem in aRoot.subItems)
                    HideAllControlsRecursively(subItem);
        }

        private void ReArrange(VisualItem aVisualItem)
        {
            if (!aVisualItem.IsFiltered || aVisualItem.Item.type == "root")
            {
                if (aVisualItem.controlsGroup != null)
                    aVisualItem.controlsGroup.Visible = false;
                return;
            }

            aVisualItem.controlsGroup.Visible = true;

            ItemTree aItem = aVisualItem.Item;
            DataViewPlacement p = (DataViewPlacement)fParams.placement;
            bool isMenu = aItem.type == "menu";
            //Create parent container
            ItemControlsGroup group = aVisualItem.controlsGroup;
            IGuiElement parent = aVisualItem.controlsGroup.parentContainer;

            parent.Width = p.TitleMaxWidth + p.TitleSpacing + p.ControlMaxWidth + p.ControlSpacing + p.DefaultButtonWidth;
            parent.Height = p.LineSpacing;
            aVisualItem.PanelBackgroundColor = isMenu ? Color.DarkBlue : fCurrentRow % 2 == 0 ? Color.White : Color.LightGray;
            parent.BackColor = aVisualItem.PanelBackgroundColor;

            fCurrentPanelPosition.X = p.HorizontalMargin * fNesting;
            parent.Location = fCurrentPanelPosition;
            fCurrentPanelPosition.Y += p.LineSpacing;
            Point controlPosition = new Point(0, (p.LineSpacing - p.LineHeight) / 2);
            LabelSingleClick label = group.label;
            label.Width = p.TitleMaxWidth;
            label.Height = p.LineHeight;
            label.Location = controlPosition;
            controlPosition.X = p.TitleMaxWidth + p.TitleSpacing;
            IGuiElement control = group.control;
            control.Location = controlPosition;
            control.Height = p.LineHeight;
            control.Width = p.ControlMaxWidth;
            controlPosition.X += p.ControlMaxWidth + p.ControlSpacing;

            //Add a default button 
            if (!isMenu)
            {
                Button button = group.defaultButton;
                button.Width = p.DefaultButtonWidth;
                button.Height = p.LineHeight;
                button.Location = controlPosition;
                fCurrentRow++;
            }
        }

        public void SetFilter(Filter aFilter, bool aCommit = true)
        {
            fParams.filter = aFilter;
            if (aCommit)
                ReArrange();
        }

        private bool ApplyFilterRecursively(VisualItem root)
        {
            VisualItem visualItem = root;
            ItemTree item = root.Item;

            if (fParams.filter == null || String.IsNullOrEmpty(fParams.filter.IncludeName) || String.IsNullOrWhiteSpace(fParams.filter.IncludeName))
            {
                visualItem.IsFiltered = true;
            }
            else
            {
                if (item.type == "menu" || item.type == "root")
                {
                    visualItem.IsFiltered = true;
                }
                else
                    if (item.displayName != null)
                {
                    visualItem.IsFiltered = item.displayName.ToLower().Contains(fParams.filter.IncludeName.ToLower());
                }
            }

            if (item.subItems != null)
            {
                bool isFiltered = false;
                foreach (VisualItem subItem in visualItem.subItems)
                    isFiltered |= ApplyFilterRecursively(subItem);

                //if at least one of the childs is visible then the parent is visible as well.
                visualItem.IsFiltered = isFiltered;

            }
            return visualItem.IsFiltered;
        }

        private void CheckLabelColor(VisualItem aVisualItem)
        {
            aVisualItem.controlsGroup.label.IFont = GetLabelFont(aVisualItem);
        }
        #endregion

        private IFont GetLabelFont(VisualItem aVisualItem)
        {
            if (aVisualItem.Item.type == "menu")
                return fMenuLabel;

            ItemTree item = aVisualItem.Item;
            object val = GetValue(aVisualItem.Item);
            return item.defaultValue != null && val != null && !item.defaultValue.Equals(val)
                ? fLabelBold : fLabelNormal;
            //fLabelNormal->SetFont
        }

        void button_Click(object sender, EventArgs e)
        {
            IGuiElement c = sender as IGuiElement;
            VisualItem item = GetItemFromControl(c);
            if (item != null)
                SetValue(item, item.Item.defaultValue);
        }

        void l_MouseEnter(object sender, EventArgs e)
        {
            VisualItem item = (sender as IGuiElement).Tag as VisualItem;
            if (item != null)
            {
                if (item.Item.description != null && fDescriptionTextBox != null)
                    fDescriptionTextBox.Text = item.Item.description;

                if (item.Item.type != "menu")
                {
                    item.controlsGroup.parentContainer.BackColor = Color.YellowGreen;
                }
            }
        }

        void panel_MouseLeave(object sender, EventArgs e)
        {
            VisualItem item = (sender as IGuiElement).Tag as VisualItem;
            if (item != null)
            {
                if (item.Item.type != "menu")
                {
                    IGuiElement container = item.controlsGroup.parentContainer;
                    container.BackColor = item.PanelBackgroundColor;

                }
            }
        }

        private void RefreshControlValueRecursivly(VisualItem aRoot)
        {
            RefreshControlValue(aRoot);
            if (aRoot.subItems != null)
                foreach (VisualItem subItem in aRoot.subItems)
                    RefreshControlValueRecursivly(subItem);
        }

        private void RefreshControlValue(VisualItem aVisualItem)
        {
            ItemTree item = aVisualItem.Item;
            if (aVisualItem.controlsGroup != null)
            {
                IGuiElement aControl = aVisualItem.controlsGroup.control;
                object val = GetValue(item);
                switch (item.type)
                {
                    case "bool":
                        bool _val = false;
                        if (val != null)
                            _val = (bool)val;
                        (aControl as ICheckBox).Checked = _val;
                        break;
                    case "text":

                        (aControl as ITextBox).Text = (val != null ? (string)val : "");
                        break;
                    case "number":
                        (aControl as ITextBox).Text = (val != null ? ToDoubleString(((double)val)) : "");
                        break;
                    case "combo":
                        (aControl as IComboBox).SelectedItem = val;
                        break;
                    case "image":
                        (aControl as ITextBox).Text = (val != null ? val as string : "");
                        break;
                    case "color":
                        (aControl as ColorControl).color = (val != null ? (Color)val : Color.Empty);

                        break;
                }
                CheckLabelColor(aVisualItem);

            }
        }

        private void ColorControl_KeyDown(object sender, EventArgs e)
        {
            ColorControl colorControl = sender as ColorControl;
            RefreshControlValue((colorControl.Tag as VisualItem));
        }

        private void ColorControl_TextChanged(object sender, EventArgs e)
        {
            ColorControl colorControl = sender as ColorControl;
            Color c;
            if (NetSettings.View.DataViewHelper.TryGetColor(colorControl.Text, out c))
                SetValue((sender as IGuiElement).Tag as VisualItem, c, ItemChangedMode.OnTheFly);
        }

        private void CheckBox_MouseClick(object sender, EventArgs e)
        {
            IGuiElement control = sender as IGuiElement;
            VisualItem item = control.Tag as VisualItem;
            ICheckBox checkBox = item.controlsGroup.control as ICheckBox;
            if (!(control is ICheckBox))
                checkBox.Checked = !checkBox.Checked;

            SetValue(item, checkBox.Checked);
        }

        private object GetValueFromControl(VisualItem aVisualItem)
        {
            object result = null;
            switch (aVisualItem.Item.type)
            {
                case "text":
                    result = aVisualItem.controlsGroup.control.Text;
                    break;
                case "number":
                    double value;
                    if (double.TryParse(aVisualItem.controlsGroup.control.Text, out value))
                        result = value;
                    break;
            }
            return result;
        }

        private void Number_Leave(object sender, EventArgs e)
        {
            Leave(sender);
        }

        void DataView_Leave(object sender, EventArgs e)
        {
            Leave(sender);
        }

        private void Leave(object sender)
        {
            ITextBox textBox = sender as ITextBox;
            VisualItem visualItem = GetItemFromControl(textBox);
            object value = GetValueFromControl(visualItem);
            SetValue(visualItem, value, ItemChangedMode.UserConfirmed);
        }

        private void SetValue(VisualItem aVisualItem, object aVal, ItemChangedMode aMode = ItemChangedMode.UserConfirmed)
        {

            if (aVisualItem != null)
            {
                fParams.dataProvider.SetValue(
                    new ItemChangedArgs()
                    {
                        sender = this,
                        ChangedMode = aMode,
                        Key = aVisualItem.Item.FullName,
                        Val = aVal
                    });


                // Get the data back from the DataProvider
                RefreshControlValue(aVisualItem);
            }
        }

        private string ToDoubleString(double num)
        {
            return num.ToString("0.################");
        }

        private object GetValue(string name)
        {
            return fParams.dataProvider.GetValueOrDefault(name);
        }

        private object GetValue(ItemTree item)
        {
            return GetValue(item.FullName);
        }

        private void Combo_MouseDoubleClick(object sender, EventArgs e)
        {
            IComboBox comboBox = sender as IComboBox;
            VisualItem item = GetItemFromControl(comboBox);
            SetValue(item, comboBox.SelectedItem);
        }

        void MenuSettings_Click(object sender, EventArgs e)
        {
            IGuiElement p = sender as IGuiElement;
            var color =  (p as IColorControl).ShowColorDialog();
            SetValue(item, color);

            ColorDialog dialog;
            VisualItem item = GetItemFromControl(p);
            ColorControl colorControl = item.controlsGroup.control as ColorControl;
            if ((dialog = new ColorDialog() { FullOpen = true, Color = (Color)GetValue(item.Item.FullName) }).ShowDialog() == DialogResult.OK)
                SetValue(item, dialog.Color);
        }

        private void MenuSettings_NumberChanged(object sender, EventArgs e)
        {
            ITextBox textbox = sender as ITextBox;
            VisualItem item = GetItemFromControl(textbox);
            double num;
            if (double.TryParse(textbox.Text, out num))
            {
                // check if user is inserting the decimal point of a number while trying to define the fractional part of a number
                // if yes don't update the number yet since the parsing from string to double will omit the decimal point.
                string testString = ToDoubleString(num);
                if (testString == textbox.Text)
                    SetValue(item, num, ItemChangedMode.OnTheFly);
            }
        }

        private void MenuSettings_MouseEnter(object sender, EventArgs e)
        {
            var textBox = (ITextBox)sender;
            if (!string.IsNullOrEmpty(textBox.Text))
            {
                Task.Delay(500).ContinueWith(_ =>
                  {
                      textBox.Invoke((MethodInvoker)delegate
                   {
                       fPreviewForm.Show();
                       fPreviewForm.ImageName = textBox.Text;
                   });
                  });
            }
        }

        private void MenuSettings_MouseLeave(object sender, EventArgs e)
        {
                    fPreviewForm.Hide(); 
        }

        public static string ChooseFile(bool aOpen, string aFilter, string aPath)
        {
            string strResult = null;
            FileDialog dialog;
            if (aOpen)
                dialog = new OpenFileDialog();
            else
                dialog = new SaveFileDialog();

            dialog.Filter = aFilter;
            dialog.InitialDirectory = aPath;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                strResult = dialog.FileName;
                string directory = System.IO.Path.GetDirectoryName(strResult);//TODO: Remove or what???
            }
            return strResult;
        }

        void MenuSettings_MouseDoubleClick(object sender, EventArgs e)
        {
            string imageName = ChooseFile(true, "", "");
            if (imageName != null)
            {
                ITextBox t = sender as ITextBox;
                VisualItem item = GetItemFromControl(t);
                SetValue(item, t.Text = imageName);
            }
        }

        void c_SelectedIndexChanged(object sender, EventArgs e)
        {
            IComboBox comboBox = sender as IComboBox;
            VisualItem item = GetItemFromControl(comboBox);
            SetValue(item, comboBox.SelectedItem);
        }

        void MenuSettings_TextChanged(object sender, EventArgs e)
        {
            var textbox = sender as ITextBox;
            var item = GetItemFromControl(textbox);
            SetValue(item, textbox.Text, ItemChangedMode.OnTheFly);

        }

        VisualItem GetItemFromControl(IGuiElement aControl)
        {
            return aControl.Tag as VisualItem;
        }

        public void RefreshViewFromData()
        {
            RefreshControlValueRecursivly(fRootVisualItem);
        }
    }

    public class SaveFileDialog : FileDialog
    {
    }

    public class OpenFileDialog : FileDialog
    {
    }

    public enum DockStyle
    {
        Fill
    }

    public enum FontStyle
    {
        Regular,
        Bold
    }

    internal interface ICheckBox
    {
        bool Checked { get; set; }
    }
}
