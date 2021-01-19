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
        public Piece SelectedPiece = null;

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

        private void UpdateBackgroundColors()
        {
            int i = 0;
            for (int y = 0; y < Chessboard.Height; y++)
            {
                for (int x = 0; x < Chessboard.Width; x++)
                {
                    Boardcells[x, y].BackColor = ++i % 2 == 0 ? Color.White : Color.CornflowerBlue;
                }

                i++;
            }
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

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {

                    PictureBox box = new PictureBox
                    {
                        Dock = DockStyle.Fill,
                        BorderStyle = BorderStyle.None,
                        SizeMode = PictureBoxSizeMode.Zoom,
                    };

                    box.Click += CellClicked;
                    Boardcells[x, y] = box;

                    tableLayoutPanel1.Controls.Add(box);
                }
            }

            UpdateBackgroundColors();
        }

        private void MakeMove(Coordinate from, Coordinate to)
        {
            if (Chessboard.PerformMove(from.ToString() + to.ToString(), MoveNotation.UCI))
            {
                //UpdateBoard();
                
                //Image image = Boardcells[from.File, from.Rank].Image;
                //Boardcells[to.File, to.Rank].Image = image;
                //Boardcells[from.File, from.Rank].Image = null;
            }
        }

        private void CellClicked(object sender, System.EventArgs e)
        {
            MouseButtons button = ((MouseEventArgs)e).Button;

            Point click = tableLayoutPanel1.PointToClient(MousePosition);
            int windowX = click.X;
            int windowY = click.Y;
            
            int cellX = windowX / (tableLayoutPanel1.Width / BoardWidth);
            int cellY = windowY / (tableLayoutPanel1.Height / BoardHeight);

            Coordinate clickTarget = new Coordinate(cellX, cellY);

            switch (button)
            {
                case MouseButtons.Left:
                    UpdateBackgroundColors();

                    Piece piece = Chessboard[new Coordinate(cellX, cellY)];

                    if (!(FromPosition is null) && piece?.Color == Chessboard.CurrentTurn && piece != SelectedPiece)
                    {
                        DeselectPiece(FromPosition.Value.File, FromPosition.Value.Rank);
                        UpdateBoard();
                        FromPosition = null;
                    }

                    if (FromPosition is null)
                    {
                        if (piece is null || piece.Color != Chessboard.CurrentTurn)
                        {
                            return;
                        }

                        FromPosition = clickTarget;
                        this.Text = FromPosition.ToString();
                        SelectPiece(cellX, cellY);
                        SelectedPiece = piece;
                        

                        foreach (var item in piece.GetMoves(Chessboard))
                        {
                            Coordinate guardedSquare = item.Moves[0].Destination;

                            Image cellImage = Boardcells[guardedSquare.File, guardedSquare.Rank].Image;

                            if (cellImage is null)
                            {
                                Boardcells[guardedSquare.File, guardedSquare.Rank].Image = Properties.Resources.MuligtTrækBrik;
                            }
                            else
                            {
                                Boardcells[guardedSquare.File, guardedSquare.Rank].BackColor = Color.Red;
                            }
                        }
                    }
                    else
                    {
                        // select target
                        if (clickTarget != FromPosition)
                        {
                            DeselectPiece(cellX, cellY);
                            MakeMove(FromPosition.Value, clickTarget);
                        }

                        UpdateBoard();

                        Text = "Select move";
                        DeselectPiece(cellX, cellY);
                        SelectedPiece = null;
                        FromPosition = null;
                    }
                    break;
                case MouseButtons.None:
                    break;
                case MouseButtons.Right:
                    Boardcells[cellX, cellY].BackColor = Color.Green;
                    break;
                case MouseButtons.Middle:
                    break;
                case MouseButtons.XButton1:
                    break;
                case MouseButtons.XButton2:
                    break;
                default:
                    break;
            }
        }

        private void DeselectPiece(int x, int y)
        {
            Boardcells[x, y].BorderStyle = BorderStyle.None;
        }

        private void SelectPiece(int x, int y)
        {
            Boardcells[x, y].BorderStyle = BorderStyle.FixedSingle;
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
