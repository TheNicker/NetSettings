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

    public class CreationParams
    {
        public ControlContainer container;
        public ControlContainer descriptionContainer;
        public ItemTree root;
        public Filter filter;
        public PlacementParams placement = new PlacementParams();
    }

    public class PlacementParams
    {
        public int LineSpacing = 25;
        public int TitleMaxWidth = 150;
        public int TitleSpacing = 30;
        public int ControlMaxWidth = 80;
        public int ControlSpacing = 20;
        public int LineHeight = 20;
        public int DefaultButtonWidth = 50;
        public int HorizontalMArgin = 20;
    }
    
    public class MenuSettings
    {
        public event ItemChangedDelegate ItemChanged = delegate { };
        private Dictionary<string, Type> fStringToType;
        Point panelPosition;
        
        int Nesting = 0;
        int currentRow;
        ControlContainer fDescriptionPanel;
        TextBox fDescriptionTextBox;
        CreationParams fParams;

        PreviewForm fPreviewForm;
        Point fLastCursorPosition;

        public MenuSettings()
        {
            fStringToType = new Dictionary<string, Type>();
            fStringToType.Add("text", typeof(TextBox));
            fStringToType.Add("bool", typeof(CheckBox));
            fStringToType.Add("menu", typeof(Label));
            fStringToType.Add("combo", typeof(ComboBox));
            fStringToType.Add("image", typeof(TextBox));
            fStringToType.Add("number", typeof(TextBox));
            fStringToType.Add("color", typeof(Control));
        }

        public void Create(CreationParams aParams)
        {
            fParams = aParams;
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

            if (fPreviewForm != null)
            {
                fPreviewForm = new PreviewForm();
                fPreviewForm.PreviewTimer.Tick += PreviewTimer_Tick;
            }
            RefreshTree();
        }

        public void SetFilter(Filter aFilter)
        {
            fParams.filter = aFilter;
            fParams.container.StartUpdate();
            RefreshTree();
            fParams.container.EndUpdate();
        }

        private void RefreshTree()
        {
            panelPosition = new Point();
            currentRow = 0;
            fParams.container.Reset();
            ApplyFilterRecursively(fParams.root);
            AddControlRecursivly(fParams.root);
        }
        
        private bool ApplyFilterRecursively(ItemTree root)
        {
            ItemTree item = root;
            
            if (fParams.filter == null || String.IsNullOrEmpty(fParams.filter.IncludeName) || String.IsNullOrWhiteSpace(fParams.filter.IncludeName))
            {
                item.IsVisible = true;
            }
            else
            {
                if (item.type == "menu" || item.type == "root")
                {
                    item.IsVisible = true;
                }
                else
                if (item.displayname != null)
                {
                    item.IsVisible = item.displayname.ToLower().Contains(fParams.filter.IncludeName.ToLower());
                }
            }

            if (item.subitems != null)
            {
                bool isVisible = false;
                foreach (ItemTree subItem in item.subitems)
                    isVisible |= ApplyFilterRecursively(subItem);
                
                //if at least one of the childs is visible then the parent is visible as well.
                item.IsVisible = isVisible;

            }
            return item.IsVisible;

        }

        public void RaiseEvent(ItemTree aTreeItem)
        {
            
            ItemChanged(aTreeItem);
        }

        private void AddControlRecursivly(ItemTree aRoot)
        {
            ItemTree item = aRoot;
            if (!item.IsVisible)
                return;
            Control control = fParams.container;
            Type type;
            if (fStringToType.TryGetValue(item.type, out type))
                AddControl(item,type);

            //Add children
            if (item.subitems != null)
                foreach (ItemTree subItem in item.subitems)
                    AddControlRecursivly(subItem);
        }

        private void AddControl(ItemTree root, Type type)
        {
            PlacementParams p = fParams.placement;
            bool isMenu = root.type == "menu";
            //Create parent container
            Control panel = new Control();
            fParams.container.Controls.Add(panel);
            
            panel.Width = p.TitleMaxWidth + p.TitleSpacing + p.ControlMaxWidth + p.ControlSpacing + p.DefaultButtonWidth;
            panel.Height = p.LineSpacing;
            panel.BackColor = isMenu ? Color.Yellow : currentRow % 2 == 0 ? Color.White : Color.LightGray;
            panel.MouseEnter +=l_MouseEnter;
            panelPosition.X = p.HorizontalMArgin * Nesting;
            panel.Location = panelPosition;
            panelPosition.Y += p.LineSpacing;
            panel.Tag = root;
            Point controlPosition = new Point(0, (p.LineSpacing - p.LineHeight) / 2);
            
            //Add label describing the entry
            
            Label label = new Label();

            label.Width = p.TitleMaxWidth;
            label.Height = p.LineHeight;
            label.Font = new Font("consolas", 10);
            label.Text = root.displayname;
            label.Tag = root;
            label.Location = controlPosition;
            //label.MouseEnter += l_MouseEnter;
            panel.Controls.Add(label);
            controlPosition.X = p.TitleMaxWidth + p.TitleSpacing;
            
            //Add the  control itself
            Control control = Activator.CreateInstance(type) as Control;
            panel.Controls.Add(control);
            control.Location = controlPosition;
            control.Tag = root;
            //control.MouseEnter += l_MouseEnter;
            control.Height = p.LineHeight;
            control.Width = p.ControlMaxWidth;

            controlPosition.X += p.ControlMaxWidth + p.ControlSpacing;
            
            //Add reference from the menu item to the control holding the values.
            root.control = control;

        
            //Add a default button 
            if (!isMenu)
            {
                Button button = new Button();
                button.Width = p.DefaultButtonWidth;
                button.Height = p.LineHeight;
                panel.Controls.Add(button);
                button.Text = "Default";
                button.Location = controlPosition;
                button.Click += button_Click;
                button.Tag = root;
                currentRow++;
            }

            ProceeControl(label, control, root);
        }

        void button_Click(object sender, EventArgs e)
        {
            Control c = sender as Control;
            ItemTree item = GetItemFromControl(c);
            if (item != null)
            {
                if (item.defaultvalue != null)
                {
                    item.value = item.defaultvalue;
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

        private void RefreshControlValue(ItemTree item)
        {
            Control aControl = item.control;
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
                        string[] values = item.values.Split(';');
                        foreach (string v in values)
                            (aControl as ComboBox).Items.Add(v);
                        (aControl as ComboBox).SelectedItem = item.currentValue;
                    break;
                case "image":
                    (aControl as TextBox).Text = item.currentValue as string;
                    break;
                case "color":
                    (aControl as Control).BackColor = (Color)item.currentValue;
                    break;
            }

            //if (item.va)


        }

        private void ProceeControl(Control label, Control actualControl, ItemTree root)
        {
            RefreshControlValue(root);
            ProcessEvents(label, actualControl, root);
        }

        private void ProcessEvents(Control l, Control t, ItemTree root)
        {
            switch (root.type)
            {
                case "menu":
                    (l as Label).Font = new Font("consolas", 12,FontStyle.Bold);
                    (l as Label).ForeColor = Color.Blue;
                    break;
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
                    break;
                case "image":
                    (t as TextBox).TextChanged += MenuSettings_TextChanged;
                    (t as TextBox).MouseDoubleClick += MenuSettings_MouseDoubleClick;
                    (t as TextBox).MouseLeave += MenuSettings_MouseLeave;
                    (t as TextBox).MouseHover += MenuSettings_MouseHover;
                    break;
                case "color":
                    //(t as Control).BorderStyle = BorderStyle.FixedSingle;
                    (t as Control).Click += MenuSettings_Click;
                    break;
            }

        }

        void MenuSettings_Click(object sender, EventArgs e)
        {
            Control p = sender as Control;
            ColorDialog dialog;
            ItemTree item = GetItemFromControl(p);
            if ((dialog = new ColorDialog(){FullOpen = true,Color = (Color)item.currentValue }).ShowDialog() == DialogResult.OK)
            {
                
                if (item != null)
                {
                    item.value = p.BackColor = dialog.Color;
                    RaiseEvent(item);
                }
            }
        }

        private void MenuSettings_NumberChanged(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;
            ItemTree item = GetItemFromControl(textbox);
            double num;
            if (double.TryParse(textbox.Text, out num))
                item.value = num;
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
            ItemTree item = GetItemFromControl(t);
            item.value = t.Text = imageName;
        }
        void c_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ItemTree item = GetItemFromControl(comboBox);
            if (item != null)
                item.value = comboBox.SelectedItem;
            RaiseEvent(item);
        }

        void MenuSettings_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;
            ItemTree item = GetItemFromControl(textbox);
            item.value = textbox.Text;
            RaiseEvent(item);
        }

        void MenuSettings_CheckStateChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            ItemTree item = checkBox.Tag as ItemTree;
            item.value = checkBox.Checked;
            RaiseEvent(item);
        }

        ItemTree GetItemFromControl(Control aControl)
        {
            return aControl.Tag as ItemTree;
        }
    }

}
