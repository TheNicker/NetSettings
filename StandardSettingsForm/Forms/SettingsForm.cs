﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetSettings.Data;
using NetSettings.View;

namespace NetSettings.Forms
{
    public partial class SettingsForm : Form
    {
        DataView fMenuSettings;
        DataProvider fData;

        public delegate void OnSaveDelegate();
        public event OnSaveDelegate OnSave = delegate { };
        
        public SettingsForm(DataProvider aData) 
        {
            InitializeComponent();
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            fData = aData;
            StartPosition = FormStartPosition.CenterParent;
            DoubleBuffered = true;
            fMenuSettings = new DataView();
            DataViewParams c = new DataViewParams();
            c.dataProvider =  aData;
            controlContainer1.AutoScroll = true;
            c.container = controlContainer1;
            c.descriptionContainer = controlContainer2;
            
            c.placement.TitleMaxWidth = 250;
            c.placement.ControlMaxWidth = 150;
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
            fMenuSettings.SetFilter(new Filter() { IncludeName = (sender as TextBox).Text });
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        //protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        //{
            
        //    if (keyData == Keys.Escape || keyData == Keys.Enter)
        //    {
        //        if (keyData == Keys.Escape)
        //            Close();
        //        return true;
        //    }
        //    return base.ProcessCmdKey(ref msg, keyData);
        //}

        private void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            OnSave();
        }

        private void SettingsForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Hide();
        }
    }
}
