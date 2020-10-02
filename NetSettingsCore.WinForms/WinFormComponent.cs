using System.ComponentModel;
using System.Windows.Forms;
using IComponent = NetSettings.Common.Interfaces.IComponent;

namespace NetSettings.WinForms
{
    public class WinFormComponent : IComponent
    {
        protected Component _component;

        public WinFormComponent()
        {
            _component = new Component();
        }
    }
}