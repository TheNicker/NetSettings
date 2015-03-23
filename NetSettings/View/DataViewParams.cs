using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSettings
{
    public class DataViewParams
    {
        public ControlContainer container { get; set; }
        public ControlContainer descriptionContainer { get; set; }
        public DataProvider dataProvider { get; set; }
        public Filter filter;
        public DataViewPlacement placement = new DataViewPlacement();
    }
}
