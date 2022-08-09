namespace Tetris;

public class Game
{
    private readonly Random random = new();
    private readonly Pen gridPen;
    private readonly int height = 20;
    private readonly int width = 10;
    private Figure currentFigure;
    private Figure nextFigure;
    private List<Brush[]> partsOnBottom;
    private bool isPause = true;

    public Game()
    {
        gridPen = Pens.Black;
        Start();
    }

    public int Score { get; private set; }

    private void Start()
    {
        isPause = false;
        Score = 0;
        currentFigure = new Figure(GetRandomFigureType());
        currentFigure.MoveToMiddleOfWidth(width);
        nextFigure = new Figure(GetRandomFigureType());
        partsOnBottom = new List<Brush[]>();

        for (int i = 0; i < height; i++) partsOnBottom.Add(new Brush[width]);
    }

    public void Update()
    {
        if (!isPause)
            MoveFigureDown();
    }

    private bool CanMoveFigureDown()
    {
        foreach (FigurePart part in currentFigure.StatesOfFigureParts[currentFigure.FigureRotationIndex])
            if (part.Y + currentFigure.Y >= height - 1 ||
                partsOnBottom[part.Y + currentFigure.Y + 1][part.X + currentFigure.X] != null)
                return false;
        return true;
    }

    private bool CanMoveFigureLeft()
    {
        foreach (FigurePart part in currentFigure.StatesOfFigureParts[currentFigure.FigureRotationIndex])
            if (part.X + currentFigure.X == 0 ||
                partsOnBottom[part.Y + currentFigure.Y][part.X + currentFigure.X - 1] != null)
                return false;
        return true;
    }

    private bool CanMoveFigureRight()
    {
        foreach (FigurePart part in currentFigure.StatesOfFigureParts[currentFigure.FigureRotationIndex])
            if (part.X + currentFigure.X == width - 1 ||
                partsOnBottom[part.Y + currentFigure.Y][part.X + currentFigure.X + 1] != null)
                return false;
        return true;
    }

    private bool CanRotateFigure()
    {
        foreach (FigurePart part in currentFigure.NextRotationState())
        {
            if (part.X + currentFigure.X < 0 || part.X + currentFigure.X >= width || part.Y + currentFigure.Y < 0 ||
                part.Y + currentFigure.Y >= height ||
                partsOnBottom[part.Y + currentFigure.Y][part.X + currentFigure.X] != null)
                return false;
        }

        return true;
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
            currentFigure.MoveToMiddleOfWidth(width);
            nextFigure = new Figure(GetRandomFigureType());
        }
    }

    private void AddFigureToPartsOnBottom()
    {
        foreach (FigurePart part in currentFigure.StatesOfFigureParts[currentFigure.FigureRotationIndex])
        {
            if (partsOnBottom[part.Y + currentFigure.Y][part.X + currentFigure.X] != null)
            {
                ShowFailMessage();
                return;
            }

            partsOnBottom[part.Y + currentFigure.Y][part.X + currentFigure.X] = part.Brush;
        }
    }

    private void ClearFullLinesPartsOnBottom()
    {
        List<int> linesToDelete = new();
        foreach (FigurePart part in currentFigure.StatesOfFigureParts[currentFigure.FigureRotationIndex])
        {
            bool isFullLine = true;
            for (int i = 0; i < partsOnBottom[part.Y + currentFigure.Y].Length; i++)
                if (partsOnBottom[part.Y + currentFigure.Y][i] == null)
                    isFullLine = false;

            if (!linesToDelete.Contains(part.Y + currentFigure.Y) && isFullLine)
                linesToDelete.Add(part.Y + currentFigure.Y);
        }

        linesToDelete.Sort();
        foreach (int lineNumber in linesToDelete)
        {
            for (int i = 0; i < partsOnBottom[lineNumber].Length; i++) partsOnBottom[lineNumber][i] = null;
            partsOnBottom.Insert(0, partsOnBottom[lineNumber]);
            partsOnBottom.RemoveAt(lineNumber + 1);
        }

        if (linesToDelete.Count > 0)
            Score += CalculateScore(linesToDelete.Count);
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
        int randomNumber = random.Next(Enum.GetNames(typeof(FigureType)).Length);

        switch (randomNumber)
        {
            case 0:
                return FigureType.O;
            case 1:
                return FigureType.J;
            case 2:
                return FigureType.L;
            case 3:
                return FigureType.S;
            case 4:
                return FigureType.Z;
            case 5:
                return FigureType.T;
            case 6:
                return FigureType.I;
            case 7:
            default:
                return FigureType.Dot;
        }
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
        currentFigure.Draw(graphics, fieldWidth, fieldHeight, width, height);

        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            if (partsOnBottom[y][x] != null)
                graphics.FillRectangle(partsOnBottom[y][x], fieldWidth / width * x, fieldHeight / height * y,
                    fieldWidth / width, fieldHeight / height);
            graphics.DrawRectangle(gridPen, fieldWidth / width * x, fieldHeight / height * y,
                fieldWidth / width, fieldHeight / height);
        }
    }

    public void DrawNextFigure(Graphics graphics, int fieldWidth, int fieldHeight)
    {
        nextFigure.Draw(graphics, fieldWidth, fieldHeight, width, height);
    }
}