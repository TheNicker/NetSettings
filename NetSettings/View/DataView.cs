using NetSettings.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetSettings
{
    public class DataView
    {
        const string labelFont = "calibri";
        
        private Dictionary<string, Type> fStringToType;
        Point panelPosition;
        
        int Nesting = 0;
        int currentRow;
        ControlContainer fDescriptionPanel;
        TextBox fDescriptionTextBox;
        DataViewParams fParams;

        VisualItem rootVisualItem;

        PreviewForm fPreviewForm;
        Point fLastCursorPosition;

        Font labelNormal;
        Font labelBold;

        public DataView()
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

        

        public void Create(DataViewParams aParams)
        {
            fParams = aParams;
            fParams.dataProvider.AddView(this);
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
            rootVisualItem.Item = fParams.dataProvider.fRootTemplate;
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
            //RefreshTree();
        }

        public void RefreshTree()
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

        //public void RaiseEvent(VisualItem aVisualItem)
        //{
        //    //ItemChanged(aVisualItem.Item);

        //}

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
            DataViewPlacement p = fParams.placement;
            bool isMenu = aItem.type == "menu";
            //Create parent container
            ItemControlsGroup group = new ItemControlsGroup();
            Control panel = group.parentContainer = new Control();
            
            fParams.container.Controls.Add(panel);
            
            
            

            panel.Width = p.TitleMaxWidth + p.TitleSpacing + p.ControlMaxWidth + p.ControlSpacing + p.DefaultButtonWidth;
            panel.Height = p.LineSpacing;
            aVisualItem.PanelBackgroundColor = isMenu ? Color.Orange : currentRow % 2 == 0 ? Color.White : Color.LightGray;
            panel.BackColor = aVisualItem.PanelBackgroundColor;
            
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

            MouseEnterLeave l = new MouseEnterLeave(panel);
            l.MouseEnter += l_MouseEnter;
            l.MouseLeave += panel_MouseLeave;
            ProceeControl(aVisualItem);
        }

        

       

        private Font GetLabelFont(ItemTree aTreeItem)
        {
            object val = GetValue(aTreeItem);
            return aTreeItem.defaultvalue != null && val != null  && !aTreeItem.defaultvalue.Equals(val)
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
                    SetValue(item, item.Item.defaultvalue);
                    RefreshControlValue(item);
                }
            }
        }

        void l_MouseEnter(object sender, EventArgs e)
        {
            VisualItem item = (sender as Control).Tag as VisualItem;
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
            
            VisualItem item = (sender as Control).Tag as VisualItem;
            if (item != null)
            {
                if (item.Item.type != "menu")
                {
                    Control container = item.controlsGroup.parentContainer;
                    container.BackColor = item.PanelBackgroundColor;

                }
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
                object val = GetValue(item);
                switch (item.type)
                {
                    case "bool":
                        bool _val = false;
                        if (val != null)
                            _val = (bool)val;
                        (aControl as CheckBox).Checked = _val;
                        break;
                    case "text":
                        (aControl as TextBox).Text = (string)val;
                        break;
                    case "number":
                        (aControl as TextBox).Text = ((double)val).ToString();
                        break;
                    case "combo":
                        (aControl as ComboBox).SelectedItem = val;
                        break;
                    case "image":
                        (aControl as TextBox).Text = val as string;
                        break;
                    case "color":
                        (aControl as Control).BackColor = (Color)val;
                        break;
                }
            }
        }

        private void ProceeControl(VisualItem aVisualItem)
        {
            PrepareControl(aVisualItem);
            RefreshControlValue(aVisualItem);
            ProcessEvents(aVisualItem);
        }

        private static void PrepareControl(VisualItem aVIsualItem)
        {

            Control label = aVIsualItem.controlsGroup.label;
            Control actualControl = aVIsualItem.controlsGroup.control;
            ItemTree aItem = aVIsualItem.Item;
            switch (aItem.type)
            {
                case "combo":
                    string[] values = aItem.values.Split(';');
                    foreach (string v in values)
                        (actualControl as ComboBox).Items.Add(v);
                    break;
                case "menu":
                    (label as Label).Font = new Font(labelFont, 12, FontStyle.Bold);
                    (label as Label).ForeColor = Color.Blue;
                    break;
            }
        }

        private void ProcessEvents(VisualItem aVisualItem)
        {

            Control t = aVisualItem.controlsGroup.control;
            Control p = aVisualItem.controlsGroup.parentContainer;
            ItemTree aItem = aVisualItem.Item;
            switch (aItem.type)
            {
                case "bool":
                    (t as CheckBox).CheckStateChanged += MenuSettings_CheckStateChanged;
                    break;
                case "text":
                    (t as TextBox).TextChanged += MenuSettings_TextChanged;
                    (t as TextBox).KeyDown += DataView_KeyDown;
                    (t as TextBox).Leave += DataView_Leave;
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

        void DataView_Leave(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            VisualItem visualItem = GetItemFromControl(textBox);
            if (visualItem != null)
                SetValue(visualItem,textBox.Text,ItemChangedMode.UserConfirmed);
        }

        void DataView_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void SetValue(VisualItem aVisualItem, object aVal,ItemChangedMode aMode = ItemChangedMode.UserConfirmed)
        {
            fParams.dataProvider.SetValue(
                new ItemChangedArgs()
                {
                    sender = this,
                    ChangedMode = aMode,
                    Key = aVisualItem.Item.FullName,
                    Val = aVal
                });

                
            CheckLabelColor(aVisualItem);
        }

        private object GetValue(string name)
        {
            return fParams.dataProvider.GetValue(name);
        }

        private object GetValue(ItemTree item)
        {
            return GetValue(item.FullName);
        }

        private void Combo_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            VisualItem item = GetItemFromControl(comboBox);
            if (item != null)
            {
                comboBox.SelectedIndex = (comboBox.SelectedIndex + 1) % comboBox.Items.Count;
            }
            SetValue(item,comboBox.SelectedItem);
        }

        void MenuSettings_Click(object sender, EventArgs e)
        {
            Control p = sender as Control;
            ColorDialog dialog;
            VisualItem item = GetItemFromControl(p);
            if ((dialog = new ColorDialog(){FullOpen = true,Color = (Color)GetValue(item.Item.FullName)}).ShowDialog() == DialogResult.OK)
            {
                if (item != null)
                {
                    SetValue(item, p.BackColor = dialog.Color);
                    
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
                SetValue(item, num,ItemChangedMode.OnTheFly);
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
            if (imageName != null)
            {
                TextBox t = sender as TextBox;
                VisualItem item = GetItemFromControl(t);
                SetValue(item, t.Text = imageName);
            }
        }
        void c_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            VisualItem item = GetItemFromControl(comboBox);
            if (item != null)
                SetValue(item,comboBox.SelectedItem);
            
        }

        void MenuSettings_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;
            VisualItem item = GetItemFromControl(textbox);
            SetValue(item,textbox.Text,ItemChangedMode.OnTheFly);
            
        }

        void MenuSettings_CheckStateChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            VisualItem item = checkBox.Tag as VisualItem;
            SetValue(item, checkBox.Checked);
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
