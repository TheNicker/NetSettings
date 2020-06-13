//using NetSettings.Controls;
using NetSettings.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
//using System.Drawing;
using NetSettings.Controls;
using NetSettings.WinForms.Controls;
using NetSettingsCore.Common;
using BorderStyle = NetSettingsCore.Common.BorderStyle;
using ControlStyles = NetSettingsCore.Common.GuiElementStyles;
using DialogResult = NetSettingsCore.Common.DialogResult;
using DockStyle = NetSettingsCore.Common.DockStyle;
using FlatStyle = NetSettingsCore.Common.FlatStyle;
//using Button = NetSettingsCore.Common.Button;
using CheckBox = NetSettingsCore.Common.ICheckBox;
using ComboBox = NetSettingsCore.Common.IComboBox;
//using Control = NetSettingsCore.Common.Control;
//using DialogResult = NetSettingsCore.Common.DialogResult;
//using DockStyle = NetSettingsCore.Common.DockStyle;
//using FileDialog = NetSettingsCore.Common.FileDialog;
//using FlatStyle = NetSettingsCore.Common.FlatStyle;
//using Font = NetSettingsCore.Common.Font;
//using FontAppearance = NetSettingsCore.Common.FontAppearance;
//using Label = NetSettingsCore.Common.ILabel;
//using OpenFileDialog = NetSettingsCore.Common.OpenFileDialog;
//using SaveFileDialog = NetSettingsCore.Common.SaveFileDialog;

//using System.Windows.Forms;
//using NetSettings.Forms;
using Point = System.Drawing.Point;

namespace NetSettings.View
{
    public class DataView
    {
        private const string labelFont = "calibri"; //TODO: Rename to familyName

        private Dictionary<string, Type> fStringToType; //TODO: Delete this field

        //Controls arrangement
        private Point fCurrentPanelPosition;
        private readonly int fNesting = 0;
        private int fCurrentRow;

        private IControlContainer fDescriptionPanel;
        private ITextBox fDescriptionTextBox;
        private DataViewParams fParams;

        private VisualItem fRootVisualItem;

        //private PreviewForm fPreviewForm;
        public IGuiProvider guiProvider { get; set; }
        private readonly Point fLastCursorPosition;

        private IFont fLabelNormal;
        private IFont fLabelBold;
        private IFont fMenuLabel;

        private readonly Dictionary<IGuiElement, VisualItem> dic = new Dictionary<IGuiElement, VisualItem>();

        public DataView()
        {
            fStringToType = new Dictionary<string, Type>
            {
                {"text", typeof(ITextBox)},
                {"bool", typeof(ICheckBox)},
                {"menu", typeof(ILabelSingleClick)},
                {"combo", typeof(IComboBoxDoubleClick)},
                {"image", typeof(ITextBox)},
                {"number", typeof(ITextBox)},
                {"color", typeof(IColorControl)}
            };

            //fPreviewForm = new PreviewForm();
        }

