namespace Tetris;

public class Figure
{
    public static Dictionary<FigureType, Brush> brushes = new()
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

    private readonly FigurePart[] parts;
    private readonly Rotation rotationType;

    public Figure(FigureType figureType)
    {
        if (brushes.TryGetValue(figureType, out Brush? brush))

            switch (figureType)
            {
                case FigureType.O:
                    parts = new[]
                    {
                        new(0, 0, brush), new FigurePart(0, 1, brush), new FigurePart(1, 0, brush),
                        new FigurePart(1, 1, brush)
                    };
                    rotationType = Rotation.NotRoatting;
                    break;
                case FigureType.J:
                    parts = new[]
                    {
                        new(1, 1, brush), new FigurePart(1, 0, brush), new FigurePart(0, 2, brush),
                        new FigurePart(1, 2, brush)
                    };
                    rotationType = Rotation.Rotating;
                    break;
                case FigureType.L:
                    parts = new[]
                    {
                        new(0, 1, brush), new FigurePart(0, 0, brush), new FigurePart(0, 2, brush),
                        new FigurePart(1, 2, brush)
                    };
                    rotationType = Rotation.Rotating;
                    break;
                case FigureType.S:
                    parts = new[]
                    {
                        new(1, 1, brush), new FigurePart(1, 0, brush), new FigurePart(2, 0, brush),
                        new FigurePart(0, 1, brush)
                    };
                    rotationType = Rotation.TwoState;
                    break;
                case FigureType.Z:
                    parts = new[]
                    {
                        new(1, 1, brush), new FigurePart(0, 0, brush), new FigurePart(1, 0, brush),
                        new FigurePart(2, 1, brush)
                    };
                    rotationType = Rotation.TwoState;
                    break;
                case FigureType.T:
                    parts = new[]
                    {
                        new(1, 0, brush), new FigurePart(0, 0, brush), new FigurePart(2, 0, brush),
                        new FigurePart(1, 1, brush)
                    };
                    rotationType = Rotation.Rotating;
                    break;
                case FigureType.I:
                    parts = new[]
                    {
                        new(1, 0, brush), new FigurePart(0, 0, brush), new FigurePart(2, 0, brush),
                        new FigurePart(3, 0, brush)
                    };
                    rotationType = Rotation.TwoState;
                    break;
                case FigureType.Dot:
                default:
                    parts = new FigurePart[] { new(0, 0, brush) };
                    rotationType = Rotation.NotRoatting;
                    break;
            }

        ReferencePoint = parts[0];
    }

    public bool IsRotate { get; private set; }

    public int Length => parts.Length;

    public FigurePart ReferencePoint { get; }

    public FigurePart this[int index]
    {
        get => parts[index];
        set => parts[index] = value;
    }

    public void MoveToCenterOfFieldWidth(int width)
    {
        foreach (FigurePart part in parts) part.x += width / 2 - 1;
    }

    public void MoveDown()
    {
        foreach (FigurePart part in parts) 
            part.y++;
    }

    public void MoveHorizontal(int direction)
    {
        foreach (FigurePart part in parts)
            part.x += direction;
    }

    public void Rotate()
    {
        switch (rotationType)
        {
            case Rotation.Rotating:
                RotateClockwise();
                break;
                ;
            case Rotation.TwoState:
                if (IsRotate)
                {
                    RotateClockwise();
                    IsRotate = false;
                }
                else
                {
                    RotateСounterСlockwise();
                    IsRotate = true;
                }

                break;
            case Rotation.NotRoatting:
            default:
                break;
        }
    }

    private void RotateClockwise()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            int rotatedFigurePartX = ReferencePoint.x + (ReferencePoint.y - parts[i].y);
            parts[i].y = ReferencePoint.y + (parts[i].x - ReferencePoint.x);
            parts[i].x = rotatedFigurePartX;
        }
    }

    private void RotateСounterСlockwise()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            int rotatedFigurePartX = ReferencePoint.x - (ReferencePoint.y - parts[i].y);
            parts[i].y = ReferencePoint.y - (parts[i].x - ReferencePoint.x);
            parts[i].x = rotatedFigurePartX;
        }
    }

    public void Draw(Graphics graphics, int fieldWidth, int fieldHeight, int widthInCell, int heightInCell)
    {
        foreach (FigurePart part in parts)
            graphics.FillRectangle(part.Brush, fieldWidth / widthInCell * part.x, fieldHeight / heightInCell * part.y,
                fieldWidth / widthInCell, fieldHeight / heightInCell);
    }
}

public enum FigureType
{
    O,
    J,
    L,
    S,
    Z,
    T,
    I,
    Dot
}

public enum Rotation
{
    Rotating,
    NotRoatting,
    TwoState
}