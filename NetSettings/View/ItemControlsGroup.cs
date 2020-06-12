using NetSettings.Controls;

namespace NetSettings.View
{
    internal class ItemControlsGroup
    {
        public LabelSingleClick label;
        public System.Windows.Forms.Control parentContainer;
        public System.Windows.Forms.Control control;
        public System.Windows.Forms.Button defaultButton;

        public bool Visible
        {
            set
            {
                label.Visible = parentContainer.Visible = control.Visible = value;
                if (defaultButton != null)
                    defaultButton.Visible = value;
            }
        }
    }
}
