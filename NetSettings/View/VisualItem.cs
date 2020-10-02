using System.Collections.Generic;
using NetSettings.Common.Classes;
using NetSettings.Data;

namespace NetSettings.View
{
    public class VisualItem
    {
        public Color PanelBackgroundColor;
        public ItemTree Item;
        public bool IsFiltered;
        public ItemControlsGroup controlsGroup;
        public bool Expanded = true;
        public IList<VisualItem> subItems;
    }
}
