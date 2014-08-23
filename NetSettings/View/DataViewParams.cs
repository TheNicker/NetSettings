using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSettings
{
    public class DataViewParams
    {
        public ControlContainer container;
        public ControlContainer descriptionContainer;
        public DataProvider dataProvider;
        public Filter filter;
        public DataViewPlacement placement = new DataViewPlacement();
    }
}
