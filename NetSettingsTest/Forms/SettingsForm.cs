using System;
using System.Windows.Forms;
using NetSettings.Data;
using NetSettings.View;
using NetSettings.WinForms;

namespace NetSettingsTest.Forms
{
    public partial class SettingsForm : Form
    {
        private readonly DataView fMenuSettings;
        private readonly DataProvider fData;//TODO: can delete?

        public delegate void OnSaveDelegate();
        public event OnSaveDelegate OnSave = delegate { };

        //class ControlContainer : NetSettingsCore.Common.IControlContainer
        //{
        //    public ControlContainerWrapper(NetSettings.Controls.ControlContainer actualControl)
        //    {
        //        fControlContainer = actualControl;
        //    }
        //    private NetSettings.Controls.ControlContainer fControlContainer;
        //}
        //public SettingsForm(DataProvider aData, DataViewParams c, DataView menuSettings)
        public SettingsForm(DataProvider aData)
        {
            InitializeComponent();
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            fData = aData;
            StartPosition = FormStartPosition.CenterParent;
            DoubleBuffered = true;
            fMenuSettings = new DataView(); //TODO: Remove this line
            //fMenuSettings = menuSettings; //TODO: Should this enabled?
            controlContainer1.AutoScroll = true;
            //ControlContainerWrapper wrapper = new ControlContainerWrapper(controlContainer1);
            DataViewParams c = new DataViewParams(); //TODO: Remove this line
            c.guiProvider = new WinFormGuiProvider();
            c.dataProvider = aData;
            //c.container = (UInt64)controlContainer1.Handle;
            c.container = controlContainer1;
            c.descriptionContainer = controlContainer2;
            c.placement.TitleMaxWidth = 200;

            fMenuSettings.Create(c);
            this.MouseWheel += SettingsForm_MouseWheel;
            c.dataProvider.ItemChanged += root_ItemChanged;
        }

        void root_ItemChanged(ItemChangedArgs args)
        {
            btnSave.Enabled = true;
        }

        void SettingsForm_MouseWheel(object sender, MouseEventArgs e)
        {
            controlContainer1.ScrollY(e.Delta);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //Filter filter = null;
            fMenuSettings.SetFilter(new Filter() { IncludeName = (sender as TextBox).Text });
            //fMenuSettings.SetFilter(filter, true); //TODO: Implement correctlly according to the line above
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            if (keyData == Keys.Escape || keyData == Keys.Enter)
            {
                if (keyData == Keys.Escape)
                    Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            OnSave();
        }
    }
}
