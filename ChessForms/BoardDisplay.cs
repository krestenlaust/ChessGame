using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ChessGame;
using ChessGame.Pieces;

namespace ChessForms;

public partial class BoardDisplay : Form
{
    const int CoordinateSystemPixelSize = 25;

    static readonly Color AlternateTileColor = Color.CornflowerBlue;
    static readonly Color CaptureTileAvailableColor = Color.Red;
    static readonly Color CaptureTileUnavailableColor = Color.Gray;
    static readonly Color RecentMoveColor = Color.Teal;
    static readonly Color MarkedSquareColor = Color.Green;
    static readonly Color DangersquareColor = Color.Red;
    static readonly Color CheckedSquareColor = Color.DarkRed;

    readonly Gamemode gamemode;
    readonly bool whiteLocal;
    readonly bool blackLocal;
    readonly System.Media.SoundPlayer soundPlayer = new System.Media.SoundPlayer(Properties.Resources.SkakLydfil);

    TilePictureControl[,] boardcells;
    Coordinate? fromPosition = null;
    Piece selectedPiece;
    Chessboard chessboard;
    Coordinate? recentMoveFrom = null;
    Coordinate? recentMoveTo = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="BoardDisplay"/> class.
    /// </summary>
    /// <param name="gamemode"></param>
    /// <param name="whiteLocal"></param>
    /// <param name="blackLocal"></param>
    public BoardDisplay(Gamemode gamemode, bool whiteLocal, bool blackLocal)
    {
        InitializeComponent();

        this.gamemode = gamemode;
        this.whiteLocal = whiteLocal;
        this.blackLocal = blackLocal;
    }

    /// <summary>
    /// Looks up the image of a piece.
    /// </summary>
    /// <param name="piece">The piece type to retrieve image of.</param>
    /// <returns>Localized resource reference as bitmap.</returns>
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

