using Timer = System.Threading.Timer;

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
            game.DrawGameField(e.Graphics,pictureGameField.Width,pictureGameField.Height);
        }

        private void pictureNextFigure_Paint(object sender, PaintEventArgs e)
        {
            game.DrawNextFigure(e.Graphics, pictureGameField.Width, pictureGameField.Height);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            game.Update();
            labelScoreCount.Text = game.Score.ToString();
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
                case Keys.S:
                case Keys.Down:
                    game.MoveFigureDown();
                    break;
                case Keys.A:
                case Keys.Left:
                    game.MoveFigureLeft();
                    break;
                case Keys.D:
                case Keys.Right:
                    game.MoveFigureRight();
                    break;
                case Keys.Space:
                    game.PutDownFigure();
                    break;
            }
            Refresh();
        }
    }
}