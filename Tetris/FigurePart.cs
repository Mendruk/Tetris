namespace Tetris;

public class FigurePart
{
    public FigurePart(int x, int y, Brush brush)
    {
        X = x;
        Y = y;
        Brush = brush;
    }

    public int X { get; set; }

    public int Y { get; set; }

    public Brush Brush { get; }
}