    /// <summary>
    /// Resets the tilecolor of a single square.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void ResetTileColor(int x, int y)
    {
        GetCell(x, y).BackColor = (x % 2) == (y % 2) ? Color.White : AlternateTileColor;
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

                pieceCoordinate = new Coordinate(x, y);

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
        tableLayoutPanelBoard.ColumnCount = chessboard.Width + 1;
        tableLayoutPanelBoard.ColumnStyles.Clear();

        for (int x = 0; x < chessboard.Width; x++)
        {
            // set size to any percent, doesnt matter
            tableLayoutPanelBoard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
        }

        tableLayoutPanelBoard.RowCount = chessboard.Height + 1;
        tableLayoutPanelBoard.RowStyles.Clear();
        for (int y = 0; y < chessboard.Height; y++)
        {
            tableLayoutPanelBoard.RowStyles.Add(new ColumnStyle(SizeType.Percent, 1));
        }

        // Coordinate row and column
        tableLayoutPanelBoard.RowStyles.Add(new RowStyle(SizeType.Absolute, CoordinateSystemPixelSize));
        tableLayoutPanelBoard.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, CoordinateSystemPixelSize));

        boardcells = new TilePictureControl[chessboard.Width, chessboard.Height];

        for (int y = 0; y < chessboard.Height; y++)
        {
            for (int x = 0; x < chessboard.Width; x++)
            {
                TilePictureControl box = new TilePictureControl();
                box.Click += CellClicked;
                tableLayoutPanelBoard.Controls.Add(box, x, y);

                boardcells[x, y] = box;
            }
        }

        Font labelFont = new Font("arial", 15, FontStyle.Bold);

        // Instantiate coordinates
        for (int x = 0; x < tableLayoutPanelBoard.ColumnCount - 1; x++)
        {
            tableLayoutPanelBoard.Controls.Add(
                new Label
                {
                    Text = ((char)(65 + x)).ToString(),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = labelFont,
                }, x, tableLayoutPanelBoard.RowCount - 1);
        }

        for (int y = 0; y < tableLayoutPanelBoard.RowCount - 1; y++)
        {
            tableLayoutPanelBoard.Controls.Add(
                new Label
                {
                    Text = (y + 1).ToString(),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = labelFont,
                }, tableLayoutPanelBoard.ColumnCount - 1, y);
        }

        ResetAllTableStyling();
    }

    /// <summary>
    /// Clears the image on a position.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void ClearPiece(int x, int y)
    {
        GetCell(x, y).Image = null;
    }

    /// <summary>
    /// Place a piece at a position.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="piece"></param>
    public void PlacePiece(int x, int y, Piece piece)
    {
        GetCell(x, y).Image = GetPieceImage(piece);
    }

    /// <summary>
    /// Color a particular square.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="color"></param>
    public void ColorSquare(int x, int y, Color color)
    {
        GetCell(x, y).BackColor = color;
    }

    public void ColorSquare(Coordinate position, Color color) => ColorSquare(position.File, position.Rank, color);

    void BoardDisplay_Load(object sender, EventArgs e)
    {
        gamemode.TurnChanged += OnTurnStarted;
        gamemode.GameStateChanged += OnGameStateUpdated;

        chessboard = gamemode.GenerateBoard();
        InstantiateUIBoard();

        UpdateBoard();

        // Starts game.
        backgroundWorkerMove.RunWorkerAsync();
    }

    void OnGameStateUpdated(GameState e)
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

    void OnTurnStarted()
    {
        Invoke((MethodInvoker)delegate
        {
            Text = $"{chessboard.CurrentPlayerTurn}'s turn";
        });

        if (MatchMaker.PlaySoundOnMove)
        {
            soundPlayer.Play();
        }

        UpdateBoard();

        if (chessboard.Moves.Count == 0)
        {
            return;
        }

        PieceMove recentMove = chessboard.Moves.Peek().Submoves[0];
        recentMoveFrom = recentMove.Source;
        recentMoveTo = recentMove.Destination;

        ResetAllTableStyling();
    }

    TilePictureControl GetCell(int x, int y)
    {
        return boardcells[x, y];
    }

    /// <summary>
    /// Resets all borderstyles and tilecolors to their correct color setting according to gamestate.
    /// </summary>
    void ResetAllTableStyling()
    {
        for (int y = 0; y < chessboard.Height; y++)
        {
            for (int x = 0; x < chessboard.Width; x++)
            {
                ResetTileColor(x, y);
                GetCell(x, y).BorderStyle = BorderStyle.None;
            }
        }

        if (recentMoveFrom is not null)
        {
            ColorSquare(recentMoveFrom.Value, RecentMoveColor);
        }

        if (recentMoveTo is not null)
        {
            ColorSquare(recentMoveTo.Value, RecentMoveColor);
        }

        if (chessboard.IsKingInCheck(chessboard.CurrentTeamTurn))
        {
            var kingsOnCheckedTeam = chessboard.GetPieces<King>()
                .Where(king => king.piece.Color == chessboard.CurrentTeamTurn);

            foreach (var (_, position) in kingsOnCheckedTeam)
            {
                ColorSquare(position, CheckedSquareColor);
            }
        }
    }

    /// <summary>
    /// Used for debugging, draws all squares, that are considered dangerzones, red.
    /// </summary>
    void DrawDangerzone()
    {
        /*
        foreach (KeyValuePair<Coordinate, List<Piece>> item in chessboard.Dangerzone)
        {
            if (!chessboard.InsideBoard(item.Key))
            {
                continue;
            }

            if (item.Value.Count == 0)
            {
                continue;
            }

            ColorSquare(item.Key, DangersquareColor);
        }*/

        for (int x = 0; x < chessboard.Width; x++)
        {
            for (int y = 0; y < chessboard.Height; y++)
            {
                Coordinate pos = new Coordinate(x, y);

                int sum = chessboard.GetDangerSquareSum(pos);

                Color color;
                if (sum > 1)
                {
                    color = Color.DarkBlue;
                }
                else if (sum == 1)
                {
                    color = Color.Blue;
                }
                else if (sum == -1)
                {
                    color = Color.Red;
                }
                else if (sum < -1)
                {
                    color = Color.DarkRed;
                }
                else
                {
                    continue;
                }

                ColorSquare(pos, color);
            }
        }
    }

    /// <summary>
    /// Makes a move.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    void MakeLocalMove(Coordinate from, Coordinate to)
    {
        IEnumerable<Move> moves = chessboard.GetMovesByNotation(from.ToString() + to.ToString(), chessboard.CurrentTeamTurn, MoveNotation.UCI);

        Move move = ChooseMove(moves);
        if (move is null)
        {
            return;
        }

        if (chessboard.PerformMove(move, false))
        {
            recentMoveFrom = from;
            recentMoveTo = to;

            ResetAllTableStyling();

            // Calls next worker.
            backgroundWorkerMove.RunWorkerAsync();
        }
    }

    Move ChooseMove(IEnumerable<Move> moves)
    {
        List<Piece> pieces = new List<Piece>();

        foreach (var move in moves)
        {
            foreach (var singleMove in move.Submoves)
            {
                if (singleMove.PromotePiece is null)
                {
                    continue;
                }

                pieces.Add(singleMove.PromotePiece);
            }
        }

        if (pieces.Count == 0)
        {
            return moves.FirstOrDefault();
        }

        PromotionPrompt prompt = new PromotionPrompt(pieces);
        DialogResult res = prompt.ShowDialog();

        if (res == DialogResult.OK)
        {
            foreach (var move in moves)
            {
                foreach (var singleMove in move.Submoves)
                {
                    if (singleMove.PromotePiece.Notation == prompt.SelectedPiece.Notation)
                    {
                        return move;
                    }
                }
            }
        }

        return moves.First();
    }

    void CellClicked(object sender, EventArgs e)
    {
        MouseButtons button = ((MouseEventArgs)e).Button;

        // translate window coordinates to table-cell coordinates
        Point click = tableLayoutPanelBoard.PointToClient(MousePosition);
        int windowX = click.X;
        int windowY = click.Y;

        // TODO: Make grid accurate by making up for the difference the coordinate system makes.
        int cellX = windowX / ((tableLayoutPanelBoard.Width - CoordinateSystemPixelSize) / chessboard.Width);
        int cellY = windowY / ((tableLayoutPanelBoard.Height - CoordinateSystemPixelSize) / chessboard.Height);

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
                    if ((chessboard.CurrentTeamTurn == TeamColor.Black && !blackLocal) ||
                        (chessboard.CurrentTeamTurn == TeamColor.White && !whiteLocal))
                    {
                        return;
                    }

                    fromPosition = clickTarget;
                    SelectPiece(cellX, cellY);
                    selectedPiece = piece;

                    foreach (var move in piece.GetMoves(chessboard))
                    {
                        if (move.Submoves[0].Destination is null)
                        {
                            continue;
                        }

                        Coordinate guardedSquare = move.Submoves[0].Destination.Value;

                        // TODO: Patrick fixer brættet, cirka her omkring
                        TilePictureControl cellImageControl = GetCell(guardedSquare.File, guardedSquare.Rank);
                        Image cellImage = cellImageControl.Image;
                        Color backgroundColor = cellImageControl.BackColor;

                        bool isMoveValid = gamemode.ValidateMove(move, chessboard);

                        if (move.Captures)
                        {
                            backgroundColor = isMoveValid ?
                                CaptureTileAvailableColor :
                                CaptureTileUnavailableColor;
                        }
                        else
                        {
                            cellImage = isMoveValid ?
                                Properties.Resources.MuligtTrkBrikTilgængelig :
                                Properties.Resources.MuligtTrkBrikUtilgængelig;
                        }

                        cellImageControl.Image = cellImage;
                        cellImageControl.BackColor = backgroundColor;
                    }
                }
                else
                {
                    // select target
                    if (clickTarget != fromPosition)
                    {
                        MakeLocalMove(fromPosition.Value, clickTarget);
                    }

                    UpdateBoard();

                    DeselectPiece(cellX, cellY);
                    selectedPiece = null;
                    fromPosition = null;
                }

                break;
            case MouseButtons.None:
                break;
            case MouseButtons.Right: // Mark square.
                                     // do not change color if square is already colored.
                if ((recentMoveFrom?.File == cellX && recentMoveFrom?.Rank == cellY) ||
                    (recentMoveTo?.File == cellX && recentMoveTo?.Rank == cellY))
                {
                    break;
                }

                if (GetCell(cellX, cellY).BackColor == MarkedSquareColor)
                {
                    ResetTileColor(cellX, cellY);
                }
                else
                {
                    GetCell(cellX, cellY).BackColor = MarkedSquareColor;
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
    void DeselectPiece(int x, int y)
    {
        GetCell(x, y).BorderStyle = BorderStyle.None;
    }

    /// <summary>
    /// Changes borderstyle on a particular position.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void SelectPiece(int x, int y)
    {
        GetCell(x, y).BorderStyle = BorderStyle.FixedSingle;
    }

    void BackgroundWorkerMove_DoWork(object sender, DoWorkEventArgs e)
    {
        chessboard.StartNextTurn();
    }

    void BackgroundWorkerMove_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
    }

    void BoardDisplay_KeyUp(object sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.Left:
                break;
            case Keys.Right:
                break;
            case Keys.R:
                // Refresh dangerzone
                chessboard.UpdateDangerzones();
                break;
            case Keys.Space:
                // Display dangerzone
                DrawDangerzone();
                break;
            default:
                break;
        }
    }
}
