namespace NetSettings.View
{
    public class DataViewPlacement
    {
        public int LineSpacing = 35;
        public int TitleMaxWidth { get; set; }
        public int TitleSpacing = 30;
        public int ControlMaxWidth = 80;
        public int ControlSpacing = 20;
        public int LineHeight = 25;
        public int DefaultButtonWidth = 60;
        public int HorizontalMargin = 20;

        public DataViewPlacement()
        {
            TitleMaxWidth = 150;
        }
    }
}
