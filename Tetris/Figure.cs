namespace Tetris;

public class Figure
{
    private readonly Dictionary<Type, Brush> brushes = new()
    {
        { Type.O, Brushes.DarkRed },
        { Type.J, Brushes.DarkOrange },
        { Type.L, Brushes.DarkGoldenrod },
        { Type.S, Brushes.DarkGreen },
        { Type.Z, Brushes.DarkCyan },
        { Type.T, Brushes.DarkBlue },
        { Type.I, Brushes.DarkViolet },
        { Type.Dot, Brushes.Gray }
    };

    private readonly Part[] parts;

    public Figure(Type type)
    {
        if (brushes.TryGetValue(type, out Brush? brush))

            switch (type)
            {
                case Type.O:
                    parts = new[]
                        { new(0, 0, brush), new Part(0, 1, brush), new Part(1, 0, brush), new Part(1, 1, brush) };
                    CanRotate = false;
                    break;
                case Type.J:
                    parts = new[]
                        { new(1, 1, brush), new Part(1, 0, brush), new Part(0, 2, brush), new Part(1, 2, brush) };
                    CanRotate = true;
                    break;
                case Type.L:
                    parts = new[]
                        { new(0, 1, brush), new Part(0, 0, brush), new Part(0, 2, brush), new Part(1, 2, brush) };
                    CanRotate = true;
                    break;
                case Type.S:
                    parts = new[]
                        { new(1, 1, brush), new Part(1, 0, brush), new Part(2, 0, brush), new Part(0, 1, brush) };
                    CanRotate = true;
                    break;
                case Type.Z:
                    parts = new[]
                        { new(1, 1, brush), new Part(0, 0, brush), new Part(1, 0, brush), new Part(2, 1, brush) };
                    CanRotate = true;
                    break;
                case Type.T:
                    parts = new[]
                        { new(1, 0, brush), new Part(0, 0, brush), new Part(2, 0, brush), new Part(1, 1, brush) };
                    CanRotate = true;
                    break;
                case Type.I:
                    parts = new[]
                        { new(1, 0, brush), new Part(0, 0, brush), new Part(2, 0, brush), new Part(3, 0, brush) };
                    CanRotate = true;
                    break;
                case Type.Dot:
                default:
                    parts = new Part[] { new(0, 0, brush) };
                    CanRotate = false;
                    break;
            }

        ReferencePoint = parts[0];
    }

    public bool CanRotate { get; }

    public int Length => parts.Length;

    public Part ReferencePoint { get; }

    public Part this[int index]
    {
        get => parts[index];
        set => parts[index] = value;
    }

    public void MoveToCenterOfFieldWidth(int width)
    {
        foreach (Part part in parts) part.X += width / 2 - 1;
    }

    public void MoveDown()
    {
        foreach (Part part in parts) part.Y++;
    }

    public void MoveHorizontal(int direction)
    {
        foreach (Part part in parts) part.X += direction;
    }

    public void Rotate()
    {
        if (!CanRotate) return;

        for (int i = 0; i < parts.Length; i++)
        {
            int tempX = ReferencePoint.X + (ReferencePoint.Y - parts[i].Y);
            parts[i].Y = ReferencePoint.Y + (parts[i].X - ReferencePoint.X);
            parts[i].X = tempX;
        }
    }

    public void Draw(Graphics graphics, int fieldWidth, int fieldHeight, int widthInCell, int heightInCell)
    {
        foreach (Part part in parts)
            graphics.FillRectangle(part.Brush, fieldWidth / widthInCell * part.X, fieldHeight / heightInCell * part.Y,
                fieldWidth / widthInCell, fieldHeight / heightInCell);
    }
}

public enum Type
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