        public void Create(DataViewParams aParams)
        {
            guiProvider = aParams.guiProvider;
            VerifyParameters(aParams);
            fParams = aParams;

            //fLabelNormal = new Font(labelFont, 10, FontAppearance.Regular);
            fLabelNormal = (IFont)guiProvider.CreateGuiElement(GuiElementType.IFont, labelFont, 10f, FontAppearance.Regular);
            //fLabelNormal.FontFamily = labelFont;
            //fLabelNormal.Size = 10;
            //fLabelNormal.Appearance = FontAppearance.Regular;
            //fLabelBold = new Font(labelFont, 10, FontAppearance.Bold);
            fLabelBold = (IFont)guiProvider.CreateGuiElement(GuiElementType.IFont, labelFont, 10f, FontAppearance.Bold);
            //fLabelNormal.FontFamily = labelFont;
            //fLabelNormal.Size = 10;
            //fLabelNormal.Appearance = FontAppearance.Bold;
            //fMenuLabel = new Font(labelFont, 12, FontAppearance.Bold);
            fMenuLabel = (IFont)guiProvider.CreateGuiElement(GuiElementType.IFont, labelFont, 12f, FontAppearance.Bold);
            //fLabelNormal.FontFamily = labelFont;
            //fLabelNormal.Size = 12;
            //fLabelNormal.Appearance = FontAppearance.Bold;


            fParams.dataProvider.AddView(this);
            CreateVisualItemTree();
            fDescriptionPanel = fParams.descriptionContainer;

            if (fDescriptionPanel != null)
            {
                fDescriptionPanel.Reset();
                fDescriptionPanel.StartUpdate();
                //var textBox = new TextBox(guiProvider.CreateGuiElement("textbox"));
                var textBox = (ITextBox)guiProvider.CreateGuiElement(GuiElementType.Text);
                textBox.Multiline = true;
                textBox.Dock = DockStyle.Fill;
                textBox.ReadOnly = true;
                textBox.BorderStyle = BorderStyle.FixedSingle;
                textBox.Font = (IFont)guiProvider.CreateGuiElement(GuiElementType.IFont, "Lucida Fax", 10f);
                //textBox.Font.FontFamily = "Lucida fax";
                //textBox.Font.Size = 10;

                fDescriptionTextBox = textBox;
                fDescriptionPanel.Controls.Add(textBox); //.Instance
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
                Item = fParams.dataProvider.RootTemplate
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

        private void AddControlRecursivly(VisualItem aRoot)
        {
            VisualItem visualItem = aRoot;
            ItemTree item = aRoot.Item;
            if (!visualItem.IsFiltered)
                return;

            //if (fStringToType.TryGetValue(item.type, out var type))
            if (Enum.TryParse(typeof(GuiElementType), item.type, true, out var elem))
            {
                //AddControl(aRoot, type);
                AddControl(aRoot, elem);
            }

            //Add children
            if (visualItem.subItems != null)
                foreach (var subItem in visualItem.subItems)
                    AddControlRecursivly(subItem);
        }

        //private void AddControl(VisualItem aVisualItem, Type aType)
        private void AddControl(VisualItem aVisualItem, object aType)
        {
            ItemTree aItem = aVisualItem.Item;

            var isMenu = aItem.type == "menu";
            var isBool = aItem.type == "bool";
            //Create parent container
            var group = new ItemControlsGroup();
            var parent = group.parentContainer = guiProvider.CreateGuiElement(GuiElementType.GuiElement) as IControl;

            fParams.container.Controls.Add(parent);
            dic.Add(parent, aVisualItem);

            //Add label describing the entry

            var label = group.label = (ILabelSingleClick)guiProvider.CreateGuiElement(GuiElementType.Label);
            label.Font = GetLabelFont(aVisualItem);
            label.Text = aItem.displayName;
            label.SetStyle(ControlStyles.StandardDoubleClick, !isBool);
            parent.Controls.Add(label);
            dic.Add(label, aVisualItem);

            //Add the  control itself
            var control = group.control = guiProvider.CreateGuiElement(aType) as IControl;
            //var control = group.control = Activator.CreateInstance(aType) as Control;
            parent.Controls.Add(control);
            dic.Add(control, aVisualItem);

            //Add reference from the menu item to the control holding the values.
            aVisualItem.controlsGroup = group;

            //Add a default button 
            if (!isMenu)
            {
                var button = group.defaultButton = guiProvider.CreateGuiElement(GuiElementType.Button) as IButton;
                button.Text = "Default";
                button.Click += button_Click;
                //button.Tag = aVisualItem;
                button.FlatStyle = FlatStyle.Popup;
                button.BackColor = SystemColors.Control;
                parent.Controls.Add(button);
                dic.Add(button, aVisualItem);
                fCurrentRow++;
            }

            //TODO: WTF?????
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

            var label = aVIsualItem.controlsGroup.label;
            var actualControl = aVIsualItem.controlsGroup.control;
            var aItem = aVIsualItem.Item;
            switch (aItem.type)
            {
                case "combo":
                    string[] values = aItem.values.Split(';');
                    foreach (string v in values)
                        (actualControl as IComboBox).AddItem(v);
                    break;
                case "menu":
                    (label as ILabelSingleClick).ForeColor = Color.LawnGreen;
                    break;
            }
        }

        private void ProcessEvents(VisualItem aVisualItem)
        {

            var t = aVisualItem.controlsGroup.control;
            var p = aVisualItem.controlsGroup.parentContainer;
            var l = aVisualItem.controlsGroup.label;
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
            VisualItem item = GetItemFromControl(sender as IControl);
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
            DataViewPlacement p = fParams.placement;
            bool isMenu = aItem.type == "menu";
            //Create parent container
            ItemControlsGroup group = aVisualItem.controlsGroup;
            IControl parent = aVisualItem.controlsGroup.parentContainer;

            parent.Width = p.TitleMaxWidth + p.TitleSpacing + p.ControlMaxWidth + p.ControlSpacing + p.DefaultButtonWidth;
            parent.Height = p.LineSpacing;
            aVisualItem.PanelBackgroundColor = isMenu ? Color.DarkBlue : fCurrentRow % 2 == 0 ? Color.White : Color.LightGray;
            parent.BackColor = aVisualItem.PanelBackgroundColor;

            fCurrentPanelPosition.X = p.HorizontalMargin * fNesting;
            parent.Location = fCurrentPanelPosition;
            fCurrentPanelPosition.Y += p.LineSpacing;
            Point controlPosition = new Point(0, (p.LineSpacing - p.LineHeight) / 2);
            var label = group.label;
            label.Width = p.TitleMaxWidth;
            label.Height = p.LineHeight;
            label.Location = controlPosition;
            controlPosition.X = p.TitleMaxWidth + p.TitleSpacing;
            IControl control = group.control;
            control.Location = controlPosition;
            control.Height = p.LineHeight;
            control.Width = p.ControlMaxWidth;
            controlPosition.X += p.ControlMaxWidth + p.ControlSpacing;

            //Add a default button 
            if (!isMenu)
            {
                var button = group.defaultButton;
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
            aVisualItem.controlsGroup.label.Font = GetLabelFont(aVisualItem);
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
        }

        void button_Click(object sender, EventArgs e)
        {
            IControl c = sender as IControl;
            VisualItem item = GetItemFromControl(c);
            if (item != null)
                SetValue(item, item.Item.defaultValue);
        }

        void l_MouseEnter(object sender, EventArgs e)
        {
            dic.TryGetValue(sender as IControl, out var item);
            if (item != null)
            {
                if (item.Item.description != null && fDescriptionTextBox != null)
                {
                    fDescriptionTextBox.Text = item.Item.description;
                }

                if (item.Item.type != "menu")
                {
                    item.controlsGroup.parentContainer.BackColor = Color.YellowGreen;
                }
            }
        }

        void panel_MouseLeave(object sender, EventArgs e)
        {
            dic.TryGetValue(sender as IControl, out var item);
            if (item != null)
            {
                if (item.Item.type != "menu")
                {
                    IControl container = item.controlsGroup.parentContainer;
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
                IControl aControl = aVisualItem.controlsGroup.control;
                object val = GetValue(item);
                switch (item.type)
                {
                    case "bool":
                        var _val = false;
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
                        (aControl as IColorControl).Color = (Color?) val ?? Color.Empty;
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
            if (DataViewHelper.TryGetColor(colorControl.Text, out c))
                SetValue(GetItemFromControl(sender), c, ItemChangedMode.OnTheFly);
        }

        private void CheckBox_MouseClick(object sender, EventArgs e)
        {
            var control = sender as IControl;
            var item = GetItemFromControl(control);
            var checkBox = item.controlsGroup.control as ICheckBox;
            if (!(control is CheckBox))
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
            var textBox = sender as ITextBox;
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
            ComboBox comboBox = sender as ComboBox;
            VisualItem item = GetItemFromControl(comboBox);
            SetValue(item, comboBox.SelectedItem);
        }

        void MenuSettings_Click(object sender, EventArgs e)
        {
            IControl p = sender as IControl;
            VisualItem item = GetItemFromControl(p);
            var dialog = guiProvider.CreateGuiElement(GuiElementType.ColorDialog) as IColorDialog;
            dialog.FullOpen = true;
            dialog.Color = (Color) GetValue(item.Item.FullName);
            ColorControl colorControl = item.controlsGroup.control as ColorControl;
            if (dialog.ShowDialog() == DialogResult.OK)
                SetValue(item, dialog.Color);
        }

        private void MenuSettings_NumberChanged(object sender, EventArgs e)
        {
            var textbox = sender as ITextBox;
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
            var textBox = sender as ITextBox;
            if (!string.IsNullOrEmpty(textBox.Text))
            {
                guiProvider.ShowPreviewForm(null);                //TODO: Make sure this code works

                //Task.Delay(500).ContinueWith(_ =>
                //{
                //TODO: Make sure this code works

                //   textBox.Invoke((MethodInvoker)delegate
                //{
                //    fPreviewForm.Show();
                //    fPreviewForm.ImageName = textBox.Text;
                //});
                //});
            }
        }



        private void MenuSettings_MouseLeave(object sender, EventArgs e)
        {
            //fPreviewForm.Hide();
            //guiProvider.CancelPrevireRequest({ 500, 800, 600});//TODO: Make sure this code works
        }

        private string ChooseFile(bool aOpen, string aFilter, string aPath)
        {
            string strResult = null;
            IFileDialog dialog;
            if (aOpen)
                dialog = guiProvider.CreateGuiElement(GuiElementType.OpenFileDialog) as IFileDialog;
            else
                dialog = guiProvider.CreateGuiElement(GuiElementType.SaveFileDialog) as IFileDialog;

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
                var textBox = sender as ITextBox;
                var item = GetItemFromControl(textBox);
                SetValue(item, textBox.Text = imageName);
            }
        }

        void c_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            var item = GetItemFromControl(comboBox);
            SetValue(item, comboBox.SelectedItem);
        }

        void MenuSettings_TextChanged(object sender, EventArgs e)
        {
            var textbox = sender as ITextBox;
            var item = GetItemFromControl(textbox);
            SetValue(item, textbox.Text, ItemChangedMode.OnTheFly);

        }

        private VisualItem GetItemFromControl(object aControl)
        {
            return GetItemFromControl(aControl as IControl);
        }

        private VisualItem GetItemFromControl(IControl aControl)
        {
            return dic.GetValueOrDefault(aControl);
        }

        public void RefreshViewFromData()
        {
            RefreshControlValueRecursivly(fRootVisualItem);
        }
    }
}
