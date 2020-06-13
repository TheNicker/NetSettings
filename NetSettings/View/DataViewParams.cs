//using NetSettings.Controls;
//using NetSettings.Data;
//using NetSettingsCore.Common;

using NetSettings.Data;
using NetSettingsCore.Common;

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
