using System.Windows.Forms;
using ChessGame;

namespace ChessForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            Player playerWhite = new Player("white");
            Player playerBlack = new Player("black");

            Chessboard board = new Chessboard(8, 8, new ChessGame.Gamemodes.ClassicChess(), playerWhite, playerBlack);
            board.onGameStateUpdated += Board_onGameStateUpdated;

            board.StartGame();
        }

        private void Board_onGameStateUpdated(Chessboard.GameState obj)
        {

        }
    }
}
