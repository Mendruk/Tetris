namespace Tetris;

public class FigurePart
{
    private int x;
    private int y;

    public FigurePart(int x, int y, Brush brush)
    {
        this.x = x;
        this.y = y;
        Brush = brush;
    }

    public int X
    {
        get => x;
        set => x = value;
    }

    public int Y
    {
        get => y;
        set => y = value;
    }

    public Brush Brush { get; }
}