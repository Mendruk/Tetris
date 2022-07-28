namespace Tetris
{
    public partial class MainForm : Form
    {
        private Game game;
        public MainForm()
        {
            game = new Game();
            InitializeComponent();
        }

        private void pictureGameField_Paint(object sender, PaintEventArgs e)
        {
            game.DrawFigure(e.Graphics,pictureGameField.Width,pictureGameField.Height);
        }

        private void pictureNextFigure_Paint(object sender, PaintEventArgs e)
        {
            game.DrawNextFigure(e.Graphics, pictureGameField.Width, pictureGameField.Height);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            game.Update();
            Refresh();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                case Keys.Up:
                    game.RotateFigure();
                    break;
                case Keys.A:
                case Keys.Left:
                    game.MoveFigureHorizontal(-1);
                    break;
                case Keys.D:
                case Keys.Right:
                    game.MoveFigureHorizontal(1);
                    break;
                case Keys.Space:
                    game.PutDownFigure();
                    break;
            }
        }
    }
}