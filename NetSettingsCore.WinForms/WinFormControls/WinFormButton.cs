using System.Windows.Forms;
using NetSettingsCore.Common;
using FlatStyle = NetSettingsCore.Common.FlatStyle;

namespace NetSettingsCore.WinForms.WinFormControls
{
    internal class WinFormButton : WinFormControl, IButton
    {
        private readonly Button _button = new Button();

        public WinFormButton()
        {
            _control = _button;
        }

        public FlatStyle FlatStyle
        {
            get => (FlatStyle)_button.FlatStyle;
            set => _button.FlatStyle = (System.Windows.Forms.FlatStyle)value;
        }
    }
}