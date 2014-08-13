using Newtonsoft.Json;
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
            fCreationParameters.root = Item.FromFile(SettingsFilePath);
            settings.Create(fCreationParameters);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = JsonConvert.SerializeObject(fCreationParameters.root ,Formatting.Indented);
            File.WriteAllText(SettingsFilePath, text);
        }
    }
}
