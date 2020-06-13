using NetSettingsCore.Common;

namespace NetSettings
{
    public class DataViewPlacement
    {
        public int LineSpacing = 25;
        public int TitleMaxWidth { get; set; }
        public int TitleSpacing = 30;
        public int ControlMaxWidth = 80;
        public int ControlSpacing = 20;
        public int LineHeight = 20;
        public int DefaultButtonWidth = 50;
        public int HorizontalMargin = 20;

        public DataViewPlacement()
        {
            TitleMaxWidth = 150;
        }
    }
}
