namespace Tetris;

public class Figure
{
    private static readonly Dictionary<FigureType, Brush> brushes = new()
    {
        { FigureType.O, Brushes.DarkRed },
        { FigureType.J, Brushes.DarkOrange },
        { FigureType.L, Brushes.DarkGoldenrod },
        { FigureType.S, Brushes.DarkGreen },
        { FigureType.Z, Brushes.DarkCyan },
        { FigureType.T, Brushes.DarkBlue },
        { FigureType.I, Brushes.DarkViolet },
        { FigureType.Dot, Brushes.Gray }
    };
    // To be continued
    private static readonly Dictionary<FigureType, Figure> figures = new();

    private static readonly Random random = new();

    private readonly RotationType rotationTypeType;

    private FigureType figureType;

    private int X;

    private int Y;

    static Figure()
    {
        foreach (KeyValuePair<FigureType, Brush> brush in brushes) figures.Add(brush.Key, new Figure(brush.Key));
    }

    public Figure(FigureType figureType)
    {
        this.figureType = figureType;
        StatesOfFigurePoints = new List<Point[]>();

        if (brushes.TryGetValue(figureType, out Brush? brush))
        {
            switch (figureType)
            {
                case FigureType.O:
                    StatesOfFigurePoints.Add(new Point[] { new(0, 0), new(0, 1), new(1, 0), new(1, 1) });
                    rotationTypeType = RotationType.NotRotating;
                    break;
                case FigureType.J:
                    StatesOfFigurePoints.Add(new Point[] { new(1, 1), new(1, 0), new(0, 2), new(1, 2) });
                    rotationTypeType = RotationType.Rotating;
                    break;
                case FigureType.L:
                    StatesOfFigurePoints.Add(new Point[] { new(0, 1), new(0, 0), new(0, 2), new(1, 2) });
                    rotationTypeType = RotationType.Rotating;
                    break;
                case FigureType.S:
                    StatesOfFigurePoints.Add(new Point[] { new(1, 1), new(1, 0), new(2, 0), new(0, 1) });
                    rotationTypeType = RotationType.TwoStateOfTurn;
                    break;
                case FigureType.Z:
                    StatesOfFigurePoints.Add(new Point[] { new(1, 1), new(0, 0), new(1, 0), new(2, 1) });
                    rotationTypeType = RotationType.TwoStateOfTurn;
                    break;
                case FigureType.T:
                    StatesOfFigurePoints.Add(new Point[] { new(1, 0), new(0, 0), new(2, 0), new(1, 1) });
                    rotationTypeType = RotationType.Rotating;
                    break;
                case FigureType.I:
                    StatesOfFigurePoints.Add(new Point[] { new(1, 0), new(0, 0), new(2, 0), new(3, 0) });
                    rotationTypeType = RotationType.TwoStateOfTurn;
                    break;
                case FigureType.Dot:
                    StatesOfFigurePoints.Add(new Point[] { new(0, 0) });
                    rotationTypeType = RotationType.NotRotating;
                    break;
            }

            Brush = brush;
        }

        ReferencePoint = StatesOfFigurePoints[0][0];

        switch (rotationTypeType)
        {
            case RotationType.Rotating:
                FillOtherRotationState(3);
                break;
            case RotationType.TwoStateOfTurn:
                FillOtherRotationState(1);
                break;
            case RotationType.NotRotating:
            default:
                break;
        }
    }

    public Brush Brush { get; }

    public Point ReferencePoint { get; }

    public List<Point[]> StatesOfFigurePoints { get; set; }

    public int FigureRotationIndex { get; private set; }

    public static Figure GetRandomFigure()
    {
        if (figures.TryGetValue((FigureType)random.Next(Enum.GetNames(typeof(FigureType)).Length), out Figure? figure))
            return figure;
        return null;
    }

    public void ZeroingCoordinatesFigure()
    {
        X = 0;
        Y = 0;
    }

    public Point[] GetFigurePoints()
    {
        return StatesOfFigurePoints[FigureRotationIndex]
            .Select(point => new Point(point.X + X, point.Y + Y))
            .ToArray();
    }

    public Point[] GetNextRotationFigurePoints()
    {
        return StatesOfFigurePoints[(FigureRotationIndex + 1) % StatesOfFigurePoints.Count]
            .Select(point => new Point(point.X + X, point.Y + Y))
            .ToArray();
    }

    public void ReduceHorizontally(int indent)
    {
        X = indent;
    }

    public void DrawFigure(Graphics graphics, int cellWidth, int cellHeight)
    {
        foreach (Point part in GetFigurePoints())
            graphics.FillRectangle(Brush,
                cellWidth * part.X,
                cellHeight * part.Y,
                cellWidth,
                cellHeight);
    }

    public void DrawFigureStructure(Graphics graphics, int cellWidth, int cellHeight)
    {
        foreach (Point part in StatesOfFigurePoints[FigureRotationIndex])
            graphics.FillRectangle(Brush,
                cellWidth * part.X,
                cellHeight * part.Y,
                cellWidth,
                cellHeight);
    }

    public void MoveDown()
    {
        Y++;
    }

    public void MoveLeft()
    {
        X--;
    }

    public void MoveRight()
    {
        X++;
    }

    public void Rotate()
    {
        FigureRotationIndex = (FigureRotationIndex + 1) % StatesOfFigurePoints.Count;
    }

    private void FillOtherRotationState(int number)
    {
        for (int i = 1; i <= number; i++)
            StatesOfFigurePoints.Add(NextRorationStateOfFigure(StatesOfFigurePoints[i - 1]));
    }

    private Point[] NextRorationStateOfFigure(Point[] points)
    {
        return points
            .Select(point => new Point(
                ReferencePoint.X + (ReferencePoint.Y - point.Y),
                ReferencePoint.Y + (point.X - ReferencePoint.X)))
            .ToArray();
    }
}