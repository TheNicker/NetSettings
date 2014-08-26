using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSettings
{
    public class VisualItem
    {
        public Color PanelBackgroundColor;
        public ItemTree Item;
        public bool IsVisible;
        public VisualItem[] subitems;
        public ItemControlsGroup controlsGroup;
    }
}
