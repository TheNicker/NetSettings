// ReSharper disable PossibleNullReferenceException

using NetSettings.Data;
using System;
using System.Collections.Generic;
using NetSettings.Common.Classes;
using NetSettings.Common.Interfaces;
using BorderStyle = NetSettings.Common.Classes.BorderStyle;
using CheckBox = NetSettings.Common.Interfaces.ICheckBox;
using DialogResult = NetSettings.Common.Classes.DialogResult;
using DockStyle = NetSettings.Common.Classes.DockStyle;
using FlatStyle = NetSettings.Common.Classes.FlatStyle;

namespace NetSettings.View
{
    public class DataView
    {
        private const string labelFont = "calibri"; //TODO: Rename to familyName

        //private Dictionary<string, Type> fStringToType; //TODO: Delete this field

        //Controls arrangement
        private Point fCurrentPanelPosition;
        private readonly int fNesting = 0;
        private int fCurrentRow;

        private IControlContainer fDescriptionPanel; //TODO: Can we remove this from the class level and move it next to usage?
        private ITextBox fDescriptionTextBox;
        private DataViewParams fParams;

        private VisualItem fRootVisualItem;

        //private PreviewForm fPreviewForm;
        public IGuiProvider guiProvider { get; set; }
        private readonly Point fLastCursorPosition; //TODO: Can we delete this?

        private IFont fLabelNormal;
        private IFont fLabelBold;
        private IFont fMenuLabel;

        private readonly Dictionary<IGuiElement, VisualItem> dic = new Dictionary<IGuiElement, VisualItem>();

        public DataView()
        {
            //fStringToType = new Dictionary<string, Type>
            //{
            //    {"text", typeof(ITextBox)},
            //    {"bool", typeof(ICheckBox)},
            //    {"menu", typeof(ILabel)},
            //    {"combo", typeof(IComboBoxDoubleClick)},
            //    {"image", typeof(ITextBox)},
            //    {"number", typeof(ITextBox)},
            //    {"color", typeof(IColorControl)}
            //};

            //fPreviewForm = new PreviewForm();
        }

        public void Create(DataViewParams aParams)
        {
            guiProvider = aParams.guiProvider;
            DataView.VerifyParameters(aParams);
            fParams = aParams;

            fLabelNormal = (IFont)guiProvider.CreateGuiElement(GuiElementType.IFont, labelFont, 10f, FontAppearance.Regular);
            fLabelBold = (IFont)guiProvider.CreateGuiElement(GuiElementType.IFont, labelFont, 10f, FontAppearance.Bold);
            fMenuLabel = (IFont)guiProvider.CreateGuiElement(GuiElementType.IFont, labelFont, 12f, FontAppearance.Bold);

            fParams.dataProvider.AddView(this);
            CreateVisualItemTree();
            fDescriptionPanel = fParams.descriptionContainer;

            if (fDescriptionPanel != null)
            {
                //TODO: Move this to the bottom of the screen
                fDescriptionPanel.Reset();
                fDescriptionPanel.StartUpdate();

                var textBox = (ITextBox)guiProvider.CreateGuiElement(GuiElementType.Text);
                textBox.Multiline = true;
                textBox.Dock = DockStyle.Fill;
                textBox.ReadOnly = true;
                textBox.BorderStyle = BorderStyle.FixedSingle;
                textBox.Font = (IFont)guiProvider.CreateGuiElement(GuiElementType.IFont, "Lucida Fax", 10f);

                fDescriptionTextBox = textBox;
                fDescriptionPanel.AddControl(textBox);
                fDescriptionPanel.EndUpdate();
            }

            ReCreateTree();
        }

        private static void VerifyParameters(DataViewParams aParams)
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
                rootVisualItem.subItems = new List<VisualItem>();
                foreach (var subItem in item.subItems)
                {
                    var visualItem = new VisualItem
                    {
                        Item = subItem,
                        IsFiltered = true
                    };
                    rootVisualItem.subItems.Add(visualItem);
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
            {
                return;
            }

            if (Enum.TryParse<GuiElementType>(item.type, true, out var elementType))
            {
                AddControl(aRoot, elementType);
            }

            //Add children
            if (visualItem.subItems != null)
            {
                foreach (var subItem in visualItem.subItems)
                {
                    AddControlRecursivly(subItem);
                }
            }
        }

