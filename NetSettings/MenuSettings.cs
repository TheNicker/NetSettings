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
        Graphics gfx;
        Control fDescriptionPanel;
        TextBox fDescriptionTextBox;

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
            fStringToType.Add("color", typeof(Panel));
        }

        public void Create(CreationParams aParams)
        {
            panelPosition = new Point();
            currentRow = 0;
            gfx = Graphics.FromHwnd(aParams.panel.Handle);
            fDescriptionPanel = aParams.descriptionPanel;
            TextBox t = new TextBox();
            t.Multiline = true;
            t.Dock = DockStyle.Fill;
            t.ReadOnly = true;
            t.BorderStyle = BorderStyle.FixedSingle;
            t.Font = new Font("Lucida fax",10);
            fDescriptionTextBox = t;
            fDescriptionPanel.Controls.Add(t);
            //root.Tag = menu;
            AddControlRecursivly(aParams);

            previewForm = new PreviewForm();
            previewForm.PreviewTimer.Tick += PreviewTimer_Tick;
        }

        
        public void RaiseEvent(ItemTree aTreeItem)
        {
            ItemChanged(aTreeItem);
        }

        private void AddControlRecursivly(CreationParams  aParams)
        {
            ItemTree root = aParams.root;
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
            Panel panel = new Panel();
            panel.Width = TitleMaxWidth + TitleSpacing + ControlMaxWidth + ControlSpacing + DefaultButtonWidth;
            panel.Height = LineSpacing ;
            parent.Controls.Add(panel);
            panel.BackColor = isMenu ? Color.Yellow : currentRow % 2 == 0 ? Color.White : Color.LightGray;
            
            panelPosition.X = HorizontalMArgin * Nesting;
            panelPosition.Y += LineSpacing;
            panel.Location = panelPosition;
            Point controlPosition = new Point(0, (LineSpacing - LineHeight) / 2 );
            
            //Add label describing the entry
            
            Label label = new Label();

            label.Width = TitleMaxWidth;
            label.Height = LineHeight;
            label.Text = root.displayname;
            label.Tag = root;
            label.Location = controlPosition;
            label.MouseEnter += l_MouseEnter;
            panel.Controls.Add(label);
            controlPosition.X = TitleMaxWidth + TitleSpacing;
            
            //Add the  control itself
            Control control = Activator.CreateInstance(type) as Control;
            panel.Controls.Add(control);
            control.Location = controlPosition;
            control.Tag = root;
            control.MouseEnter += l_MouseEnter;
            control.Height = LineHeight;
            control.Width = ControlMaxWidth;
            
            controlPosition.X += ControlMaxWidth + ControlSpacing;
            
            ProceeControl(label, control, root);
            

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
            }
            currentRow++;

        }

        void button_Click(object sender, EventArgs e)
        {
            Control c = sender as Control;
            ItemTree item = GetItemFromControl(c);
            if (item != null)
            {
                item.value = item.defaultvalue;
            }
            //Default button
            
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

        private void ProceeControl(Control l, Control t, ItemTree root)
        {
            if (root.type == "menu")
            {
                (l as Label).Font = new Font("Arial", 10);
                (l as Label).ForeColor = Color.Blue;
            }

            if (root.type == "bool")
            {

                bool val = false;
                if (root.currentValue != null)
                    val = (bool)root.currentValue;

                (t as CheckBox).Checked = val;

                (t as CheckBox).CheckStateChanged += MenuSettings_CheckStateChanged;
                
            }

            if (root.type == "text")
            {
                (t as TextBox).TextChanged += MenuSettings_TextChanged;
                (t as TextBox).Text = (string)root.currentValue;
            }

            if (root.type == "number")
            {
                (t as TextBox).TextChanged += MenuSettings_NumberChanged;
                (t as TextBox).Text = ((double)root.currentValue).ToString();
                
            }

            if (root.type == "combo")
            {
                ComboBox c = (t as ComboBox);
                if (c != null)
                {
                    string[] values = root.values.Split(';');
                    foreach (string v in values)
                        c.Items.Add(v);
                    c.SelectedItem = root.currentValue;
                    c.SelectedIndexChanged += c_SelectedIndexChanged;
                }
                
            }

            if (root.type == "image")
            {
                (t as TextBox).Text = root.currentValue as string;
                (t as TextBox).TextChanged += MenuSettings_TextChanged;
                (t as TextBox).MouseDoubleClick += MenuSettings_MouseDoubleClick;
                //(t as TextBox).MouseEnter += MenuSettings_MouseEnter;
                (t as TextBox).MouseLeave += MenuSettings_MouseLeave;
                (t as TextBox).MouseHover += MenuSettings_MouseHover;
            }

            if (root.type == "color")
            {
                //(t as Panel).Height = 20;
                (t as Panel).Width = 50;
                (t as Panel).BorderStyle = BorderStyle.FixedSingle;
                (t as Panel).Click += MenuSettings_Click;


                (t as Panel).BackColor = (Color)root.currentValue ;
                
            }
        }

        void MenuSettings_Click(object sender, EventArgs e)
        {
            Panel p = sender as Panel;
            ColorDialog dialog;
            if ((dialog = new ColorDialog(){FullOpen = true}).ShowDialog() == DialogResult.OK)
            {
                ItemTree item = GetItemFromControl(p);
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
