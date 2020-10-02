using System.Windows.Forms;
using NetSettings.Common.Interfaces;

namespace NetSettings.WinForms.WinFormControls
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