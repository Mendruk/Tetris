namespace Tetris;

public class Game
{
    private readonly Random random = new();
    private readonly Pen gridPen = Pens.Black;
    private readonly Brush emptyCellBrush = Brushes.Azure;
    private readonly int height = 20;
    private readonly int width = 10;
    private Figure currentFigure;
    private Figure nextFigure;
    private List<Brush[]> partsOnBottom;
    private bool isPause = true;

    public Game()
    {
        Start();
    }

    public int Score { get; private set; }

    private void Start()
    {

        Score = 0;
        currentFigure = new Figure(GetRandomFigureType());
        currentFigure.ReduceHorizontally(width / 2 - 1);
        nextFigure = new Figure(GetRandomFigureType());

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
            .All(part => (part.X >= 0 &&
                              part.X < width &&
                              part.Y >= 0 &&
                              part.Y < height &&
                              partsOnBottom[part.Y][part.X] == emptyCellBrush));
    }

    public void RotateFigure()
    {
        if (CanRotateFigure())
        {
            currentFigure.Rotate();
        }
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

            ClearFullLinesPartsOnBottom();

            currentFigure = nextFigure;
            currentFigure.ReduceHorizontally(width / 2 - 1);
            nextFigure = new Figure(GetRandomFigureType());
        }
    }

    private void AddFigureToPartsOnBottom()
    {
        foreach (Point part in currentFigure.GetFigurePoints())
        {
            if (partsOnBottom[part.Y][part.X] != emptyCellBrush)
            {
                ShowFailMessage();
                return;
            }
            partsOnBottom[part.Y][part.X] = currentFigure.Brush;
        }
    }

    private void ClearFullLinesPartsOnBottom()
    {
        int[] linesToDelete = this.currentFigure
            .GetFigurePoints()
            .Select(point => point.Y)
            .Distinct()
            .Where(y => this.partsOnBottom[y].All(brush => brush != emptyCellBrush))
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

    private FigureType GetRandomFigureType()
    {
        return (FigureType)random.Next(Enum.GetNames(typeof(FigureType)).Length);
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

    public void DrawGameField(Graphics graphics, int fieldWidth, int fieldHeight)
    {
        int cellWidth = fieldWidth / width;
        int cellHeight = fieldHeight / height;

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                graphics.FillRectangle(partsOnBottom[y][x],
                    cellWidth * x,
                    cellHeight * y,
                    cellWidth,
                    cellHeight);
                graphics.DrawRectangle(gridPen,
                    cellWidth * x,
                    cellHeight * y,
                    cellWidth,
                    cellHeight);
            }
        currentFigure.DrawFigure(graphics, cellWidth, cellHeight);
    }

    public void DrawNextFigure(Graphics graphics, int fieldWidth, int fieldHeight)
    {
        int cellWidth = fieldWidth / width;
        int cellHeight = fieldHeight / height;

        nextFigure.DrawFigureStructure(graphics, cellWidth, cellHeight);
    }
}