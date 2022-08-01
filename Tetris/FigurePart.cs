namespace Tetris;

public class FigurePart
{
    private readonly Figure figure;
    private int x;
    private int y;

    public FigurePart(Figure figure, int x, int y, Brush brush)
    {
        this.figure = figure;
        this.x = x;
        this.y = y;
        Brush = brush;
    }

    public int X
    {
        get => x + figure.shiftX;
        set => x = value - figure.shiftX;
    }

    public int Y
    {
        get => y + figure.shiftY;
        set => y = value - figure.shiftY;
    }

    public Brush Brush { get; }
}