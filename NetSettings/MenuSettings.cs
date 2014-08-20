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

    public struct CreationParams
    {
        public Control panel;
        public Control descriptionPanel;
        public ItemTree root;
        public Filter filter;
    }
    
    public class MenuSettings
    {
        public event ItemChangedDelegate ItemChanged = delegate { };
        private Dictionary<string, Type> fStringToType;
        Point panelPosition;
        const int LineSpacing = 25;
        const int TitleMaxWidth = 150;
        const int TitleSpacing = 30;
        const int ControlMaxWidth = 80;
        const int ControlSpacing = 20;
        const int LineHeight = 20;
        const int DefaultButtonWidth = 50;
        const int HorizontalMArgin = 20;
        int Nesting = 0;
        int currentRow;
        Control fDescriptionPanel;
        TextBox fDescriptionTextBox;
        CreationParams fParams;

        PreviewForm previewForm;
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
            fDescriptionPanel = fParams.descriptionPanel;
            TextBox t = new TextBox();
            t.Multiline = true;
            t.Dock = DockStyle.Fill;
            t.ReadOnly = true;
            t.BorderStyle = BorderStyle.FixedSingle;
            t.Font = new Font("Lucida fax",10);
            fDescriptionTextBox = t;
            fDescriptionPanel.Controls.Add(t);
            previewForm = new PreviewForm();
            previewForm.PreviewTimer.Tick += PreviewTimer_Tick;
            RefreshTree();
        }

        public void SetFilter(Filter aFilter)
        {
            fParams.filter = aFilter;
            RefreshTree();
        }

        private void RefreshTree()
        {
            panelPosition = new Point();
            currentRow = 0;
            CleanControls(fParams);
            ApplyFilterRecursively(fParams);
            AddControlRecursivly(fParams);
        }

        private void CleanControls(CreationParams fParams)
        {
            fParams.panel.Controls.Clear();
        }

        private bool ApplyFilterRecursively(CreationParams aParams)
        {
            ItemTree item = aParams.root;
            
            if (aParams.filter == null || String.IsNullOrEmpty(aParams.filter.IncludeName) || String.IsNullOrWhiteSpace(aParams.filter.IncludeName))
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
                    item.IsVisible = item.displayname.ToLower().Contains(aParams.filter.IncludeName.ToLower());
                }
            }

            if (item.subitems != null)
            {
                bool isVisible = false;
                foreach (ItemTree subItem in item.subitems)
                {
                    aParams.root = subItem;
                    isVisible |= ApplyFilterRecursively(aParams);
                }
                //if at least one of the childs is visible then the parent is visible as well.
                item.IsVisible = isVisible;

            }
            return item.IsVisible;

        }

        public void RaiseEvent(ItemTree aTreeItem)
        {
            ItemChanged(aTreeItem);
        }

        private void AddControlRecursivly(CreationParams  aParams)
        {
            ItemTree root = aParams.root;
            if (!root.IsVisible)
                return;
            Control control = aParams.panel;
            Type type;
            if (fStringToType.TryGetValue(root.type, out type))
                AddControl(control, root, type);

            //Add children
            if (root.subitems != null)
                foreach (ItemTree item in root.subitems)
                {
                    aParams.root = item;
                    AddControlRecursivly(aParams);
                }
        }

        private void AddControl(Control parent, ItemTree root, Type type)
        {
            bool isMenu = root.type == "menu";
            //Create parent container
            Control panel = new Control();
            parent.Controls.Add(panel);
            panel.Width = TitleMaxWidth + TitleSpacing + ControlMaxWidth + ControlSpacing + DefaultButtonWidth;
            panel.Height = LineSpacing ;
            panel.BackColor = isMenu ? Color.Yellow : currentRow % 2 == 0 ? Color.White : Color.LightGray;
            panel.MouseEnter +=l_MouseEnter;
            panelPosition.X = HorizontalMArgin * Nesting;
            panelPosition.Y += LineSpacing;
            panel.Location = panelPosition;
            panel.Tag = root;
            Point controlPosition = new Point(0, (LineSpacing - LineHeight) / 2 );
            
            //Add label describing the entry
            
            Label label = new Label();

            label.Width = TitleMaxWidth;
            label.Height = LineHeight;
            label.Font = new Font("consolas", 10);
            label.Text = root.displayname;
            label.Tag = root;
            label.Location = controlPosition;
            //label.MouseEnter += l_MouseEnter;
            panel.Controls.Add(label);
            controlPosition.X = TitleMaxWidth + TitleSpacing;
            
            //Add the  control itself
            Control control = Activator.CreateInstance(type) as Control;
            panel.Controls.Add(control);
            control.Location = controlPosition;
            control.Tag = root;
            //control.MouseEnter += l_MouseEnter;
            control.Height = LineHeight;
            control.Width = ControlMaxWidth;
            
            controlPosition.X += ControlMaxWidth + ControlSpacing;
            
            //Add reference from the menu item to the control holding the values.
            root.control = control;

        
            //Add a default button 
            if (!isMenu)
            {
                Button button = new Button();
                button.Width = DefaultButtonWidth;
                button.Height = LineHeight;
                panel.Controls.Add(button);
                button.Text = "Default";
                button.Location = controlPosition;
                button.Click += button_Click;
                button.Tag = root;
            }

            ProceeControl(label, control, root);

            currentRow++;

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
                if (item.description != null)
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
            previewForm.ImageName = (sender as TextBox).Text;
        }

        void MenuSettings_MouseLeave(object sender, EventArgs e)
        {
            if (Cursor.Position != fLastCursorPosition &&  previewForm != null)
                previewForm.Hide();
                
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
