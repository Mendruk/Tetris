namespace Tetris
{
    public class Game
    {
        private Brush figureBrush;
        private Pen figurePen;
        private int width = 10;
        private int height = 20;
        private FigurePart[] currentFigure;

        public Game()
        {
            figureBrush = Brushes.Blue;
            figurePen = Pens.Black;
            currentFigure = CreateFigure(FigureType.O);
            MoveFigureToCenterOfFieldWidth();
        }

        public void Update()
        {
            if(!CheckBlock()) MoveFigureOnePointDown();
        }

        private void MoveFigureOnePointDown()
        {
            foreach(FigurePart part in currentFigure)
            {
                part.Y++;
            }
        }

        private bool CheckBlock()
        {
            foreach (FigurePart part in currentFigure)
            {
                if (part.Y == height-1) return true;
            }
            return false;
        }
        private FigurePart[] CreateFigure(FigureType type)
        {
            switch (type)
            {
                case FigureType.O:
                default:
                    return new FigurePart[] { new FigurePart(0, 0), new FigurePart(0, 1), new FigurePart(1, 0), new FigurePart(1, 1) };
            }
        }

        private void MoveFigureToCenterOfFieldWidth()
        {
            foreach (FigurePart part in currentFigure)
            {
                part.X += width / 2 - 1;
            }
        }
        public void Draw(Graphics graphics,int fieldWidth,int fieldHeight)
        {
            foreach (FigurePart part in currentFigure)
            {
                graphics.FillRectangle(figureBrush, fieldWidth / this.width * part.X, fieldHeight / this.height * part.Y,
                    fieldWidth / this.width, fieldHeight / this.height);
                graphics.DrawRectangle(figurePen, fieldWidth / this.width * part.X, fieldHeight / this.height * part.Y,
                    fieldWidth / this.width, fieldHeight / this.height);
            }
        }
    }
    enum FigureType { O };
}
