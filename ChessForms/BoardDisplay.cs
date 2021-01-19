using System.Drawing;
using System.Windows.Forms;
using ChessGame;
using ChessGame.Pieces;

namespace ChessForms
{
    public partial class BoardDisplay : Form
    {
        public PictureBox[,] Boardcells;
        public int BoardWidth, BoardHeight;
        public Coordinate? FromPosition = null;
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
            playerWhite.onTurnStarted += PlayerWhite_onTurnStarted;

            ChessGame.Bots.SimpletronBot bot = new ChessGame.Bots.SimpletronBot();

            Chessboard = new ChessGame.Gamemodes.ClassicChess().GenerateBoard(playerWhite, bot.GeneratePlayer());
            CreateBoard(Chessboard.Width, Chessboard.Height);
            BoardWidth = Chessboard.Width;
            BoardHeight = Chessboard.Height;

            UpdateBoard();

            Chessboard.onGameStateUpdated += Board_onGameStateUpdated;

            Chessboard.StartGame();
        }

        private void PlayerWhite_onTurnStarted(Chessboard obj)
        {
            UpdateBoard();
        }

        public void UpdateBoard()
        {
            for (int y = 0; y < Chessboard.Height; y++)
            {
                for (int x = 0; x < Chessboard.Width; x++)
                {
                    Piece cellPiece = Chessboard.GetPiece(new Coordinate(x, y));

                    if (cellPiece is null)
                    {
                        ClearPiece(x, y);
                    }
                    else
                    {
                        PlacePiece(x, y, cellPiece);
                    }
                }
            }
        }

        public void CreateBoard(int width, int height)
        {
            tableLayoutPanel1.ColumnCount = width;
            tableLayoutPanel1.ColumnStyles.Clear();
            int i;
            for (i = 0; i < width; i++)
            {
                // set size to any percent, doesnt matter
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            }

            tableLayoutPanel1.RowCount = height;
            tableLayoutPanel1.RowStyles.Clear();
            for (i = 0; i < height; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            }

            Boardcells = new PictureBox[width, height];

            i = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color backgroundColor;
                    if (++i % 2 == 0)
                    {
                        backgroundColor = Color.White;
                    }
                    else
                    {
                        backgroundColor = Color.CornflowerBlue;
                    }

                    PictureBox box = new PictureBox
                    {
                        Dock = DockStyle.Fill,
                        BorderStyle = BorderStyle.None,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        BackColor = backgroundColor
                    };

                    box.Click += CellClicked;
                    Boardcells[x, y] = box;

                    tableLayoutPanel1.Controls.Add(box);
                }

                i++;
            }
        }

        private void MakeMove(Coordinate from, Coordinate to)
        {
            if (Chessboard.PerformMove(from.ToString() + to.ToString(), MoveNotation.UCI))
            {
                UpdateBoard();
                //Image image = Boardcells[from.File, from.Rank].Image;
                //Boardcells[to.File, to.Rank].Image = image;
                //Boardcells[from.File, from.Rank].Image = null;
            }
        }

        private void CellClicked(object sender, System.EventArgs e)
        {
            Point click = tableLayoutPanel1.PointToClient(MousePosition);
            int windowX = click.X;
            int windowY = click.Y;
            
            int cellX = windowX / (tableLayoutPanel1.Width / BoardWidth);
            int cellY = windowY / (tableLayoutPanel1.Height / BoardHeight);

            Coordinate clickTarget = new Coordinate(cellX, cellY);

            if (FromPosition is null)
            {
                FromPosition = clickTarget;
                this.Text = FromPosition.ToString();

                Boardcells[cellX, cellY].BorderStyle = BorderStyle.FixedSingle;
            }
            else
            {
                // select
                if (clickTarget != FromPosition)
                {
                    MakeMove(FromPosition.Value, clickTarget);
                }

                Text = "Select move";
                Boardcells[FromPosition.Value.File, FromPosition.Value.Rank].BorderStyle = BorderStyle.None;
                FromPosition = null;
            }
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
            if (piece.Color == TeamColor.White)
            {
                switch (piece)
                {
                    case Bishop _:
                        return Properties.Resources.LøberHvid;
                    case King _:
                        return Properties.Resources.KongeHvid;
                    case Pawn _:
                        return Properties.Resources.BondeHvid;
                    case Rook _:
                        return Properties.Resources.TårnHvid;
                    case Queen _:
                        return Properties.Resources.DronningHvid;
                    case Knight _:
                        return Properties.Resources.HestHvid;
                }
            }
            else
            {
                switch (piece)
                {
                    case Bishop _:
                        return Properties.Resources.LøberSort;
                    case King _:
                        return Properties.Resources.KongeSort;
                    case Pawn _:
                        return Properties.Resources.BondeSort;
                    case Rook _:
                        return Properties.Resources.TårnSort;
                    case Queen _:
                        return Properties.Resources.DronningSort;
                    case Knight _:
                        return Properties.Resources.HestSort;
                }
            }
            

            return null;
        }

        private void pictureBox1_Click(object sender, System.EventArgs e)
        {
            
        }
    }
}
