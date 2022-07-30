namespace Tetris;

public class FigurePart
{
    public int x;
    public int y;

    public FigurePart(int x, int y, Brush brush)
    {
        this.x = x;
        this.y = y;
        Brush = brush;
    }

    public Brush Brush { get; }
}