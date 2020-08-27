namespace NetSettings.Common.Classes
{
    public class Point
    {
        public Point() : this(0, 0) //TODO: Check if this constructor is needed
        {
        }

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}
