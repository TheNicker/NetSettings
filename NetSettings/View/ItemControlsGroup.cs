using NetSettings.Common.Interfaces;

namespace NetSettings.View
{
    public class ItemControlsGroup
    {
        public ILabelSingleClick label;
        public IControl parentContainer;
        public IControl control;
        public IButton defaultButton;

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
