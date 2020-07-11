using NetSettings.Data;
using NetSettingsCore.Common.Classes;

namespace NetSettings.View
{
    public class VisualItem
    {
        public Color PanelBackgroundColor;
        public ItemTree Item;
        public bool IsFiltered;
        public ItemControlsGroup controlsGroup;
        public bool Expanded = true;
        public VisualItem[] subItems;
    }
}
