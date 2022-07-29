namespace Tetris;

public class Part
{
    public int X;
    public int Y;

    public Part(int X, int Y, Brush brush)
    {
        this.X = X;
        this.Y = Y;
        Brush = brush;
    }

    public Brush Brush { get; }
}