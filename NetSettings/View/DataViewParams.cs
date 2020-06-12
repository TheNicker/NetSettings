using NetSettings.Controls;
using NetSettings.Data;

namespace NetSettings.View
{
    public class DataViewParams
    {
        public ControlContainer container { get; set; }
        public ControlContainer descriptionContainer { get; set; }
        public DataProvider dataProvider { get; set; }
        internal Filter filter;
        internal DataViewPlacement placement = new DataViewPlacement();
    }
}
