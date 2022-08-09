namespace Tetris;

public class Figure
{
    private static Dictionary<FigureType, Brush> brushes = new()
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

    private Rotation rotationType;

    public Figure(FigureType figureType)
    {
        StatesOfFigureParts = new List<FigurePart[]>();
        if (brushes.TryGetValue(figureType, out Brush? brush))
            switch (figureType)
            {
                case FigureType.O:
                    StatesOfFigureParts.Add(new FigurePart[]
                    {
                        new(0, 0, brush), new(0, 1, brush), new(1, 0, brush), new(1, 1, brush)
                    });
                    rotationType = Rotation.NotRotating;
                    break;
                case FigureType.J:
                    StatesOfFigureParts.Add(new FigurePart[]
                    {
                        new(1, 1, brush), new(1, 0, brush), new(0, 2, brush), new(1, 2, brush)
                    });
                    rotationType = Rotation.Rotating;
                    break;
                case FigureType.L:
                    StatesOfFigureParts.Add(new FigurePart[]
                    {
                        new(0, 1, brush), new(0, 0, brush), new(0, 2, brush), new(1, 2, brush)
                    });
                    rotationType = Rotation.Rotating;
                    break;
                case FigureType.S:
                    StatesOfFigureParts.Add(new FigurePart[]
                    {
                        new(1, 1, brush), new(1, 0, brush), new(2, 0, brush), new(0, 1, brush)
                    });
                    rotationType = Rotation.TwoStateOfTurn;
                    break;
                case FigureType.Z:
                    StatesOfFigureParts.Add(new FigurePart[]
                    {
                        new(1, 1, brush), new(0, 0, brush), new(1, 0, brush), new(2, 1, brush)
                    });
                    rotationType = Rotation.TwoStateOfTurn;
                    break;
                case FigureType.T:
                    StatesOfFigureParts.Add(new FigurePart[]
                    {
                        new(1, 0, brush), new(0, 0, brush), new(2, 0, brush), new(1, 1, brush)
                    });
                    rotationType = Rotation.Rotating;
                    break;
                case FigureType.I:
                    StatesOfFigureParts.Add(new FigurePart[]
                    {
                        new(1, 0, brush), new(0, 0, brush), new(2, 0, brush), new(3, 0, brush)
                    });
                    rotationType = Rotation.TwoStateOfTurn;
                    break;
                case FigureType.Dot:
                default:
                    StatesOfFigureParts.Add(new FigurePart[] { new(0, 0, brush) });
                    rotationType = Rotation.NotRotating;
                    break;
            }

        ReferencePoint = StatesOfFigureParts[0][0];

        switch (rotationType)
        {
            case Rotation.Rotating:
                FillAllRotationState(3);
                break;
            case Rotation.TwoStateOfTurn:
                FillAllRotationState(1);
                break;
            case Rotation.NotRotating:
            default:
                break;
        }
    }

    public FigurePart ReferencePoint { get; }

    public List<FigurePart[]> StatesOfFigureParts { get; set; }

    public int FigureRotationIndex { get; private set; }

    public int X { get; private set; }

    public int Y { get; private set; }

    public void MoveToMiddleOfWidth(int width)
    {
        X = width / 2 - 1;
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

    public FigurePart[] NextRotationState()
    {
        return StatesOfFigureParts[(FigureRotationIndex + 1) % StatesOfFigureParts.Count];
    }

    private void FillAllRotationState(int number)
    {
        for (int i = 1; i <= number; i++)
        {
            StatesOfFigureParts.Add(NextRorationStateOfFigure(StatesOfFigureParts[i - 1]));
        }
    }

    public void Rotate()
    {
        FigureRotationIndex = (FigureRotationIndex + 1) % StatesOfFigureParts.Count;
    }

    private FigurePart[] NextRorationStateOfFigure(FigurePart[] parts)
    {
        FigurePart[] rotatedFigureParts = new FigurePart[parts.Length];
        for (int i = 0; i < parts.Length; i++)
        {
            int rotatedFigurePartX = ReferencePoint.X + (ReferencePoint.Y - parts[i].Y);
            int rotatedFigurePartY = ReferencePoint.Y + (parts[i].X - ReferencePoint.X);
            rotatedFigureParts[i] = new FigurePart(rotatedFigurePartX, rotatedFigurePartY, parts[i].Brush);
        }

        return rotatedFigureParts;
    }

    public void Draw(Graphics graphics, int fieldWidth, int fieldHeight, int widthInCell, int heightInCell)
    {
        foreach (FigurePart part in StatesOfFigureParts[FigureRotationIndex])
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