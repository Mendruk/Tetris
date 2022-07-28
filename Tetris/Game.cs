namespace Tetris
{
    public class Game
    {
        private Random random = new Random();
        private Brush figureBrush;
        private Pen figurePen;
        private int width = 10;
        private int height = 20;
        private Point[] currentFigure;
        private Point[] nextFigure;
        private Point[,] partsOnBotton;
        private Point figureReferencePoint;

        public Game()
        {
            figureBrush = Brushes.Blue;
            figurePen = Pens.Black;
            currentFigure = CreateFigure(GetRandomFigureType());
            figureReferencePoint = currentFigure[0];
            MoveFigureToCenterOfFieldWidth();
            nextFigure = CreateFigure(GetRandomFigureType());
            partsOnBotton = new Point[width, height];
        }

        public void Update()
        {
            if (!CheckBlockVertical()) MoveFigureOnePointDown();
            else
            {
                foreach (Point point in currentFigure)
                {
                    partsOnBotton[point.X, point.Y] = point;
                }

                currentFigure = nextFigure;
                figureReferencePoint = currentFigure[0];
                MoveFigureToCenterOfFieldWidth();
                nextFigure = CreateFigure(GetRandomFigureType());
            }
        }

        private void MoveFigureOnePointDown()
        {
            foreach (Point point in currentFigure)
            {
                point.Y++;
            }
        }
        private void MoveFigureOnePointHorizontal(int direction)
        {
            foreach (Point point in currentFigure)
            {
                point.X += direction;
            }
        }

        private bool CheckBlockVertical()
        {
            foreach (Point point in currentFigure)
            {
                if (point.Y == height - 1 || partsOnBotton[point.X, point.Y + 1] != null) return true;
            }
            return false;
        }

        private bool CheckBlockHorizontal(int direction)
        {
            foreach (Point point in currentFigure)
            {
                if (direction == 1 && (point.X == width - 2 + direction
                                       || partsOnBotton[point.X + direction, point.Y] != null)) return true;
                if (direction == -1 && (point.X == 1 + direction
                                        || partsOnBotton[point.X + direction, point.Y] != null)) return true;
            }
            return false;
        }

        public void RotateFigure()
        {
            Point[] rotationCurrentFigure = new Point[currentFigure.Length];

            for (int i = 0; i < currentFigure.Length; i++)
            {
                rotationCurrentFigure[i] = new Point(currentFigure[i].X, currentFigure[i].Y);
                int tempX = figureReferencePoint.X + (figureReferencePoint.Y - rotationCurrentFigure[i].Y);
                rotationCurrentFigure[i].Y = figureReferencePoint.Y + (rotationCurrentFigure[i].X - figureReferencePoint.X);
                rotationCurrentFigure[i].X = tempX;

                if (rotationCurrentFigure[i].X < 0 || rotationCurrentFigure[i].X >= width || rotationCurrentFigure[i].Y < 0
                    || rotationCurrentFigure[i].Y >= height || partsOnBotton[rotationCurrentFigure[i].X, rotationCurrentFigure[i].Y] != null)
                    return;
            }
            currentFigure = rotationCurrentFigure;
            figureReferencePoint = currentFigure[0];
        }

        public void MoveFigureHorizontal(int direction)
        {
            if (!CheckBlockHorizontal(direction))
                MoveFigureOnePointHorizontal(direction);
        }

        public void PutDownFigure()
        {
            while (!CheckBlockVertical()) MoveFigureOnePointDown();
        }

        private Point[] CreateFigure(FigureType type)
        {
            switch (type)
            {
                case FigureType.O:
                    return new Point[] { new Point(0, 0), new Point(0, 1), new Point(1, 0), new Point(1, 1) };
                case FigureType.J:
                    return new Point[] { new Point(1, 1), new Point(1, 0), new Point(0, 2), new Point(1, 2) };
                case FigureType.L:
                    return new Point[] { new Point(0, 1), new Point(0, 0), new Point(0, 2), new Point(1, 2) };
                case FigureType.S:
                    return new Point[] { new Point(1, 1), new Point(1, 0), new Point(2, 0), new Point(0, 1) };
                case FigureType.Z:
                    return new Point[] { new Point(1, 1), new Point(0, 0), new Point(1, 0), new Point(2, 1) };
                case FigureType.T:
                    return new Point[] { new Point(1, 0), new Point(0, 0), new Point(2, 0), new Point(1, 1) };
                case FigureType.I:
                    return new Point[] { new Point(1, 0), new Point(0, 0), new Point(2, 0), new Point(3, 0) };
                case FigureType.Dot:
                default:
                    return new Point[] { new Point(0, 0) };
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
            foreach (Point point in currentFigure)
            {
                point.X += width / 2 - 1;
            }
        }

        public void DrawFigure(Graphics graphics, int fieldWidth, int fieldHeight)
        {
            foreach (Point part in currentFigure)
            {
                graphics.FillRectangle(figureBrush, fieldWidth / this.width * part.X, fieldHeight / this.height * part.Y,
                    fieldWidth / this.width, fieldHeight / this.height);
                graphics.DrawRectangle(figurePen, fieldWidth / this.width * part.X, fieldHeight / this.height * part.Y,
                    fieldWidth / this.width, fieldHeight / this.height);
            }
            graphics.FillRectangle(Brushes.Brown, fieldWidth / this.width * figureReferencePoint.X, fieldHeight / this.height * figureReferencePoint.Y,
                fieldWidth / this.width, fieldHeight / this.height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (partsOnBotton[x, y] != null)
                    {
                        graphics.FillRectangle(figureBrush, fieldWidth / this.width * x, fieldHeight / this.height * y,
                            fieldWidth / this.width, fieldHeight / this.height);
                        graphics.DrawRectangle(figurePen, fieldWidth / this.width * x, fieldHeight / this.height * y,
                            fieldWidth / this.width, fieldHeight / this.height);
                    }

                }
            }
        }

        public void DrawNextFigure(Graphics graphics, int fieldWidth, int fieldHeight)
        {
            foreach (Point point in nextFigure)
            {
                graphics.FillRectangle(figureBrush, fieldWidth / this.width * point.X, fieldHeight / this.height * point.Y,
                    fieldWidth / this.width, fieldHeight / this.height);
                graphics.DrawRectangle(figurePen, fieldWidth / this.width * point.X, fieldHeight / this.height * point.Y,
                    fieldWidth / this.width, fieldHeight / this.height);
            }
        }
    }

    enum FigureType { O, J, L, S, Z, T, I, Dot };
}
