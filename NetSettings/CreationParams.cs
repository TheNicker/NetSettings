using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSettings
{
    public class CreationParams
    {
        public ControlContainer container;
        public ControlContainer descriptionContainer;
        public DataEntity root;
        public Filter filter;
        public PlacementParams placement = new PlacementParams();
    }
}