        private void AddControl(VisualItem aVisualItem, object aType)
        {
            ItemTree aItem = aVisualItem.Item;

            var isMenu = aItem.type == "menu";
            var isBool = aItem.type == "bool";
            //Create parent container
            var group = new ItemControlsGroup();
            var parent = group.parentContainer = guiProvider.CreateGuiElement(GuiElementType.GuiElement) as IControl;

            fParams.container.AddControl(parent);
            dic.Add(parent, aVisualItem);

            //Add label describing the entry
            var label = group.label = (ILabel)guiProvider.CreateGuiElement(GuiElementType.Label);
            label.Font = GetLabelFont(aVisualItem);
            label.Text = aItem.displayName;
            //label.SetStyle(ControlStyles.StandardDoubleClick, !isBool); //TODO: Where is this double click event being used?
            parent.AddVisualControl(label);
            dic.Add(label, aVisualItem);

            //Add the  control itself
            var control = group.control = guiProvider.CreateGuiElement(aType) as IControl;
            parent.AddVisualControl(control);
            dic.Add(control, aVisualItem);

            //Add reference from the menu item to the control holding the values.
            aVisualItem.controlsGroup = group;

            //Add a default button 
            if (!isMenu)
            {
                var button = group.defaultButton = guiProvider.CreateGuiElement(GuiElementType.Button) as IButton;
                button.Text = "Default";
                button.Click += (sender, e) => button_Click(button, e);
                button.FlatStyle = FlatStyle.Popup;
                button.BackColor = Color.LightGray;
                parent.AddVisualControl(button);
                dic.Add(button, aVisualItem);
                fCurrentRow++;
            }

            //TODO: WTF?????
            var l = new MouseEnterLeave(parent);
            //TODO: Do we need this 2 events? probably yes. Can we do this code nicer? Can we remove the MouseEnterLeave class?
            l.MouseEnter += l_MouseEnter;
            l.MouseLeave += panel_MouseLeave;
            ProcessControl(aVisualItem);
        }

        private void ProcessControl(VisualItem aVisualItem)
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
                    foreach (var value in values)
                    {
                        (actualControl as IComboBox).AddItem(value);
                    }

