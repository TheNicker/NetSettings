using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetSettingsTest;

namespace NetSettings
{
    public partial class Form1 : Form
    {
        const string SettingsFilePath = @"Resources\GuiTemplate.json";
        DataView fView;
        DataViewParams fDataViewParams;
        Timer fFilterTimer;
        Filter fSettingsFilter;
        DataProvider fData;

        SettingsForm fSettingsForm;
        public Form1()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            fView = new DataView();
            fData = new DataProvider(ItemTree.FromFile(SettingsFilePath));
            fDataViewParams = new DataViewParams();
            fDataViewParams.container = userControl11;
            fDataViewParams.descriptionContainer = controlContainer1;
            fDataViewParams.dataProvider = fData;
            

            fSettingsForm = new SettingsForm(fData);
            fSettingsForm.OnSave += fSettingsForm_OnSave;
            fSettingsForm.Show();
            
            fView.Create(fDataViewParams);

            fFilterTimer = new Timer();
            fFilterTimer.Tick += fFilterTimer_Tick;
            fFilterTimer.Interval = 300;
        }

        void fSettingsForm_OnSave()
        {
            int k = 0;
        }

        void fFilterTimer_Tick(object sender, EventArgs e)
        {
            fFilterTimer.Enabled = false;
            fView.SetFilter(fSettingsFilter);
            fView.RefreshTree();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //fCreationParameters.root.fRootTemplate.ToFile(SettingsFilePath);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            fSettingsFilter = new Filter()
            {
                IncludeName = (sender as TextBox).Text
            };
            fFilterTimer.Enabled = false;
            fFilterTimer.Enabled = true;
         
        }
    }
}
