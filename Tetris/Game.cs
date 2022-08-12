namespace Tetris;

public class Game
{
    private readonly Brush emptyCellBrush = Brushes.Azure;
    private readonly Pen gridPen = Pens.Black;
    private readonly int height = 20;
    private readonly int width = 10;
    private Figure currentFigure;
    private Figure nextFigure;
    private List<Brush[]> partsOnBottom;
    private bool isPause = true;

    public Game()
    {
        Start();
        currentFigure = new Figure(FigureType.T);
    }

    public int Score { get; private set; }

    private void Start()
    {
        Score = 0;
        currentFigure = Figure.GetRandomFigure();
        currentFigure.ReduceHorizontally(width / 2 - 1);
        nextFigure = Figure.GetRandomFigure();

        partsOnBottom = new List<Brush[]>();

        for (int i = 0; i < height; i++)
        {
            partsOnBottom.Add(new Brush[width]);
            Array.Fill(partsOnBottom[i], emptyCellBrush);
        }

        isPause = false;
    }

    public void Update()
    {
        if (!isPause)
            MoveFigureDown();
    }

    public void DrawGameField(Graphics graphics, int fieldWidth, int fieldHeight)
    {
        int cellWidth = fieldWidth / width;
        int cellHeight = fieldHeight / height;

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                graphics.FillRectangle(partsOnBottom[y][x],
                    cellWidth * x,
                    cellHeight * y,
                    cellWidth,
                    cellHeight);
        currentFigure.DrawFigure(graphics, cellWidth, cellHeight);

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                graphics.DrawRectangle(gridPen,
                    cellWidth * x,
                    cellHeight * y,
                    cellWidth,
                    cellHeight);
    }

    public void DrawNextFigure(Graphics graphics, int fieldWidth, int fieldHeight)
    {
        int cellWidth = fieldWidth / width;
        int cellHeight = fieldHeight / height;

        nextFigure.DrawFigureStructure(graphics, cellWidth, cellHeight);
    }

    public void RotateFigure()
    {
        if (CanRotateFigure()) currentFigure.Rotate();
    }

    public void MoveFigureLeft()
    {
        if (CanMoveFigureLeft())
            currentFigure.MoveLeft();
    }

    public void MoveFigureRight()
    {
        if (CanMoveFigureRight())
            currentFigure.MoveRight();
    }

    public void MoveFigureDown()
    {
        if (CanMoveFigureDown())
        {
            currentFigure.MoveDown();
        }
        else
        {
            AddFigureToPartsOnBottom();

            if (IsDefeat()) ShowFailMessage();

            ClearFullLinesPartsOnBottom();

            currentFigure.ZeroingCoordinatesFigure();
            currentFigure = nextFigure;
            currentFigure.ReduceHorizontally(width / 2 - 1);
            nextFigure = Figure.GetRandomFigure();
        }
    }

    private bool CanMoveFigureDown()
    {
        return currentFigure.GetFigurePoints()
            .All(part => part.Y + 1 < height &&
                         partsOnBottom[part.Y + 1][part.X] == emptyCellBrush);
    }

    private bool CanMoveFigureLeft()
    {
        return currentFigure.GetFigurePoints()
            .All(part => part.X > 0 &&
                         partsOnBottom[part.Y][part.X - 1] == emptyCellBrush);
    }

    private bool CanMoveFigureRight()
    {
        return currentFigure.GetFigurePoints()
            .All(part => part.X < width - 1 &&
                         partsOnBottom[part.Y][part.X + 1] == emptyCellBrush);
    }

    private bool CanRotateFigure()
    {
        return currentFigure.GetNextRotationFigurePoints()
            .All(part => part.X >= 0 &&
                         part.X < width &&
                         part.Y >= 0 &&
                         part.Y < height &&
                         partsOnBottom[part.Y][part.X] == emptyCellBrush);
    }

    private void AddFigureToPartsOnBottom()
    {
        foreach (Point point in currentFigure.GetFigurePoints()) partsOnBottom[point.Y][point.X] = currentFigure.Brush;
    }

    private bool IsDefeat()
    {
        return currentFigure.GetFigurePoints()
            .Any(part => part.Y == 0);
    }

    private void ClearFullLinesPartsOnBottom()
    {
        int[] linesToDelete = currentFigure
            .GetFigurePoints()
            .Select(point => point.Y)
            .Distinct()
            .Where(y => partsOnBottom[y].All(brush => brush != emptyCellBrush))
            .OrderBy(y => y)
            .ToArray();

        foreach (int lineNumber in linesToDelete)
        {
            for (int i = 0; i < partsOnBottom[lineNumber].Length; i++)
                partsOnBottom[lineNumber][i] = emptyCellBrush;

            partsOnBottom.Insert(0, partsOnBottom[lineNumber]);
            partsOnBottom.RemoveAt(lineNumber + 1);
        }

        if (linesToDelete.Length > 0)
            Score += CalculateScore(linesToDelete.Length);
    }

    private int CalculateScore(int linesCount)
    {
        if (linesCount > 0)
            return (int)Math.Pow(2, linesCount) * 100 - 100;
        return 0;
    }

    public void PutDownFigure()
    {
        while (CanMoveFigureDown())
            MoveFigureDown();
        MoveFigureDown();
    }

    private void ShowFailMessage()
    {
        isPause = true;
        DialogResult result = MessageBox.Show("Game Over! Restart?", "Game Over", MessageBoxButtons.YesNo);
        if (result == DialogResult.Yes)
            Start();
        if (result == DialogResult.No)
            ShowFailMessage();
    }


}