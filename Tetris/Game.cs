namespace Tetris;

public class Game
{
    private readonly Pen gridPen;
    private readonly int height = 20;
    private readonly Random random = new();
    private readonly int width = 10;
    public Figure currentFigure;
    private bool isPause = true;
    private Figure nextFigure;
    private Brush[,] partsOnBottom;

    public Game()
    {
        gridPen = Pens.Black;
        Start();
    }

    public int Score { get; private set; }

    public void Start()
    {
        isPause = false;
        Score = 0;
        currentFigure = new Figure(GetRandomFigureType());
        currentFigure.shiftX = width / 2 - 1;
        nextFigure = new Figure(GetRandomFigureType());
        partsOnBottom = new Brush[width, height];
    }

    public void Update()
    {
        if (isPause)
            return;
        if (!CheckBlockVertical())
        {
            MoveFigureDown();
        }
        else
        {
            for (int i = 0; i < currentFigure.Length; i++)
            {
                if (partsOnBottom[currentFigure[i].X, currentFigure[i].Y] != null)
                {
                    ShowFailMessage();
                    return;
                }

                partsOnBottom[currentFigure[i].X, currentFigure[i].Y] = currentFigure[i].Brush;
            }

            List<int> linesToDelete = new();
            int pointCount = 0;

            for (int i = 0; i < currentFigure.Length; i++)
            {
                for (int x = 0; x < width; x++)
                    if (partsOnBottom[x, currentFigure[i].Y] != null)
                        pointCount++;

                if (pointCount == width)
                {
                    linesToDelete.Add(currentFigure[i].Y);
                    for (int x = 0; x < width; x++)
                        partsOnBottom[x, currentFigure[i].Y] = null;
                }

                pointCount = 0;
            }

            foreach (int lineNumber in linesToDelete)
                for (int y = lineNumber; y > 0; y--)
                for (int x = 0; x < width; x++)
                {
                    partsOnBottom[x, y] = partsOnBottom[x, y - 1];
                    partsOnBottom[x, y - 1] = null;
                }

            if (linesToDelete.Count > 0)
                Score += CalculateScore(linesToDelete.Count);

            currentFigure = nextFigure;
            currentFigure.shiftX = width / 2 - 1;
            nextFigure = new Figure(GetRandomFigureType());
        }
    }

    private int CalculateScore(int line)
    {
        if (line > 0)
            return (int)Math.Pow(2, line) * 100 - 100;
        return 0;
    }

    private bool CheckBlockVertical()
    {
        for (int i = 0; i < currentFigure.Length; i++)
            if (currentFigure[i].Y >= height - 1
                || partsOnBottom[currentFigure[i].X, currentFigure[i].Y + 1] != null)
                return true;
        return false;
    }

    private bool CheckBlockHorizontal(int direction)
    {
        for (int i = 0; i < currentFigure.Length; i++)
        {
            if (direction == 1 && (currentFigure[i].X == width - 2 + direction
                                   || partsOnBottom[currentFigure[i].X + direction, currentFigure[i].Y] != null))
                return true;
            if (direction == -1 && (currentFigure[i].X == 1 + direction
                                    || partsOnBottom[currentFigure[i].X + direction, currentFigure[i].Y] != null))
                return true;
        }

        return false;
    }

    public void RotateFigure()
    {
        if (currentFigure.IsRotate)
            for (int i = 0; i < currentFigure.Length; i++)
            {
                int rotatedFigurePartX = currentFigure.ReferencePoint.X +
                                         (currentFigure.ReferencePoint.Y - currentFigure[i].Y);
                int rotatedFigurePartY = currentFigure.ReferencePoint.Y +
                                         (currentFigure[i].X - currentFigure.ReferencePoint.X);

                if (rotatedFigurePartX < 0 || rotatedFigurePartX >= width || rotatedFigurePartY < 0 ||
                    rotatedFigurePartY >= height || partsOnBottom[rotatedFigurePartX, rotatedFigurePartY] != null)
                    return;
            }
        else
            for (int i = 0; i < currentFigure.Length; i++)
            {
                int rotatedFigurePartX = currentFigure.ReferencePoint.X -
                                         (currentFigure.ReferencePoint.Y - currentFigure[i].Y);
                int rotatedFigurePartY = currentFigure.ReferencePoint.Y -
                                         (currentFigure[i].X - currentFigure.ReferencePoint.X);

                if (rotatedFigurePartX < 0 || rotatedFigurePartX >= width || rotatedFigurePartY < 0 ||
                    rotatedFigurePartY >= height || partsOnBottom[rotatedFigurePartX, rotatedFigurePartY] != null)
                    return;
            }

        currentFigure.Rotate();
    }

    public void MoveFigureHorizontal(int direction)
    {
        if (!CheckBlockHorizontal(direction))
            currentFigure.MoveHorizontal(direction);
    }

    public void PutDownFigure()
    {
        while (!CheckBlockVertical())
            currentFigure.MoveDown();
    }

    public void MoveFigureDown()
    {
        currentFigure.MoveDown();
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
        if (result == DialogResult.Yes) Start();
        if (result == DialogResult.No) ShowFailMessage();
    }

    public void DrawGameField(Graphics graphics, int fieldWidth, int fieldHeight)
    {
        currentFigure.Draw(graphics, fieldWidth, fieldHeight, width, height);

        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            if (partsOnBottom[x, y] != null)
                graphics.FillRectangle(partsOnBottom[x, y], fieldWidth / width * x, fieldHeight / height * y,
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