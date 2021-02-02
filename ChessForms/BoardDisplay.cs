using ChessGame;
using ChessGame.Pieces;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessForms
{
    public partial class BoardDisplay : Form
    {
        private readonly Color AlternateTileColor = Color.CornflowerBlue;
        private readonly Color CaptureTileAvailableColor = Color.Red;
        private readonly Color CaptureTileUnavailableColor = Color.Gray;
        private readonly Color RecentMoveColor = Color.Green;
        private readonly Color MarkedSquareColor = Color.Green;
        private readonly Color DangersquareColor = Color.Red;

        private TilePictureControl[,] boardcells;
        private Coordinate? fromPosition = null;
        private Piece selectedPiece;
        private readonly Gamemode gamemode;
        private Chessboard chessboard;
        private readonly bool whiteLocal, blackLocal;
        private bool unFlipped = false;
        private Coordinate? recentFrom = null;
        private Coordinate? recentTo = null;

        public BoardDisplay(Gamemode gamemode, bool whiteLocal, bool blackLocal)
        {
            InitializeComponent();

            this.gamemode = gamemode;
            this.whiteLocal = whiteLocal;
            this.blackLocal = blackLocal;

            // flip board if black is only local player
            //unFlipped = !(blackLocal && !whiteLocal);
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

        public static Image GetPieceImage(Piece piece)
        {
            if (piece.Color == TeamColor.White)
            {
                switch (piece)
                {
                    case Bishop:
                        return Properties.Resources.LøberHvid;
                    case King:
                        return Properties.Resources.KongeHvid;
                    case Pawn:
                        return Properties.Resources.BondeHvid;
                    case Rook:
                        return Properties.Resources.TårnHvid;
                    case Queen:
                        return Properties.Resources.DronningHvid;
                    case Knight:
                        return Properties.Resources.HestHvid;
                }
            }
            else
            {
                switch (piece)
                {
                    case Bishop:
                        return Properties.Resources.LøberSort;
                    case King:
                        return Properties.Resources.KongeSort;
                    case Pawn:
                        return Properties.Resources.BondeSort;
                    case Rook:
                        return Properties.Resources.TårnSort;
                    case Queen:
                        return Properties.Resources.DronningSort;
                    case Knight:
                        return Properties.Resources.HestSort;
                }
            }

            return null;
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

            Invoke((MethodInvoker)delegate
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

            if (MatchMaker.PlaySoundOnMove)
            {
                Console.Beep();
            }

            UpdateBoard();
        }

        /// <summary>
        /// Resets all borderstyles and tilecolors.
        /// </summary>
        private void ResetAllTableStyling()
        {
            for (int y = 0; y < chessboard.Height; y++)
            {
                for (int x = 0; x < chessboard.Width; x++)
                {
                    ResetTileColor(x, y);
                    boardcells[x, y].BorderStyle = BorderStyle.None;

                    if (recentFrom is not null)
                    {
                        ColorSquare(recentFrom.Value.File, recentFrom.Value.Rank, RecentMoveColor);
                    }
                    
                    if (recentTo is not null)
                    {
                        ColorSquare(recentTo.Value.File, recentTo.Value.Rank, RecentMoveColor);
                    }
                }
            }
        }

        /// <summary>
        /// Resets the tilecolor of a single square.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void ResetTileColor(int x, int y)
        {
            boardcells[x, y].BackColor = (x % 2) == (y % 2) ? Color.White : AlternateTileColor;
        }

        /// <summary>
        /// Updates the board to represent the position portrayed by <c>chessboard</c>.
        /// </summary>
        public void UpdateBoard()
        {
            for (int y = 0; y < chessboard.Height; y++)
            {
                for (int x = 0; x < chessboard.Width; x++)
                {
                    Coordinate pieceCoordinate;

                    if (unFlipped)
                    {
                        pieceCoordinate = new Coordinate(chessboard.Width - 1 - x, chessboard.Height - 1 - y); //Det her burde også fikse noget
                    } //#442
                    else
                    {
                        pieceCoordinate = new Coordinate(x, y);
                    }

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

        /// <summary>
        /// Instantiates UI controls, creates the visual representation of the chessboard.
        /// </summary>
        public void InstantiateUIBoard()
        {
            tableLayoutPanel1.ColumnCount = chessboard.Width + 1;
            tableLayoutPanel1.ColumnStyles.Clear();
            int i;
            for (i = 0; i < chessboard.Width; i++)
            {
                // set size to any percent, doesnt matter
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            }

            tableLayoutPanel1.RowCount = chessboard.Height + 1;
            tableLayoutPanel1.RowStyles.Clear();
            for (i = 0; i < chessboard.Height; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            }

            // Coordinate row and column
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 25));

            boardcells = new TilePictureControl[chessboard.Width, chessboard.Height];

            for (int y = 0; y < chessboard.Height; y++)
            {
                for (int x = 0; x < chessboard.Width; x++)
                {
                    TilePictureControl box = new TilePictureControl();

                    box.Click += CellClicked;

                    if (unFlipped)
                    {
                        boardcells[chessboard.Width - 1 - x, (chessboard.Height - 1) - y] = box;
                    } //#442
                    else
                    {
                        boardcells[x, y] = box;
                    }

                    tableLayoutPanel1.Controls.Add(box, x, y);
                }
            }

            Font labelFont = new Font("ariel", 15, FontStyle.Bold);
            // Instantiate coordinates
            for (int x = 0; x < tableLayoutPanel1.ColumnCount - 1; x++)
            {
                Label label = new Label
                {
                    Text = ((char)(65 + x)).ToString(),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = labelFont
                };

                tableLayoutPanel1.Controls.Add(label, x, tableLayoutPanel1.RowCount - 1);
            }
            for (int y = 0; y < tableLayoutPanel1.RowCount - 1; y++)
            {
                Label label = new Label
                {
                    Text =  (chessboard.Height - y).ToString(),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = labelFont
                };

                tableLayoutPanel1.Controls.Add(label, tableLayoutPanel1.ColumnCount - 1, y);
            }

            ResetAllTableStyling();
        }

        /// <summary>
        /// Used for debugging, draws all squares, that are considered dangerzones, red.
        /// </summary>
        private void DrawDangerzone()
        {
            foreach (var item in chessboard.Dangerzone)
            {
                if (!chessboard.InsideBoard(item.Key))
                {
                    continue;
                }

                ColorSquare(item.Key.File, item.Key.Rank, DangersquareColor);
            }
        }

        /// <summary>
        /// Makes a move.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private void MakeMove(Coordinate from, Coordinate to)
        {
            string move = from.ToString() + to.ToString();

            if (chessboard.PerformMove(move, MoveNotation.UCI))
            {
                // TODO: Draw color green to show what piece moved and where.
                recentFrom = from;
                recentTo = to;

                ResetAllTableStyling();
            }
            
            // TODO: Use backgroundworker instead.
            //Thread moveThread = new Thread(() => chessboard.PerformMove(move, MoveNotation.UCI));
            //moveThread.Start();
        }

        private void CellClicked(object sender, EventArgs e)
        {
            MouseButtons button = ((MouseEventArgs)e).Button;

            // translate window coordinates to table-cell coordinates
            Point click = tableLayoutPanel1.PointToClient(MousePosition);
            int windowX = click.X;
            int windowY = click.Y;

            // TODO: Make grid accurate by making up for the difference the coordinate system makes.
            int cellX = windowX / (tableLayoutPanel1.Width / chessboard.Width);
            int cellY = windowY / (tableLayoutPanel1.Height / chessboard.Height);

            if (unFlipped)
            {
                cellY = (chessboard.Height - 1) - cellY;
            }
            else
            {
                //cellX = (chessboard.Width - 1) - cellX; //Det her fikser lidt et problem
            }

            Coordinate clickTarget = new Coordinate(cellX, cellY);

            // handle click
            switch (button)
            {
                // Deselects all squares marked, (de)selects a piece, or makes a move with a selected piece.
                case MouseButtons.Left:
                    ResetAllTableStyling();

                    Piece piece = chessboard[clickTarget];

                    // deselect piece selected
                    if (fromPosition is not null && piece?.Color == chessboard.CurrentTeamTurn && piece != selectedPiece)
                    {
                        DeselectPiece(fromPosition.Value.File, fromPosition.Value.Rank);
                        UpdateBoard();
                        fromPosition = null;
                    }

                    if (fromPosition is null)
                    {
                        // return if piece is null or has wrong color.
                        if (piece?.Color != chessboard.CurrentTeamTurn)
                        {
                            return;
                        }

                        // only allow selection of local players
                        if (chessboard.CurrentTeamTurn == TeamColor.Black && !blackLocal ||
                            chessboard.CurrentTeamTurn == TeamColor.White && !whiteLocal)
                        {
                            return;
                        }

                        fromPosition = clickTarget;
                        SelectPiece(cellX, cellY);
                        selectedPiece = piece;
                        
                        foreach (var move in piece.GetMoves(chessboard))
                        {
                            if (move.Moves[0].Destination is null)
                            {
                                continue;
                            }

                            Coordinate guardedSquare = move.Moves[0].Destination.Value;

                            // TODO: Patrick fixer brættet, cirka her omkring
                            Image cellImage = boardcells[guardedSquare.File, guardedSquare.Rank].Image;

                            if (cellImage is null)
                            {
                                Bitmap dotTexture;
                                if (gamemode.ValidateMove(move, chessboard))
                                {
                                    dotTexture = Properties.Resources.MuligtTrkBrikTilgængelig;
                                }
                                else
                                {
                                    dotTexture = Properties.Resources.MuligtTrkBrikUtilgængelig;
                                }

                                boardcells[guardedSquare.File, guardedSquare.Rank].Image = dotTexture;
                            }
                            else
                            {
                                Color backgroundColor;
                                if (gamemode.ValidateMove(move, chessboard))
                                {
                                    backgroundColor = CaptureTileAvailableColor;
                                }
                                else
                                {
                                    backgroundColor = CaptureTileUnavailableColor;
                                }

                                boardcells[guardedSquare.File, guardedSquare.Rank].BackColor = backgroundColor;
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
                    // Mark square.
                case MouseButtons.Right:
                    // do not change color if square is already colored.
                    if (recentFrom?.File == cellX && recentFrom?.Rank == cellY || 
                        recentTo?.File == cellX && recentTo?.Rank == cellY)
                    {
                        break;
                    }

                    if (boardcells[cellX, cellY].BackColor == MarkedSquareColor)
                    {
                        ResetTileColor(cellX, cellY);
                    }
                    else
                    {
                        boardcells[cellX, cellY].BackColor = MarkedSquareColor;
                    }
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

        /// <summary>
        /// Clears bordersrtle on a particular position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void DeselectPiece(int x, int y)
        {
            // TODO: Clean up the mess in these methods.
            if (unFlipped)
            {
                y = (chessboard.Height - 1) - y;
            }
            else
            {
                //x = (chessboard.Width - 1) - x;
            }

            boardcells[x, y].BorderStyle = BorderStyle.None;
        }
        
        /// <summary>
        /// Changes borderstyle on a particular position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void SelectPiece(int x, int y)
        {
            /*if (flipped) //flip fiks
            {
                y = (chessboard.Height - 1) - y;
            }
            else
            {
                x = (chessboard.Width - 1) - x;
            }*/

            boardcells[x, y].BorderStyle = BorderStyle.FixedSingle;
        }

        // Clear the image on a position.
        public void ClearPiece(int x, int y)
        {
            if (unFlipped)
            {
                y = (chessboard.Height - 1) - y;
            }
            else
            {
                //x = (chessboard.Width - 1) - x;
            }

            boardcells[x, y].Image = null;
        }

        /// <summary>
        /// Place a piece at a position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="piece"></param>
        public void PlacePiece(int x, int y, Piece piece)
        {
            if (unFlipped)
            {
                y = (chessboard.Height - 1) - y;
            }
            else
            {
                //x = (chessboard.Width - 1) - x;
            }

            boardcells[x, y].Image = GetPieceImage(piece);
        }

        /// <summary>
        /// Color a particular square.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void ColorSquare(int x, int y, Color color)
        {
            if (unFlipped)
            {
                y = (chessboard.Height - 1) - y;
            }
            else
            {
                //x = (chessboard.Width - 1) - x;
            }

            boardcells[x, y].BackColor = color;
        }

        private void BoardDisplay_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    break;
                case Keys.Right:
                    break;
                    // Refresh dangerzone
                case Keys.R:
                    chessboard.UpdateDangerzones();
                    break;
                    // Display dangerzone
                case Keys.Space:
                    DrawDangerzone();
                    break;
                default:
                    break;
            }
        }
    }
}
