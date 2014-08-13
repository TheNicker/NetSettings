using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetSettings
{
    public struct CreationParams
    {
        public Control panel;
        public Control descriptionPanel;
        public Item root;
    }
    public class MenuSettings
    {
        private Dictionary<string, Type> fStringToType;
        Point currentPosition = new Point();
        const int LineSpacing = 15;
        const int HorizontalMArgin = 20;
        int Nesting = 1;
        Graphics gfx;
        Control fDescriptionPanel;
        TextBox fDescriptionTextBox;
        public MenuSettings()
        {
            fStringToType = new Dictionary<string, Type>();
            fStringToType.Add("text", typeof(TextBox));
            fStringToType.Add("bool", typeof(CheckBox));
            fStringToType.Add("menu", typeof(Label));
        }

        public void Create(CreationParams aParams)
        {
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
        }
        private void AddControlRecursivly(CreationParams  aParams)
        {
            Item root = aParams.root;
            Control control = aParams.panel;
            Type type;
            if (fStringToType.TryGetValue(root.type, out type))
                AddControl(control, root, type);

            //Add children
            if (root.subitems != null)
                foreach (Item item in root.subitems)
                {
                    aParams.root = item;
                    AddControlRecursivly(aParams);
                }
        }

        private void AddMenu(Item root, Type type)
        {

        }

        private void AddControl(Control parent, Item root, Type type)
        {
            currentPosition.X = HorizontalMArgin * Nesting;
            currentPosition.Y += LineSpacing;
            Control t = Activator.CreateInstance(type) as Control;
            Label l = new Label();
            l.AutoSize = true;
            l.Text = root.displayname;
            l.Tag = root;
            l.Location = currentPosition;
            l.MouseEnter += l_MouseEnter;
            //l.MouseHover += l_MouseHover;
            currentPosition.X = HorizontalMArgin * Nesting;
            parent.Controls.Add(l);
            SizeF size = gfx.MeasureString(l.Text, l.Font);
            //l.Width = (int)size.Width;
            currentPosition.X += (int)size.Width + 5;
            parent.Controls.Add(t);
            t.Location = currentPosition;
            currentPosition.Y += LineSpacing;
            t.Tag = root;
            ProceeControl(l, t, root);
        }

        void l_MouseEnter(object sender, EventArgs e)
        {
            Item item = (sender as Control).Tag as Item;
            if (item != null)
            {
                if (item.description != null)
                    fDescriptionTextBox.Text = item.description;
            }
        }

        void l_MouseHover(object sender, EventArgs e)
        {
           
            
        }

        private void ProceeControl(Control l, Control t, Item root)
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

        }

        Item GetItemFromControl(Control aControl)
        {
            return aControl.Tag as Item;
        }

        void MenuSettings_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;
            Item item = GetItemFromControl(textbox);
            item.value = textbox.Text;
        }

        void MenuSettings_CheckStateChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            Item item = checkBox.Tag as Item;
            item.value = checkBox.Checked;
        }
    }
}
