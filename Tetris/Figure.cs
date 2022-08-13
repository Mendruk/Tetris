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

    private static readonly Dictionary<FigureType, Figure> Figures = new();

    private static readonly Random random = new();

    private readonly RotationType rotationType;

    private FigureType figureType;

    private int x;

    private int y;

    static Figure()
    {
        foreach (KeyValuePair<FigureType, Brush> brush in brushes)
            Figures.Add(brush.Key, new Figure(brush.Key));
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
                    rotationType = RotationType.NotRotating;
                    break;
                case FigureType.J:
                    StatesOfFigurePoints.Add(new Point[] { new(1, 1), new(1, 0), new(0, 2), new(1, 2) });
                    rotationType = RotationType.Rotating;
                    break;
                case FigureType.L:
                    StatesOfFigurePoints.Add(new Point[] { new(0, 1), new(0, 0), new(0, 2), new(1, 2) });
                    rotationType = RotationType.Rotating;
                    break;
                case FigureType.S:
                    StatesOfFigurePoints.Add(new Point[] { new(1, 1), new(1, 0), new(2, 0), new(0, 1) });
                    rotationType = RotationType.TwoStateOfTurn;
                    break;
                case FigureType.Z:
                    StatesOfFigurePoints.Add(new Point[] { new(1, 1), new(0, 0), new(1, 0), new(2, 1) });
                    rotationType = RotationType.TwoStateOfTurn;
                    break;
                case FigureType.T:
                    StatesOfFigurePoints.Add(new Point[] { new(1, 0), new(0, 0), new(2, 0), new(1, 1) });
                    rotationType = RotationType.Rotating;
                    break;
                case FigureType.I:
                    StatesOfFigurePoints.Add(new Point[] { new(1, 0), new(0, 0), new(2, 0), new(3, 0) });
                    rotationType = RotationType.TwoStateOfTurn;
                    break;
                case FigureType.Dot:
                    StatesOfFigurePoints.Add(new Point[] { new(0, 0) });
                    rotationType = RotationType.NotRotating;
                    break;
                default:
                    throw new Exception("Not correct figure type");
            }

            Brush = brush;
        }

        ReferencePoint = StatesOfFigurePoints[0][0];

        switch (rotationType)
        {
            case RotationType.Rotating:
                FillOtherRotationState(3);
                break;
            case RotationType.TwoStateOfTurn:
                FillOtherRotationState(1);
                break;
            case RotationType.NotRotating:
                break;
            default:
                throw new Exception("Not correct rotation type");
        }
    }
    public List<Point[]> StatesOfFigurePoints { get; set; }

    public int FigureRotationIndex { get; private set; }

    public Point ReferencePoint { get; }

    public Brush Brush { get; }

    public static Figure GetRandomFigure()
    {
        if (Figures.TryGetValue((FigureType)random.Next(Enum.GetNames(typeof(FigureType)).Length), out Figure? figure))
            return figure;
        else
            throw new Exception("Not correct brush");
    }

    public void ZeroingCoordinatesAndRotationOfFigure()
    {
        x = 0;
        y = 0;
        FigureRotationIndex = 0;
    }

    public Point[] GetFigurePoints()
    {
        return StatesOfFigurePoints[FigureRotationIndex]
            .Select(point => new Point(point.X + x, point.Y + y))
            .ToArray();
    }

    public Point[] GetNextRotationFigurePoints()
    {
        return StatesOfFigurePoints[(FigureRotationIndex + 1) % StatesOfFigurePoints.Count]
            .Select(point => new Point(point.X + x, point.Y + y))
            .ToArray();
    }

    public void ReduceHorizontally(int indent)
    {
        x = indent;
    }

    public void DrawFigure(Graphics graphics, int cellWidth, int cellHeight)
    {
        foreach (Point point in GetFigurePoints())
            graphics.FillRectangle(Brush,
                cellWidth * point.X,
                cellHeight * point.Y,
                cellWidth,
                cellHeight);
    }

    public void DrawFigureStructure(Graphics graphics, int cellWidth, int cellHeight)
    {
        foreach (Point point in StatesOfFigurePoints[0])
            graphics.FillRectangle(Brush,
                cellWidth * point.X,
                cellHeight * point.Y,
                cellWidth,
                cellHeight);
    }

    public void MoveDown()
    {
        y++;
    }

    public void MoveLeft()
    {
        x--;
    }

    public void MoveRight()
    {
        x++;
    }

    public void Rotate()
    {
        FigureRotationIndex = (FigureRotationIndex + 1) % StatesOfFigurePoints.Count;
    }

    private void FillOtherRotationState(int number)
    {
        for (int i = 1; i <= number; i++)
            StatesOfFigurePoints.Add(NextRotationStateOfFigure(StatesOfFigurePoints[i - 1]));
    }

    private Point[] NextRotationStateOfFigure(Point[] points)
    {
        return points
            .Select(point => new Point(
                ReferencePoint.X + (ReferencePoint.Y - point.Y),
                ReferencePoint.Y + (point.X - ReferencePoint.X)))
            .ToArray();
    }
}