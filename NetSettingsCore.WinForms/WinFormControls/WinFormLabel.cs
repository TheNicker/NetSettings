using NetSettings.Common.Interfaces;
using System.Windows.Forms;

namespace NetSettings.WinForms.WinFormControls
{
    internal class WinFormLabel : WinFormControl, ILabel
    {
        private readonly Label _label = new Label();

        public WinFormLabel()
        {
            _control = _label;
        }
    }
}