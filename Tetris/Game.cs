namespace Tetris;

public class Game
{
    private readonly Pen gridPen;
    private readonly int height = 20;
    private readonly int width = 10;
    private readonly Random random = new();
    private Figure currentFigure;
    private Figure nextFigure;
    private Part[,] partsOnBotton;
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
        currentFigure.MoveToCenterOfFieldWidth(width);
        nextFigure = new Figure(GetRandomFigureType());
        partsOnBotton = new Part[width, height];
    }

    public void Update()
    {
        if (isPause) return;
        if (!CheckBlockVertical())
        {
            currentFigure.MoveDown();
        }
        else
        {
            for (int i = 0; i < currentFigure.Length; i++)
            {
                if (partsOnBotton[currentFigure[i].X, currentFigure[i].Y] != null)
                {
                    ShowFailMessage();
                    return;
                }

                partsOnBotton[currentFigure[i].X, currentFigure[i].Y] = currentFigure[i];
            }

            List<int> linesToDelite = new List<int>();
            int poinCount = 0;

            for (int i = 0; i < currentFigure.Length; i++)
            {
                for (int x = 0; x < width; x++)
                    if (partsOnBotton[x, currentFigure[i].Y] != null)
                        poinCount++;

                if (poinCount == width)
                {
                    linesToDelite.Add(currentFigure[i].Y);
                    for (int x = 0; x < width; x++) partsOnBotton[x, currentFigure[i].Y] = null;
                }

                poinCount = 0;
            }

            foreach (int lineNumber in linesToDelite)
                for (int y = lineNumber; y > 0; y--)
                for (int x = 0; x < width; x++)
                {
                    partsOnBotton[x, y] = partsOnBotton[x, y - 1];
                    partsOnBotton[x, y - 1] = null;
                }

            if (linesToDelite.Count > 0) Score += CalculateScore(linesToDelite.Count);

            currentFigure = nextFigure;
            currentFigure.MoveToCenterOfFieldWidth(width);
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
            if (currentFigure[i].Y == height - 1 || partsOnBotton[currentFigure[i].X, currentFigure[i].Y + 1] != null)
                return true;
        return false;
    }

    private bool CheckBlockHorizontal(int direction)
    {
        for (int i = 0; i < currentFigure.Length; i++)
        {
            if (direction == 1 && (currentFigure[i].X == width - 2 + direction
                                   || partsOnBotton[currentFigure[i].X + direction, currentFigure[i].Y] != null))
                return true;
            if (direction == -1 && (currentFigure[i].X == 1 + direction
                                    || partsOnBotton[currentFigure[i].X + direction, currentFigure[i].Y] != null))
                return true;
        }

        return false;
    }

    public void RotateFigure()
    {
        if (!currentFigure.CanRotate) return;
        ;

        for (int i = 0; i < currentFigure.Length; i++)
        {
            int tempX = currentFigure.ReferencePoint.X + (currentFigure.ReferencePoint.Y - currentFigure[i].Y);
            int tempY = currentFigure.ReferencePoint.Y + (currentFigure[i].X - currentFigure.ReferencePoint.X);

            if (tempX < 0 || tempX >= width || tempY < 0 || tempY >= height || partsOnBotton[tempX, tempY] != null)
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
        while (!CheckBlockVertical()) currentFigure.MoveDown();
    }

    public void MoveFigureDown()
    {
        currentFigure.MoveDown();
    }

    private Type GetRandomFigureType()
    {
        int randomNumber = random.Next(Enum.GetNames(typeof(Type)).Length);

        switch (randomNumber)
        {
            case 0:
                return Type.O;
            case 1:
                return Type.J;
            case 2:
                return Type.L;
            case 3:
                return Type.S;
            case 4:
                return Type.Z;
            case 5:
                return Type.T;
            case 6:
                return Type.I;
            case 7:
            default:
                return Type.Dot;
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
            if (partsOnBotton[x, y] != null)
                graphics.FillRectangle(partsOnBotton[x, y].Brush, fieldWidth / width * x, fieldHeight / height * y,
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