                    break;
                case "menu":
                    label.ForeColor = Color.LawnGreen;
                    break;
            }
        }

        private void ProcessEvents(VisualItem aVisualItem)
        {
            var control = aVisualItem.controlsGroup.control;
            var parentContainer = aVisualItem.controlsGroup.parentContainer;
            var label = aVisualItem.controlsGroup.label;
            switch (aVisualItem.Item.type)
            {
                //TODO: Remove all the casting in this switch
                case "menu":
                    label.DoubleClick += (sender, e) => { Menu_DoubleClick(control, e); };
                    break;
                case "bool":
                    control.MouseClick += (sender, e) => { CheckBox_MouseClick(control, e); };
                    parentContainer.MouseClick += (sender, e) => { CheckBox_MouseClick(control, e); };
                    label.MouseClick += (sender, e) => { CheckBox_MouseClick(control, e); };
                    break;
                case "text":
                    control.TextChanged += MenuSettings_TextChanged;
                    control.Leave += (sender, e) => { DataView_Leave(control, e); };
                    break;
                case "number":
                    control.TextChanged += (sender, e) => { MenuSettings_NumberChanged(control, e); }; 
                    control.Leave += (sender, e) => { Number_Leave(control, e); };
                    break;
                case "combo":
                    (control as IComboBox).SelectedIndexChanged += (sender, e) => { c_SelectedIndexChanged(control, e); };
                    control.MouseDoubleClick += Combo_MouseDoubleClick;
                    break;
                case "image":
                    control.TextChanged += MenuSettings_TextChanged;
                    control.MouseDoubleClick += MenuSettings_MouseDoubleClick;
                    control.MouseLeave += MenuSettings_MouseLeave;
                    control.MouseEnter += (sender, e) => MenuSettings_MouseEnter(control, e);
                    break;
                case "color":
                    control.KeyDown += ColorControl_KeyDown;
                    control.TextChanged += (sender, e) => { ColorControl_TextChanged(control, e); };
                    control.DoubleClick += (sender, e) => { MenuSettings_Click(control, e); };
                    parentContainer.Click += (sender, e) => { MenuSettings_Click(control, e); };
                    label.Click += (sender, e) => { MenuSettings_Click(control, e); };
                    break;
            }
        }

        private void Menu_DoubleClick(object sender, EventArgs e)
        {
            var item = GetItemFromControl(sender as IControl);
            if (item != null) //TODO: Can we remove this condition?
                item.Expanded = !item.Expanded;
            ReArrange();
        }
        #endregion

        #region ReArrange Tree
        private void ReArrangeRecursively(VisualItem aRoot)
        {
            var visualItem = aRoot;
            ReArrange(visualItem);
            if (visualItem.subItems != null && (visualItem.Expanded || (fParams.filter != null && !fParams.filter.IsEmpty())))
            {
                foreach (var subItem in visualItem.subItems)
                {
                    ReArrangeRecursively(subItem);
                }
            }
        }

        public void ReArrange()
        {
            fCurrentPanelPosition = new Point();
            fCurrentRow = 0;
            fParams.container.ResetPosition();
            fParams.container.StartUpdate();

            HideAllControls();
            ApplyFilterRecursively(fRootVisualItem);
            ReArrangeRecursively(fRootVisualItem);

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

            var aItem = aVisualItem.Item;
            var dvp = fParams.placement;
            var isMenu = aItem.type == "menu";
            //Create parent container
            var group = aVisualItem.controlsGroup;
            var parent = aVisualItem.controlsGroup.parentContainer;

            parent.Width = dvp.TitleMaxWidth + dvp.TitleSpacing + dvp.ControlMaxWidth + dvp.ControlSpacing + dvp.DefaultButtonWidth;
            parent.Height = dvp.LineSpacing;
            aVisualItem.PanelBackgroundColor = isMenu ? Color.DarkBlue : fCurrentRow % 2 == 0 ? Color.White : Color.LightGray;
            parent.BackColor = aVisualItem.PanelBackgroundColor;

            fCurrentPanelPosition.X = dvp.HorizontalMargin * fNesting;
            fCurrentPanelPosition.Y += dvp.LineSpacing;
            parent.Location = fCurrentPanelPosition;
            var label = group.label;
            label.Width = dvp.TitleMaxWidth;
            label.Height = dvp.LineHeight;
            var controlPosition = new Point(0, (dvp.LineSpacing - dvp.LineHeight) / 2);
            label.Location = controlPosition;
            controlPosition.X = dvp.TitleMaxWidth + dvp.TitleSpacing;
            var control = group.control;
            control.Location = controlPosition;
            control.Height = dvp.LineHeight;
            control.Width = dvp.ControlMaxWidth;

            //Add a default button 
            if (!isMenu)
            {
                controlPosition.X += dvp.ControlMaxWidth + dvp.ControlSpacing;
                var button = group.defaultButton;
                button.Width = dvp.DefaultButtonWidth;
                button.Height = dvp.LineHeight;
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
            {
                return fMenuLabel;
            }

            var item = aVisualItem.Item;
            var val = GetValue(aVisualItem.Item);
            return item.defaultValue != null && val != null && !item.defaultValue.Equals(val) ? fLabelBold : fLabelNormal;
        }

        private void button_Click(object sender, EventArgs e)
        {
            var c = sender as IControl;
            var item = GetItemFromControl(c);
            if (item != null)
            {
                SetValue(item, item.Item.defaultValue);
            }
        }

        private void l_MouseEnter(object sender, EventArgs e)
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

        private void panel_MouseLeave(object sender, EventArgs e)
        {
            dic.TryGetValue(sender as IControl, out var item);
            if (item != null)
            {
                if (item.Item.type != "menu")
                {
                    var container = item.controlsGroup.parentContainer;
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
                var aControl = aVisualItem.controlsGroup.control;
                object val = GetValue(item);
                switch (item.type)
                {
                    //TODO: Remove all the casting of the controls unless needed
                    case "bool":
                        var _val = false;
                        if (val != null)
                            _val = (bool)val;
                        (aControl as ICheckBox).Checked = _val;
                        break;
                    case "text":
                    case "image":
                        aControl.Text = (val != null ? val as string : string.Empty);
                        break;
                    case "number":
                        aControl.Text = (val != null ? ToDoubleString(((double)val)) : string.Empty);
                        break;
                    case "combo":
                        (aControl as IComboBox).SelectedItem = val;
                        break;
                    case "color":
                        aControl.BackColor = (Color)val;
                        break;
                }
                CheckLabelColor(aVisualItem);

            }
        }

        private void ColorControl_KeyDown(object sender, EventArgs e)
        {
            var colorControl = sender as IColorControl;
            RefreshControlValue(dic[colorControl]);
        }

        private void ColorControl_TextChanged(object sender, EventArgs e)
        {
            var colorControl = sender as IColorControl;
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
            Leave(sender, e);
        }

        void DataView_Leave(object sender, EventArgs e)
        {
            Leave(sender, e);
        }

        private void Leave(object sender, EventArgs e)
        {
            var textBox = sender as ITextBox;
            var visualItem = GetItemFromControl(textBox);
            var value = GetValueFromControl(visualItem);
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
            var comboBox = sender as IComboBox;
            VisualItem item = GetItemFromControl(comboBox);
            SetValue(item, comboBox.SelectedItem);
        }

        void MenuSettings_Click(object sender, EventArgs e)
        {
            var p = sender as IControl;
            var item = GetItemFromControl(p);
            var dialog = guiProvider.CreateGuiElement(GuiElementType.ColorDialog) as IColorDialog;
            dialog.FullOpen = true;
            dialog.Color = (Color)GetValue(item.Item.FullName);

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
            var comboBox = sender as IComboBox;
            var item = GetItemFromControl(comboBox);
            SetValue(item, comboBox.SelectedItem);
        }

        void MenuSettings_TextChanged(object sender, EventArgs e)
        {
            var textBox = sender as ITextBox;
            var item = GetItemFromControl(textBox);
            SetValue(item, textBox.Text, ItemChangedMode.OnTheFly);

        }

        private VisualItem GetItemFromControl(object aControl)
        {
            return GetItemFromControl(aControl as IControl);
        }

        private VisualItem GetItemFromControl(IGuiElement aControl)
        {
            return dic.GetValueOrDefault(aControl);
        }

        public void RefreshViewFromData()
        {
            RefreshControlValueRecursivly(fRootVisualItem);
        }
    }
}
