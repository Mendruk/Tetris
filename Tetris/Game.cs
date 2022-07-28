namespace Tetris
{
    public class Game
    {
        private Random random = new Random();
        private Brush figureBrush;
        private Pen figurePen;
        private int width = 10;
        private int height = 20;
        private FigurePart[] currentFigure;
        private FigurePart[] nextFigure;

        public Game()
        {
            figureBrush = Brushes.Blue;
            figurePen = Pens.Black;
            currentFigure = CreateFigure(GetRandomFigureType());
            MoveFigureToCenterOfFieldWidth();
            nextFigure = CreateFigure(GetRandomFigureType());
        }

        public void Update()
        {
            if (!CheckBlockVertical()) MoveFigureOnePointDown();
            else
            {
                currentFigure = nextFigure;
                MoveFigureToCenterOfFieldWidth();
                nextFigure = CreateFigure(GetRandomFigureType());
            }
        }

        private void MoveFigureOnePointDown()
        {
            foreach (FigurePart part in currentFigure)
            {
                part.Y++;
            }
        }
        private void MoveFigureOnePointHorizontal(int direction)
        {
            foreach (FigurePart part in currentFigure)
            {
                part.X+=direction;
            }
        }

        private bool CheckBlockVertical()
        {
            foreach (FigurePart part in currentFigure)
            {
                if (part.Y == height - 1) return true;
            }
            return false;
        }

        private bool CheckBlockHorizontal(int direction)
        {
            foreach (FigurePart part in currentFigure)
            {
                if (direction == 1 && part.X == width-2 + direction) return true;
                if (direction == -1 && part.X == 1 + direction) return true;
            }
            return false;
        }

        public void RotateFigure()
        {

        }
        
        public void MoveFigureHorizontal(int direction)
        {
            if (!CheckBlockHorizontal(direction))
                MoveFigureOnePointHorizontal(direction);
        }

        public void PutDownFigure()
        {
            for (; !CheckBlockVertical(); MoveFigureOnePointDown()) ;
        }

        private FigurePart[] CreateFigure(FigureType type)
        {
            switch (type)
            {
                case FigureType.O:
                    return new FigurePart[] { new FigurePart(0, 0), new FigurePart(0, 1), new FigurePart(1, 0), new FigurePart(1, 1) };
                case FigureType.J:
                    return new FigurePart[] { new FigurePart(1, 0), new FigurePart(1, 1), new FigurePart(0, 2), new FigurePart(1, 2) };
                case FigureType.L:
                    return new FigurePart[] { new FigurePart(0, 0), new FigurePart(0, 1), new FigurePart(0, 2), new FigurePart(1, 2) };
                case FigureType.S:
                    return new FigurePart[] { new FigurePart(1, 0), new FigurePart(2, 0), new FigurePart(0, 1), new FigurePart(1, 1) };
                case FigureType.Z:
                    return new FigurePart[] { new FigurePart(0, 0), new FigurePart(1, 0), new FigurePart(1, 1), new FigurePart(2, 1) };
                case FigureType.T:
                    return new FigurePart[] { new FigurePart(0, 0), new FigurePart(1, 0), new FigurePart(2, 0), new FigurePart(1, 1) };
                case FigureType.I:
                    return new FigurePart[] { new FigurePart(0, 0), new FigurePart(1, 0), new FigurePart(2, 0), new FigurePart(3, 0) };
                case FigureType. Dot:
                    default:
                    return new FigurePart[] { new FigurePart(0, 0) };
            }
        }

        private FigureType GetRandomFigureType()
        {
            int randomNumber = random.Next(Enum.GetNames(typeof(FigureType)).Length);

            switch (randomNumber)
            {
                case 0:
                    return FigureType.O;
                case 1:
                    return FigureType.J;
                case 2:
                    return FigureType.L;
                case 3:
                    return FigureType.S;
                case 4:
                    return FigureType.Z;
                case 5:
                    return FigureType.T;
                case 6:
                    return FigureType.I;
                case 7:
                default:
                    return FigureType.Dot;
            }
        }

        private void MoveFigureToCenterOfFieldWidth()
        {
            foreach (FigurePart part in currentFigure)
            {
                part.X += width / 2 - 1;
            }
        }

        public void DrawFigure(Graphics graphics, int fieldWidth, int fieldHeight)
        {
            foreach (FigurePart part in currentFigure)
            {
                graphics.FillRectangle(figureBrush, fieldWidth / this.width * part.X, fieldHeight / this.height * part.Y,
                    fieldWidth / this.width, fieldHeight / this.height);
                graphics.DrawRectangle(figurePen, fieldWidth / this.width * part.X, fieldHeight / this.height * part.Y,
                    fieldWidth / this.width, fieldHeight / this.height);
            }
        }

        public void DrawNextFigure(Graphics graphics, int fieldWidth, int fieldHeight)
        {
            foreach (FigurePart part in nextFigure)
            {
                graphics.FillRectangle(figureBrush, fieldWidth / this.width * part.X, fieldHeight / this.height * part.Y,
                    fieldWidth / this.width, fieldHeight / this.height);
                graphics.DrawRectangle(figurePen, fieldWidth / this.width * part.X, fieldHeight / this.height * part.Y,
                    fieldWidth / this.width, fieldHeight / this.height);
            }
        }
    }

    enum FigureType { O, J, L, S, Z, T, I, Dot};
}
