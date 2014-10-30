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
using Newtonsoft.Json;

namespace NetSettings
{
    public partial class Form1 : Form
    {
        const string SettingsFilePath = @"Resources\GuiTemplate.json";
        const string UserPAth = @".\settings";
        Dictionary<string, object> fUserSettings;
        DataView fView;
        DataViewParams fDataViewParams;
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
            fData.ItemChanged += fData_ItemChanged;
            Load();
            fData.DataBinding = fUserSettings;

            //Create manually view[1]
            fDataViewParams = new DataViewParams();
            fDataViewParams.container = userControl11;
            fDataViewParams.descriptionContainer = controlContainer1;
            fDataViewParams.dataProvider = fData;
            fView.Create(fDataViewParams);

            //Create view[2] with predefined 'SettingsForm' fro the same data provider
            fSettingsForm = new SettingsForm(fData);
            fSettingsForm.OnSave += fSettingsForm_OnSave;
            fSettingsForm.Show();
            
            
        }

        public  void Save()
        {
            string text = JsonConvert.SerializeObject(fUserSettings, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });

            File.WriteAllText(UserPAth, text);
        }

        void Load()
        {
            if (!File.Exists(UserPAth))
            {
                fUserSettings = fData.GenerateDefaultOptionsSet();
                Save();

            }
            else
            {
                string text = File.ReadAllText(UserPAth);
                fUserSettings = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
            }

        }

        
        void fData_ItemChanged(ItemChangedArgs changedArgs)
        {
            if (changedArgs.ChangedMode == ItemChangedMode.UserConfirmed)
            {
                int k = 0;
            }
        }

        void fSettingsForm_OnSave()
        {
            Save();
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            //fCreationParameters.root.fRootTemplate.ToFile(SettingsFilePath);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            fView.SetFilter(new Filter() { IncludeName = (sender as TextBox).Text});
        }
    }
}
