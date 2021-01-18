using System.Drawing;
using System.Windows.Forms;
using ChessGame;
using ChessGame.Pieces;

namespace ChessForms
{
    public partial class BoardDisplay : Form
    {
        public PictureBox[,] Boardcells;

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

            Chessboard Chessboard = new ChessGame.Gamemodes.ClassicChess().GenerateBoard(playerWhite, playerBlack);
            CreateBoard(Chessboard.Width, Chessboard.Height);

            Chessboard.onGameStateUpdated += Board_onGameStateUpdated;

            Chessboard.StartGame();
        }

        public void UpdateBoard(Chessboard board)
        {
            for (int y = 0; y < board.Height; y++)
            {
                for (int x = 0; x < board.Width; x++)
                {

                }
            }
        }

        public void CreateBoard(int width, int height)
        {
            tableLayoutPanel1.ColumnCount = width;
            tableLayoutPanel1.ColumnStyles.Clear();
            for (int i = 0; i < width; i++)
            {
                // set size to any percent, doesnt matter
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            }

            tableLayoutPanel1.RowCount = height;
            tableLayoutPanel1.RowStyles.Clear();
            for (int i = 0; i < height; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            }

            Boardcells = new PictureBox[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    PictureBox box = new PictureBox
                    {
                        Dock = DockStyle.Fill,
                        Image = Image.FromFile(@"C:\Users\kress\Documents\geotermi.jpg")
                    };

                    box.Click += CellClicked;
                    Boardcells[x, y] = box;

                    tableLayoutPanel1.Controls.Add(box);
                }
            }
        }

        private void CellClicked(object sender, System.EventArgs e)
        {
            
        }

        public void ClearPiece(int x, int y)
        {
            Boardcells[x, y].Image = null;
        }

        public void PlacePiece(int x, int y, Piece piece)
        {
            Boardcells[x, y].Image = GetPieceImage(piece);
        }

        private Image GetPieceImage(Piece piece)
        {
            switch (piece)
            {
                case Bishop _:
                    break;
                default:
                    break;
            }

            return null;
        }

        private void pictureBox1_Click(object sender, System.EventArgs e)
        {
            
        }
    }
}
