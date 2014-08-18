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

namespace NetSettings
{
    public partial class Form1 : Form
    {
        const string SettingsFilePath = @"Resources\GuiTemplate.json";
        MenuSettings settings;
        CreationParams fCreationParameters;
        public Form1()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            settings = new MenuSettings();
            fCreationParameters = new CreationParams();
            fCreationParameters.panel = splitContainer2.Panel1;
            fCreationParameters.descriptionPanel = splitContainer2.Panel2;
            fCreationParameters.root = ItemTree.FromFile(SettingsFilePath);

            Object a = fCreationParameters.root["inputsettings.enablemouse"];
            
            settings.Create(fCreationParameters);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fCreationParameters.root.ToFile(SettingsFilePath);
        }
    }
}
