using System.Windows.Forms;
using ChessGame;

namespace ChessForms
{
    public partial class BoardDisplay : Form
    {
        public Chessboard Chessboard;

        public BoardDisplay()
        {
            InitializeComponent();
        }

        private void Board_onGameStateUpdated(Chessboard.GameState obj)
        {

        }

        private void BoardDisplay_Load(object sender, System.EventArgs e)
        {
            Player playerWhite = new Player("white");
            Player playerBlack = new Player("black");

            Chessboard = new Chessboard(8, 8, new ChessGame.Gamemodes.ClassicChess(), playerWhite, playerBlack);
            CreateBoard(8, 8);

            Chessboard.onGameStateUpdated += Board_onGameStateUpdated;

            Chessboard.StartGame();
        }

        public void CreateBoard(int width, int height)
        {
            tableLayoutPanel1.ColumnCount = width;
            tableLayoutPanel1.RowCount = height;
        }

        public void ClearPiece()
        {

        }

        public void PlacePiece()
        {

        }
    }
}
