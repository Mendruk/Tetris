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
            game.Draw(e.Graphics,pictureGameField.Width,pictureGameField.Height);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            game.Update();
            Refresh();
        }
    }
}