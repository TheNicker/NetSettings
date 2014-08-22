using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSettings
{
    public class VisualItem
    {
        public ItemTree Item;
        public bool IsVisible;
        public VisualItem[] subitems;
        public ControlsGroup controlsGroup;
    }
}
