using System.Windows.Forms;
using NetSettings.Common.Interfaces;
using FlatStyle = NetSettings.Common.Classes.FlatStyle;

namespace NetSettings.WinForms.WinFormControls
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