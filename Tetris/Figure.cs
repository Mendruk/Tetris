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
    private FigureType figureType;
    public int shiftX;
    public int shiftY;

    public Figure(FigureType figureType)
    {
        this.figureType = figureType;
        if (brushes.TryGetValue(figureType, out Brush? brush))

            switch (figureType)
            {
                case FigureType.O:
                    parts = new[]
                    {
                        new(this, 0, 0, brush), new FigurePart(this, 0, 1, brush), new FigurePart(this, 1, 0, brush),
                        new FigurePart(this, 1, 1, brush)
                    };
                    rotationType = Rotation.NotRoatting;
                    break;
                case FigureType.J:
                    parts = new[]
                    {
                        new(this, 1, 1, brush), new FigurePart(this, 1, 0, brush), new FigurePart(this, 0, 2, brush),
                        new FigurePart(this, 1, 2, brush)
                    };
                    rotationType = Rotation.Rotating;
                    break;
                case FigureType.L:
                    parts = new[]
                    {
                        new(this, 0, 1, brush), new FigurePart(this, 0, 0, brush), new FigurePart(this, 0, 2, brush),
                        new FigurePart(this, 1, 2, brush)
                    };
                    rotationType = Rotation.Rotating;
                    break;
                case FigureType.S:
                    parts = new[]
                    {
                        new(this, 1, 1, brush), new FigurePart(this, 1, 0, brush), new FigurePart(this, 2, 0, brush),
                        new FigurePart(this, 0, 1, brush)
                    };
                    rotationType = Rotation.TwoState;
                    break;
                case FigureType.Z:
                    parts = new[]
                    {
                        new(this, 1, 1, brush), new FigurePart(this, 0, 0, brush), new FigurePart(this, 1, 0, brush),
                        new FigurePart(this, 2, 1, brush)
                    };
                    rotationType = Rotation.TwoState;
                    break;
                case FigureType.T:
                    parts = new[]
                    {
                        new(this, 1, 0, brush), new FigurePart(this, 0, 0, brush), new FigurePart(this, 2, 0, brush),
                        new FigurePart(this, 1, 1, brush)
                    };
                    rotationType = Rotation.Rotating;
                    break;
                case FigureType.I:
                    parts = new[]
                    {
                        new(this, 1, 0, brush), new FigurePart(this, 0, 0, brush), new FigurePart(this, 2, 0, brush),
                        new FigurePart(this, 3, 0, brush)
                    };
                    rotationType = Rotation.TwoState;
                    break;
                case FigureType.Dot:
                default:
                    parts = new FigurePart[] { new(this, 0, 0, brush) };
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

    public void MoveDown()
    {
        shiftY++;
    }

    public void MoveHorizontal(int direction)
    {
        shiftX += direction;
    }

    public void Rotate()
    {
        switch (rotationType)
        {
            case Rotation.Rotating:
                RotateClockwise();
                break;
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
            int rotatedFigurePartX = ReferencePoint.X + (ReferencePoint.Y - parts[i].Y);
            parts[i].Y = ReferencePoint.Y + (parts[i].X - ReferencePoint.X);
            parts[i].X = rotatedFigurePartX;
        }
    }

    private void RotateСounterСlockwise()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            int rotatedFigurePartX = ReferencePoint.X - (ReferencePoint.Y - parts[i].Y);
            parts[i].Y = ReferencePoint.Y - (parts[i].X - ReferencePoint.X);
            parts[i].X = rotatedFigurePartX;
        }
    }

    public void Draw(Graphics graphics, int fieldWidth, int fieldHeight, int widthInCell, int heightInCell)
    {
        foreach (FigurePart part in parts)
            graphics.FillRectangle(part.Brush, fieldWidth / widthInCell * part.X,
                fieldHeight / heightInCell * part.Y,
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