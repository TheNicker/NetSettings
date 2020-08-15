using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NetSettings.Data;
using NetSettings.Forms;
using NetSettings.View;
using NetSettingsCore.Common;
using NetSettings.View;
using NetSettingsCore.WinForms;
using Newtonsoft.Json;
using TextBox = System.Windows.Forms.TextBox;

namespace NetSettingsTestCore.Forms
{
    public partial class Form1 : Form
    {
        private const string UserPath = @".\settings";
        private Dictionary<string, object> fUserSettings;
        private readonly DataView fView;
        private readonly DataViewParams fDataViewParams;
        private readonly DataProvider fData;
        private readonly SettingsForm fSettingsForm;

        public Form1()
        {
            InitializeComponent();
            fView = new DataView();
            const string settingsFilePath = @"Resources\GuiTemplate.json";
            fData = new DataProvider(ItemTree.FromFile(settingsFilePath));
            //Create manually view[1]
            fDataViewParams = new DataViewParams
            {
                guiProvider = new WinFormGuiProvider(),
                dataProvider = fData,
                container = userControl11,
                descriptionContainer = controlContainer1
            };

            //Create view[2] with predefined 'SettingsForm' from the same data provider
            //fSettingsForm = new SettingsForm((DataProvider)fData, new DataViewParams(), new DataView());
            fSettingsForm = new SettingsForm(fData);
        }

        private void Initialize()
        {
            fData.ItemChanged += fData_ItemChanged;
            LoadSettings();
            fData.DataBinding = fUserSettings;

            fView.Create(fDataViewParams);//TODO: Compare the init process of this form with the SettingsForm

            fSettingsForm.OnSave += fSettingsForm_OnSave;
            fSettingsForm.Show();
        }

        private void Save()
        {
            string text = JsonConvert.SerializeObject(fUserSettings, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });

            File.WriteAllText(UserPath, text);
        }

        private void LoadSettings()
        {
            if (!File.Exists(UserPath))
            {
                fUserSettings = fData.GenerateDefaultOptionsSet();
                Save();
            }
            else
            {
                string text = File.ReadAllText(UserPath);
                fUserSettings = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
            }
        }

        //TODO: Delete this event
        private void fData_ItemChanged(ItemChangedArgs changedArgs)
        {
            if (changedArgs.ChangedMode == ItemChangedMode.UserConfirmed)
            {
                var k = 0;
            }
        }

        private void fSettingsForm_OnSave()
        {
            Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //fCreationParameters.root.fRootTemplate.ToFile(SettingsFilePath);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            fView.SetFilter(new Filter { IncludeName = (sender as TextBox).Text });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Initialize();
        }
    }
}