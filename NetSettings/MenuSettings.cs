using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetSettings
{
    public delegate void ItemChangedDelegate(ItemTree aItemTree);
    public class MenuSettings
    {
        const string labelFont = "calibri";
        public event ItemChangedDelegate ItemChanged = delegate { };
        private Dictionary<string, Type> fStringToType;
        Point panelPosition;
        
        int Nesting = 0;
        int currentRow;
        ControlContainer fDescriptionPanel;
        TextBox fDescriptionTextBox;
        CreationParams fParams;

        VisualItem rootVisualItem;

        PreviewForm fPreviewForm;
        Point fLastCursorPosition;

        Font labelNormal;
        Font labelBold;

        

        public MenuSettings()
        {
            fStringToType = new Dictionary<string, Type>();
            fStringToType.Add("text", typeof(TextBox));
            fStringToType.Add("bool", typeof(CheckBox));
            fStringToType.Add("menu", typeof(Label));
            fStringToType.Add("combo", typeof(ComboBoxDoubleClick));
            fStringToType.Add("image", typeof(TextBox));
            fStringToType.Add("number", typeof(TextBox));
            fStringToType.Add("color", typeof(ColorControl));
            labelNormal = new Font(labelFont, 10, FontStyle.Regular);
            labelBold = new Font(labelFont, 10, FontStyle.Bold);
        }

        

        public void Create(CreationParams aParams)
        {
            fParams = aParams;
            CreateVisualItemTree();
            fDescriptionPanel = fParams.descriptionContainer;

            if (fDescriptionPanel != null)
            {
                fDescriptionPanel.Reset();
                fDescriptionPanel.StartUpdate();
                TextBox t = new TextBox();
                t.Multiline = true;
                t.Dock = DockStyle.Fill;
                t.ReadOnly = true;
                t.BorderStyle = BorderStyle.FixedSingle;
                t.Font = new Font("Lucida fax", 10);
                fDescriptionTextBox = t;
                fDescriptionPanel.Controls.Add(t);
                fDescriptionPanel.EndUpdate();
            }

            if (fPreviewForm == null)
            {
                fPreviewForm = new PreviewForm();
                fPreviewForm.PreviewTimer.Tick += PreviewTimer_Tick;
            }
            RefreshTree();
        }

        private void CreateVisualItemTree()
        {
            rootVisualItem = new VisualItem();
            rootVisualItem.IsVisible = true;
            rootVisualItem.Item = fParams.root;
            CreateVisualItemTree(rootVisualItem);
        }

        private void CreateVisualItemTree(VisualItem rootVisualItem)
        {
            ItemTree item = rootVisualItem.Item;
            if (item.subitems != null)
            {
                rootVisualItem.subitems = new VisualItem[item.subitems.Count()];
                int i = 0;
                foreach (ItemTree subItem in item.subitems)
                {
                    VisualItem visualItem = new VisualItem();
                    rootVisualItem.subitems[i++] = visualItem;
                    visualItem.Item = subItem;
                    visualItem.IsVisible = true;
                    CreateVisualItemTree(visualItem);
                }
                    
            }
        }

        public void SetFilter(Filter aFilter)
        {
            fParams.filter = aFilter;
            RefreshTree();
        }

        private void RefreshTree()
        {
            fParams.container.StartUpdate();
            
            panelPosition = new Point();
            currentRow = 0;
            fParams.container.Reset();
            ApplyFilterRecursively(rootVisualItem);
            AddControlRecursivly(rootVisualItem);

            fParams.container.EndUpdate();
        }
        
        private bool ApplyFilterRecursively(VisualItem root)
        {
            VisualItem visualItem = root;
            ItemTree item = root.Item;
            
            if (fParams.filter == null || String.IsNullOrEmpty(fParams.filter.IncludeName) || String.IsNullOrWhiteSpace(fParams.filter.IncludeName))
            {
                visualItem.IsVisible = true;
            }
            else
            {
                if (item.type == "menu" || item.type == "root")
                {
                    visualItem.IsVisible = true;
                }
                else
                if (item.displayname != null)
                {
                    visualItem.IsVisible = item.displayname.ToLower().Contains(fParams.filter.IncludeName.ToLower());
                }
            }

            if (item.subitems != null)
            {
                bool isVisible = false;
                foreach (VisualItem subItem in visualItem.subitems)
                    isVisible |= ApplyFilterRecursively(subItem);
                
                //if at least one of the childs is visible then the parent is visible as well.
                visualItem.IsVisible = isVisible;

            }
            return visualItem.IsVisible;

        }

        public void RaiseEvent(VisualItem aVisualItem)
        {
            CheckLabelColor(aVisualItem);
            ItemChanged(aVisualItem.Item);

        }

        private void CheckLabelColor(VisualItem aVisualItem)
        {
            aVisualItem.controlsGroup.label.Font = GetLabelFont(aVisualItem.Item);
        }

        private void AddControlRecursivly(VisualItem aRoot)
        {
            VisualItem visualItem = aRoot;
            ItemTree item = aRoot.Item;
            if (!visualItem.IsVisible)
                return;
            Control control = fParams.container;
            Type type;
            if (fStringToType.TryGetValue(item.type, out type))
                AddControl(aRoot, type);

            //Add children
            if (visualItem.subitems != null)
                foreach (VisualItem subItem in visualItem.subitems)
                    AddControlRecursivly(subItem);
        }

        private void AddControl(VisualItem aVisualItem, Type aType)
        {
            ItemTree aItem = aVisualItem.Item;
            PlacementParams p = fParams.placement;
            bool isMenu = aItem.type == "menu";
            //Create parent container
            ControlsGroup group = new ControlsGroup();
            Control panel = group.parentContainer = new Control();
             
            fParams.container.Controls.Add(panel);
            
            panel.Width = p.TitleMaxWidth + p.TitleSpacing + p.ControlMaxWidth + p.ControlSpacing + p.DefaultButtonWidth;
            panel.Height = p.LineSpacing;
            panel.BackColor = isMenu ? Color.Orange : currentRow % 2 == 0 ? Color.White : Color.LightGray;
            panel.MouseEnter +=l_MouseEnter;
            panelPosition.X = p.HorizontalMArgin * Nesting;
            panel.Location = panelPosition;
            panelPosition.Y += p.LineSpacing;
            panel.Tag = aVisualItem;
            Point controlPosition = new Point(0, (p.LineSpacing - p.LineHeight) / 2);
            
            //Add label describing the entry

            Label label = group.label = new Label();

            label.Width = p.TitleMaxWidth;
            label.Height = p.LineHeight;
            label.Font = GetLabelFont(aItem);
            label.Text = aItem.displayname;
            label.Tag = aVisualItem;
            label.Location = controlPosition;
            
            panel.Controls.Add(label);
            controlPosition.X = p.TitleMaxWidth + p.TitleSpacing;
            
            //Add the  control itself
            Control control = group.control = Activator.CreateInstance(aType) as Control;
            panel.Controls.Add(control);
            control.Location = controlPosition;
            control.Tag = aVisualItem;
            control.Height = p.LineHeight;
            control.Width = p.ControlMaxWidth;

            controlPosition.X += p.ControlMaxWidth + p.ControlSpacing;
            
            //Add reference from the menu item to the control holding the values.
            aVisualItem.controlsGroup = group;

        
            //Add a default button 
            if (!isMenu)
            {
                Button button = group.defaultButton = new Button();
                button.Width = p.DefaultButtonWidth;
                button.Height = p.LineHeight;
                panel.Controls.Add(button);
                button.Text = "Default";
                button.Location = controlPosition;
                button.Click += button_Click;
                button.Tag = aVisualItem;
                currentRow++;
            }

            ProceeControl(label, control, aVisualItem);
        }

        private Font GetLabelFont(ItemTree aTreeItem)
        {
            return aTreeItem.defaultvalue != null && aTreeItem.value != null  && !aTreeItem.defaultvalue.Equals(aTreeItem.value)
                ? labelBold : labelNormal;
        }

        void button_Click(object sender, EventArgs e)
        {
            Control c = sender as Control;
            VisualItem item = GetItemFromControl(c);
            if (item != null)
            {
                if (item.Item.defaultvalue != null)
                {
                    item.Item.SetValue( item.Item.defaultvalue);
                    RefreshControlValue(item);
                    RaiseEvent(item);
                }
            }
        }

        void l_MouseEnter(object sender, EventArgs e)
        {
            ItemTree item = (sender as Control).Tag as ItemTree;
            if (item != null)
            {
                if (item.description != null && fDescriptionTextBox != null)
                    fDescriptionTextBox.Text = item.description;
            }
        }

        private void RefreshControlValueRecursivly(VisualItem aRoot)
        {
            RefreshControlValue(aRoot);
            if (aRoot.subitems != null)
                foreach (VisualItem subItem in aRoot.subitems)
                    RefreshControlValueRecursivly(subItem);

        }

        private void RefreshControlValue(VisualItem aVisualItem)
        {
            ItemTree item = aVisualItem.Item;
            if (aVisualItem.controlsGroup != null)
            {
                Control aControl = aVisualItem.controlsGroup.control;
                switch (item.type)
                {
                    case "bool":
                        bool val = false;
                        if (item.currentValue != null)
                            val = (bool)item.currentValue;
                        (aControl as CheckBox).Checked = val;
                        break;
                    case "text":
                        (aControl as TextBox).Text = (string)item.currentValue;
                        break;
                    case "number":
                        (aControl as TextBox).Text = ((double)item.currentValue).ToString();
                        break;
                    case "combo":
                        (aControl as ComboBox).SelectedItem = item.currentValue;
                        break;
                    case "image":
                        (aControl as TextBox).Text = item.currentValue as string;
                        break;
                    case "color":
                        (aControl as Control).BackColor = (Color)item.currentValue;
                        break;
                }
            }
        }

        private void ProceeControl(Control label, Control actualControl, VisualItem aVisualItem)
        {
            PrepareControl(label, actualControl, aVisualItem.Item);
            RefreshControlValue(aVisualItem);
            ProcessEvents(label, actualControl, aVisualItem.Item);
        }

        private static void PrepareControl(Control label, Control actualControl, ItemTree aItem)
        {
            switch (aItem.type)
            {
                case "combo":
                    string[] values = aItem.values.Split(';');
                    foreach (string v in values)
                        (actualControl as ComboBox).Items.Add(v);
                    break;
                case "menu":
                    //TODO: move this two line of code this is no event
                    (label as Label).Font = new Font(labelFont, 12, FontStyle.Bold);
                    (label as Label).ForeColor = Color.Blue;
                    break;
            }
        }

        private void ProcessEvents(Control l, Control t, ItemTree aItem)
        {
            switch (aItem.type)
            {
                case "bool":
                    (t as CheckBox).CheckStateChanged += MenuSettings_CheckStateChanged;
                    break;
                case "text":
                    (t as TextBox).TextChanged += MenuSettings_TextChanged;
                    break;
                case "number":
                    (t as TextBox).TextChanged += MenuSettings_NumberChanged;
                    break;
                case "combo":
                    (t as ComboBox).SelectedIndexChanged += c_SelectedIndexChanged;
                    (t as ComboBox).MouseDoubleClick += Combo_MouseDoubleClick;
                    
                    break;
                case "image":
                    (t as TextBox).TextChanged += MenuSettings_TextChanged;
                    (t as TextBox).MouseDoubleClick += MenuSettings_MouseDoubleClick;
                    (t as TextBox).MouseLeave += MenuSettings_MouseLeave;
                    (t as TextBox).MouseHover += MenuSettings_MouseHover;
                    break;
                case "color":
                    (t as Control).Click += MenuSettings_Click;
                    break;
            }

        }

       

        private void Combo_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            VisualItem item = GetItemFromControl(comboBox);
            if (item != null)
            {
                comboBox.SelectedIndex = (comboBox.SelectedIndex + 1) % comboBox.Items.Count;
            }
            item.Item.SetValue(comboBox.SelectedItem);
            RaiseEvent(item);
        }

        void MenuSettings_Click(object sender, EventArgs e)
        {
            Control p = sender as Control;
            ColorDialog dialog;
            VisualItem item = GetItemFromControl(p);
            if ((dialog = new ColorDialog(){FullOpen = true,Color = (Color)item.Item.currentValue }).ShowDialog() == DialogResult.OK)
            {
                if (item != null)
                {
                    item.Item.SetValue(p.BackColor = dialog.Color);
                    RaiseEvent(item);
                }
            }
        }

        private void MenuSettings_NumberChanged(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;
            VisualItem item = GetItemFromControl(textbox);
            double num;
            if (double.TryParse(textbox.Text, out num))
            {
                item.Item.SetValue(num);
                RaiseEvent(item);
            }
        }

        void MenuSettings_MouseHover(object sender, EventArgs e)
        {
            fPreviewForm.ImageName = (sender as TextBox).Text;
        }

        void MenuSettings_MouseLeave(object sender, EventArgs e)
        {
            if (Cursor.Position != fLastCursorPosition &&  fPreviewForm != null)
                fPreviewForm.Hide();
                
        }

        void PreviewTimer_Tick(object sender, EventArgs e)
        {
            fLastCursorPosition = Cursor.Position;
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
                string directory = System.IO.Path.GetDirectoryName(strResult);
            }
            return strResult;

        }

        void MenuSettings_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string imageName = ChooseFile(true, "", "");
            TextBox t = sender as TextBox;
            VisualItem item = GetItemFromControl(t);
            item.Item.SetValue(t.Text = imageName);
        }
        void c_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            VisualItem item = GetItemFromControl(comboBox);
            if (item != null)
                item.Item.SetValue(comboBox.SelectedItem);
            RaiseEvent(item);
        }

        void MenuSettings_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;
            VisualItem item = GetItemFromControl(textbox);
            item.Item.SetValue(textbox.Text);
            RaiseEvent(item);
        }

        void MenuSettings_CheckStateChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            VisualItem item = checkBox.Tag as VisualItem;
            item.Item.SetValue(checkBox.Checked);
            RaiseEvent(item);
        }

        VisualItem GetItemFromControl(Control aControl)
        {
            return aControl.Tag as VisualItem;
        }

        public void RefreshViewFromData()
        {
            RefreshControlValueRecursivly(rootVisualItem);
        }
    }

}
