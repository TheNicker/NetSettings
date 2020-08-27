using NetSettings.Data;
using NetSettings.View;
using NetSettingsCore.Common.Classes;

namespace NetSettingsCore.View
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
