using NetSettings.Data;
using System.Drawing;

namespace NetSettings.View
{
    internal class VisualItem
    {
        public Color PanelBackgroundColor;
        public ItemTree Item;
        public bool IsFiltered;
        public ItemControlsGroup controlsGroup;
        public bool Expanded = true;
        public VisualItem[] subItems;
    }
}
