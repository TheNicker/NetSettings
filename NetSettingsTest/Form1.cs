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
        MenuSettings settings;
        CreationParams fCreationParameters;
        Timer fFilterTimer;
        Filter fSettingsFilter;
        public Form1()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            settings = new MenuSettings();
            fCreationParameters = new CreationParams();
            fCreationParameters.panel = userControl11;
            fCreationParameters.descriptionPanel = splitContainer2.Panel2;
            fCreationParameters.root = ItemTree.FromFile(SettingsFilePath);

            Object a = fCreationParameters.root["inputsettings.enablemouse"];
            
            settings.Create(fCreationParameters);

            fFilterTimer = new Timer();
            fFilterTimer.Tick += fFilterTimer_Tick;
            fFilterTimer.Interval = 300;
        }

        void fFilterTimer_Tick(object sender, EventArgs e)
        {
            fFilterTimer.Enabled = false;
            userControl11.StartUpdate();
            settings.SetFilter(fSettingsFilter);
            userControl11.EndUpdate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fCreationParameters.root.ToFile(SettingsFilePath);
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
