using NetSettings.Common.Interfaces;
using NetSettings.Data;

namespace NetSettings.View
{
    public class DataViewParams
    {
        public IControlContainer container { get; set; } //TODO: Change to IntPtr? UInt64?
        public IControlContainer descriptionContainer { get; set; } //TODO: Change to IntPtr? UInt64?
        public DataProvider dataProvider { get; set; }
        internal Filter filter;
        public DataViewPlacement placement = new DataViewPlacement();
        public IGuiProvider guiProvider;
    }
}
