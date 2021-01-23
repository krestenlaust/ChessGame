using System.Drawing;
using System.Windows.Forms;
using ChessGame;
using ChessGame.Pieces;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace ChessForms
{
    public partial class BoardDisplay : Form
    {
        private PictureBox[,] boardcells;
        private Coordinate? fromPosition = null;
        private Piece selectedPiece;
        private readonly Gamemode gamemode;
        public Chessboard chessboard;
        private readonly bool whiteLocal, blackLocal;
        private bool flipped = false;

        public BoardDisplay(Gamemode gamemode, bool whiteLocal, bool blackLocal)
        {
            InitializeComponent();
            
            this.gamemode = gamemode;
            this.whiteLocal = whiteLocal;
            this.blackLocal = blackLocal;
        }

        private void BoardDisplay_Load(object sender, EventArgs e)
        {
            gamemode.onTurnChanged += onTurnStarted;
            gamemode.onGameStateUpdated += onGameStateUpdated;

            chessboard = gamemode.GenerateBoard();
            InstantiateUIBoard();

            UpdateBoard();
            
            Task.Run(() => chessboard.StartGame());
        }

        private void onGameStateUpdated(GameState e)
        {
            string outputMsg = string.Empty;
            switch (e)
            {
                case GameState.Stalemate:
                    outputMsg = "The game has stalemated, Game is over";
                    break;
                case GameState.Checkmate:
                    outputMsg = $"{gamemode.Winner} has delivered checkmate!";
                    break;
                case GameState.Check:
                    outputMsg = $"{chessboard.CurrentPlayerTurn} is in check!";
                    break;
            }

            if (outputMsg == string.Empty)
            {
                return;
            }

            this.Invoke((MethodInvoker)delegate
            {
                Text = outputMsg;
            });
        }

        private void onTurnStarted()
        {
            Invoke((MethodInvoker)delegate
           {
               Text = $"{chessboard.CurrentPlayerTurn}'s turn";
           });

            Console.Beep();
            UpdateBoard();
        }

        private void ResetTableStyling()
        {
            int i = 0;
            for (int y = 0; y < chessboard.Height; y++)
            {
                for (int x = 0; x < chessboard.Width; x++)
                {
                    boardcells[x, y].BackColor = ++i % 2 == 0 ? Color.White : Color.CornflowerBlue;
                    boardcells[x, y].BorderStyle = BorderStyle.None;
                }

                i++;
            }
        }

        public void UpdateBoard()
        {
            for (int y = 0; y < chessboard.Height; y++)
            {
                for (int x = chessboard.Width - 1; x >= 0; x--)
                {
                    Coordinate pieceCoordinate = new Coordinate(x, y);

                    Piece cellPiece = chessboard.GetPiece(pieceCoordinate);

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

        public void InstantiateUIBoard()
        {
            tableLayoutPanel1.ColumnCount = chessboard.Width;
            tableLayoutPanel1.ColumnStyles.Clear();
            int i;
            for (i = 0; i < chessboard.Width; i++)
            {
                // set size to any percent, doesnt matter
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            }

            tableLayoutPanel1.RowCount = chessboard.Height;
            tableLayoutPanel1.RowStyles.Clear();
            for (i = 0; i < chessboard.Height; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            }

            boardcells = new PictureBox[chessboard.Width, chessboard.Height];

            for (int y = 0; y < chessboard.Height; y++)
            {
                for (int x = 0; x < chessboard.Width; x++)
                {

                    PictureBox box = new PictureBox
                    {
                        Dock = DockStyle.Fill,
                        BorderStyle = BorderStyle.None,
                        SizeMode = PictureBoxSizeMode.Zoom,
                    };

                    box.Click += CellClicked;
                    boardcells[x, y] = box;

                    tableLayoutPanel1.Controls.Add(box);
                }
            }

            ResetTableStyling();
        }

        private void MakeMove(Coordinate from, Coordinate to)
        {
            string move = from.ToString() + to.ToString();

            Thread moveThread = new Thread(() => chessboard.PerformMove(move, MoveNotation.UCI));
            moveThread.Start();
        }

        private void CellClicked(object sender, EventArgs e)
        {
            MouseButtons button = ((MouseEventArgs)e).Button;

            // translate window coordinates to table-cell coordinates
            Point click = tableLayoutPanel1.PointToClient(MousePosition);
            int windowX = click.X;
            int windowY = click.Y;
            
            int cellX = windowX / (tableLayoutPanel1.Width / chessboard.Width);
            int cellY = windowY / (tableLayoutPanel1.Height / chessboard.Height);

            Coordinate clickTarget = new Coordinate(cellX, cellY);
            MessageBox.Show(clickTarget.ToString());

            // handle click
            switch (button)
            {
                case MouseButtons.Left:
                    ResetTableStyling();

                    Piece piece = chessboard[clickTarget];

                    if (!(fromPosition is null) && piece?.Color == chessboard.CurrentTurn && piece != selectedPiece)
                    {
                        DeselectPiece(fromPosition.Value.File, fromPosition.Value.Rank);
                        UpdateBoard();
                        fromPosition = null;
                    }

                    if (fromPosition is null)
                    {
                        // wrong color piece selected
                        if (piece is null || piece.Color != chessboard.CurrentTurn)
                        {
                            return;
                        }

                        // only allow selection of local players
                        if (chessboard.CurrentTurn == TeamColor.Black && !blackLocal ||
                            chessboard.CurrentTurn == TeamColor.White && !whiteLocal)
                        {
                            return;
                        }

                        fromPosition = clickTarget;
                        SelectPiece(cellX, cellY);
                        selectedPiece = piece;
                        

                        foreach (var item in piece.GetMoves(chessboard))
                        {
                            if (item.Moves[0].Destination is null)
                            {
                                continue;
                            }

                            Coordinate guardedSquare = item.Moves[0].Destination.Value;

                            Image cellImage = boardcells[guardedSquare.File, guardedSquare.Rank].Image;

                            if (cellImage is null)
                            {
                                boardcells[guardedSquare.File, guardedSquare.Rank].Image = Properties.Resources.MuligtTrækBrik;
                            }
                            else
                            {
                                boardcells[guardedSquare.File, guardedSquare.Rank].BackColor = Color.Red;
                            }
                        }
                    }
                    else
                    {
                        // select target
                        if (clickTarget != fromPosition)
                        {
                            DeselectPiece(cellX, cellY);
                            MakeMove(fromPosition.Value, clickTarget);
                        }

                        UpdateBoard();

                        DeselectPiece(cellX, cellY);
                        selectedPiece = null;
                        fromPosition = null;
                    }
                    break;
                case MouseButtons.None:
                    break;
                case MouseButtons.Right:
                    boardcells[cellX, cellY].BackColor = Color.Green;
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
            if (!flipped)
            {
                x = (chessboard.Width - 1) - x;
                y = (chessboard.Height - 1) - y;
            }

            boardcells[x, y].BorderStyle = BorderStyle.None;
        }

        private void SelectPiece(int x, int y)
        {
            if (!flipped)
            {
                x = (chessboard.Width - 1) - x;
                y = (chessboard.Height - 1) - y;
            }

            boardcells[x, y].BorderStyle = BorderStyle.FixedSingle;
        }

        public void ClearPiece(int x, int y)
        {
            if (!flipped)
            {
                x = (chessboard.Width - 1) - x;
                y = (chessboard.Height - 1) - y;
            }

            boardcells[x, y].Image = null;
        }

        public void PlacePiece(int x, int y, Piece piece)
        {
            if (!flipped)
            {
                x = (chessboard.Width - 1) - x;
                y = (chessboard.Height - 1) - y;
            }

            boardcells[x, y].Image = GetPieceImage(piece);
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
