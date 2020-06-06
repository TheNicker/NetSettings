using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSettings.Data;

namespace NetSettings
{
    internal class VisualItem
    {
        public Color PanelBackgroundColor;
        public ItemTree Item;
        public bool IsFiltered;
        public ItemControlsGroup controlsGroup;
        public bool Expanded = true;
        public VisualItem[] subitems;
    }
}
