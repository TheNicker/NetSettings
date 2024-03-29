﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NetSettings.Data;
using NetSettings.Forms;
using NetSettings.View;


namespace NetSettingsTest
{
    public partial class Form1 : Form
    {
        private const string SettingsFilePath = @"Resources\GuiTemplate.json";
        private const string UserPath = @".\settings";
        private Dictionary<string, object> fUserSettings;
        private DataView fView;
        private DataViewParams fDataViewParams;
        private DataProvider fData;
        private SettingsForm fSettingsForm;

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


            //Create manually view[1]
            fDataViewParams = new DataViewParams
            {
                container = userControl11,
                descriptionContainer = controlContainer1,
                dataProvider = fData
            };

            LoadSettings();
            fData.DataBinding = fUserSettings;
            fView.Create(fDataViewParams);

            //Create view[2] with predefined 'SettingsForm' from the same data provider
            fSettingsForm = new SettingsForm(fData);
            fSettingsForm.OnSave += fSettingsForm_OnSave;
            fSettingsForm.Show();
        }

        private void Save()
        {
            UserDataSerializer.SaveToFile(fData.RootTemplate, fData.DataBinding, UserPath);
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
                fUserSettings  = UserDataSerializer.LoadFromFile(UserPath);
            }
        }


        private void fData_ItemChanged(ItemChangedArgs changedArgs)
        {
            if (changedArgs.ChangedMode == ItemChangedMode.UserConfirmed)
            {
                
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
    }
}