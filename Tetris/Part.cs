namespace Tetris
{
    public class Part
    {
        private Brush brush;
        public int X;
        public int Y;

        public Part(int X,int Y,Brush brush)
        {
            this.X = X;
            this.Y = Y;
            this.brush = brush;
        }

        public Brush Brush
        {
            get
            {
                return this.brush;
            }
        }
    }
}
