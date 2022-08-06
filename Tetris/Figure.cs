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

    private readonly Rotation rotationType;

    public Figure(FigureType figureType)
    {
        if (brushes.TryGetValue(figureType, out Brush? brush))

            switch (figureType)
            {
                case FigureType.O:
                    Parts = new[]
                    {
                        new(0, 0, brush), new(0, 1, brush), new(1, 0, brush),
                        new FigurePart(1, 1, brush)
                    };
                    rotationType = Rotation.NotRotating;
                    break;
                case FigureType.J:
                    Parts = new[]
                    {
                        new(1, 1, brush), new(1, 0, brush), new(0, 2, brush),
                        new FigurePart(1, 2, brush)
                    };
                    rotationType = Rotation.Rotating;
                    break;
                case FigureType.L:
                    Parts = new[]
                    {
                        new(0, 1, brush), new(0, 0, brush), new(0, 2, brush),
                        new FigurePart(1, 2, brush)
                    };
                    rotationType = Rotation.Rotating;
                    break;
                case FigureType.S:
                    Parts = new[]
                    {
                        new(1, 1, brush), new(1, 0, brush), new(2, 0, brush),
                        new FigurePart(0, 1, brush)
                    };
                    rotationType = Rotation.TwoStateOfTurn;
                    break;
                case FigureType.Z:
                    Parts = new[]
                    {
                        new(1, 1, brush), new(0, 0, brush), new(1, 0, brush),
                        new FigurePart(2, 1, brush)
                    };
                    rotationType = Rotation.TwoStateOfTurn;
                    break;
                case FigureType.T:
                    Parts = new[]
                    {
                        new(1, 0, brush), new (0, 0, brush), new (2, 0, brush),
                        new FigurePart(1, 1, brush)
                    };
                    rotationType = Rotation.Rotating;
                    break;
                case FigureType.I:
                    Parts = new[]
                    {
                        new(1, 0, brush), new (0, 0, brush), new (2, 0, brush),
                        new FigurePart(3, 0, brush)
                    };
                    rotationType = Rotation.TwoStateOfTurn;
                    break;
                case FigureType.Dot:
                default:
                    Parts = new FigurePart[] { new(0, 0, brush) };
                    rotationType = Rotation.NotRotating;
                    break;
            }

        ReferencePoint = Parts[0];
    }

    public bool IsTurned { get; private set; }

    public FigurePart ReferencePoint { get; }

    public FigurePart[] Parts { get; }

    public int X { get; set; }

    public int Y { get; set; }

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
        switch (rotationType)
        {
            case Rotation.Rotating:
                RotateClockwise();
                break;
            case Rotation.TwoStateOfTurn:
                if (IsTurned)
                {
                    RotateСounterСlockwise();
                    IsTurned = false;
                }
                else
                {
                    RotateClockwise();
                    IsTurned = true;
                }

                break;
            case Rotation.NotRotating:
            default:
                break;
        }
    }

    private void RotateClockwise()
    {
        foreach (FigurePart part in Parts)
        {
            int rotatedFigurePartX = ReferencePoint.X + (ReferencePoint.Y - part.Y);
            part.Y = ReferencePoint.Y + (part.X - ReferencePoint.X);
            part.X = rotatedFigurePartX;
        }
    }

    private void RotateСounterСlockwise()
    {
        foreach (FigurePart part in Parts)
        {
            int rotatedFigurePartX = ReferencePoint.X - (ReferencePoint.Y - part.Y);
            part.Y = ReferencePoint.Y - (part.X - ReferencePoint.X);
            part.X = rotatedFigurePartX;
        }
    }

    public void Draw(Graphics graphics, int fieldWidth, int fieldHeight, int widthInCell, int heightInCell)
    {
        foreach (FigurePart part in Parts)
            graphics.FillRectangle(part.Brush, fieldWidth / widthInCell * (part.X + X),
                fieldHeight / heightInCell * (part.Y + Y),
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
    NotRotating,
    TwoStateOfTurn
}