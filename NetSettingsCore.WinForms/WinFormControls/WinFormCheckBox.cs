using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using NetSettingsCore.Common;
using NetSettingsCore.Common.Classes;
using NetSettingsCore.Common.Interfaces;

namespace NetSettingsCore.WinForms.WinFormControls
{
    internal class WinFormCheckBox : WinFormControl, ICheckBox
    {
        private readonly CheckBox _checkBox = new CheckBox();

        public WinFormCheckBox()
        {
            _control = _checkBox;
        }

        public bool Checked { get => _checkBox.Checked; set => _checkBox.Checked = value; }
    }
}