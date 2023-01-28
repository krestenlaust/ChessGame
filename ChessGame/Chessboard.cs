using System;
using System.Collections.Generic;
using System.Linq;
using ChessGame.Pieces;

namespace ChessGame;

public enum MoveNotation : byte
{
    /// <summary>
    /// Universal Chess Interface.
    /// </summary>
    UCI,

    /// <summary>
    /// Standard algebraic notation.
    /// </summary>
    StandardAlgebraic,
}

/// <summary>
/// A class that describes a game of chess.
/// </summary>
public class Chessboard : IEquatable<Chessboard>
{
    public readonly byte Height;
    public readonly byte Width;

    /// <summary>
    /// A list of pieces, that have moved.
    /// TODO: Rediscover the application of MovedPieces.
    /// </summary>
    public readonly HashSet<Piece> MovedPieces;
    readonly Gamemode gamemode;

    /// <summary>
    /// Initializes a new instance of the <see cref="Chessboard"/> class,
    /// which is a copy of <c>board</c>, teamColor references stay the same.
    /// </summary>
    /// <param name="board">The board to copy.</param>
    public Chessboard(Chessboard board)
    {
        Height = board.Height;
        Width = board.Width;
        CurrentTeamTurn = board.CurrentTeamTurn;
        gamemode = board.gamemode;
        CurrentState = board.CurrentState;

        Pieces = new Dictionary<Coordinate, Piece>(board.Pieces);
        MovedPieces = new HashSet<Piece>(board.MovedPieces);
        Moves = new Stack<Move>(board.Moves);

        // not needed before executing move
        Dangerzone = new Dictionary<Coordinate, List<Piece>>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Chessboard"/> class, performs a deep-copy of the chessboard, and simulates a <see cref="Move"/>.
    /// </summary>
    /// <param name="board">The chessboard instance to copy.</param>
    /// <param name="move">The move to simulate.</param>
    public Chessboard(Chessboard board, Move move)
    {
        Height = board.Height;
        Width = board.Width;
        CurrentTeamTurn = board.CurrentTeamTurn;
        gamemode = board.gamemode;
        CurrentState = board.CurrentState;

        Pieces = new Dictionary<Coordinate, Piece>(board.Pieces);
        MovedPieces = new HashSet<Piece>(board.MovedPieces);
        Moves = new Stack<Move>(board.Moves.Reverse());

        // is refreshed in simulatemove
        Dangerzone = new Dictionary<Coordinate, List<Piece>>();

        SimulateMove(move);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Chessboard"/> class.
    /// </summary>
    /// <param name="width">The amount of files.</param>
    /// <param name="height">The amount of ranks.</param>
    /// <param name="gamemode">The game mode to be associated with this board instance.</param>
    public Chessboard(byte width, byte height, Gamemode gamemode)
    {
        Width = width;
        Height = height;
        this.gamemode = gamemode;
        CurrentTeamTurn = TeamColor.White;
        CurrentState = GameState.NotStarted;

        Pieces = new Dictionary<Coordinate, Piece>();
        Dangerzone = new Dictionary<Coordinate, List<Piece>>();
        Moves = new Stack<Move>();
        MovedPieces = new HashSet<Piece>();
    }

    /// <summary>
    /// Gets the pieces with their associated position on the board.
    /// </summary>
    public Dictionary<Coordinate, Piece> Pieces { get; }

    /// <summary>
    /// Gets a dictionary that describes intersection squares. An intersection square is a square which one or more pieces threaten at once.
    /// </summary>
    public Dictionary<Coordinate, List<Piece>> Dangerzone { get; }

    /// <summary>
    /// Gets previous moves, that resolve to this position.
    /// </summary>
    public Stack<Move> Moves { get; }

    public GameState CurrentState { get; internal set; }

    public TeamColor CurrentTeamTurn { get; set; }

    /// <summary>
    /// Gets a Player object that is equal to the teamColor whose turn it is.
    /// </summary>
    public Player CurrentPlayerTurn => CurrentTeamTurn == TeamColor.White ? gamemode.PlayerWhite : gamemode.PlayerBlack;

    /// <summary>
    /// Gets a value to determine the material sum of the game.
    /// </summary>
    public int MaterialSum
    {
        get
        {
            return Pieces.Values.Sum(p => p.Color == TeamColor.Black ? -p.MaterialValue : p.MaterialValue);
        }
    }

    /// <summary>
    /// Gets a value indicating whether the game has finished.
    /// </summary>
    public bool GameFinished =>
        CurrentState is GameState.Checkmate or GameState.Stalemate or GameState.DeadPosition;

    /// <summary>
    /// Gets piece by position.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Piece this[Coordinate position]
    {
        get => GetPiece(position);
        set
        {
            if (value is null)
            {
                Pieces.Remove(position);
            }
            else
            {
                Pieces[position] = value;
            }
        }
    }

    public static bool operator ==(Chessboard left, Chessboard right) => EqualityComparer<Chessboard>.Default.Equals(left, right);

    public static bool operator !=(Chessboard left, Chessboard right) => !(left == right);

    /// <summary>
    /// Makes a move based on move notation. Calls <c>MakeMove(Move)</c> once move is translated.
    /// </summary>
    /// <param name="move"></param>
    /// <param name="notationType"></param>
    /// <param name="startNextTurn"></param>
    /// <returns></returns>
    public bool PerformMove(string move, MoveNotation notationType, bool startNextTurn = true)
    {
        Move pieceMove = GetMoveByNotation(move, CurrentTeamTurn, notationType);

        if (pieceMove is null)
            return false;

        return PerformMove(pieceMove, startNextTurn);
    }

    /// <summary>
    /// Updates chessboard state based on move, adds the move to the move stack, and calls <c>StartNextTurn</c>.
    /// </summary>
    /// <param name="move"></param>
    /// <param name="startNextTurn"></param>
    /// <returns></returns>
    public bool PerformMove(Move move, bool startNextTurn = true)
    {
        ExecuteMove(move);
        Moves.Push(move);

        CurrentTeamTurn = (TeamColor)(((int)CurrentTeamTurn + 1) % 2);

        if (startNextTurn)
        {
            StartNextTurn();
        }

        return true;
    }

    /// <summary>
    /// Updates the chessboard state, but doesnt call event listeners.
    /// </summary>
    /// <param name="move"></param>
    /// <returns></returns>
    public bool SimulateMove(Move move)
    {
        ExecuteMove(move);
        Moves.Push(move);

        CurrentTeamTurn = (TeamColor)(((int)CurrentTeamTurn + 1) % 2);

        gamemode.UpdateGameState(this);

        return true;
    }

    /// <summary>
    /// Starts game if it hasn't started yet. Refreshes dangerzone, updates turn, checks for check.
    /// </summary>
    public void StartNextTurn()
    {
        if (CurrentState == GameState.NotStarted)
        {
            CurrentState = GameState.Started;
            UpdateDangerzones();
        }

        // change turn
        if (gamemode.StartTurn(this))
        {
            CurrentPlayerTurn?.TurnStarted(this);
        }
    }

    /// <summary>
    /// Gets all legal moves for current team.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Move> GetMoves() => GetMoves(CurrentTeamTurn);

    public IEnumerable<Move> GetMovesSorted()
    {
        return from move in GetMoves()
               orderby move.Captures
               select move;
    }

    /// <summary>
    /// Gets all legal moves for a team.
    /// </summary>
    /// <param name="teamColor"></param>
    /// <returns></returns>
    public IEnumerable<Move> GetMoves(TeamColor teamColor) => Pieces.Values
            .Where(p => p.Color == teamColor)
            .SelectMany(p => p.GetMoves(this))
            .Where(move => gamemode.ValidateMove(move, this));

    /// <summary>
    /// Checks if a given position is inside the board bounds (or otherwise invalid).
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool InsideBoard(Coordinate position)
    {
        if (position.Rank >= Height || position.Rank < 0)
        {
            return false;
        }

        if (position.File >= Width || position.File < 0)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Executes a move by updating the board accordingly.
    /// </summary>
    /// <param name="move"></param>
    public void ExecuteMove(Move move)
    {
        if (move is null)
        {
            return;
        }

        foreach (var singleMove in move.Submoves)
        {
            // remove piece
            if (singleMove.Destination is null)
            {
                Pieces.Remove(singleMove.Source.Value);
                return;
            }

            Coordinate destination = singleMove.Destination.Value;

            // Remove previous instance
            if (!(singleMove.Source is null))
            {
                Pieces.Remove(singleMove.Source.Value);
            }

            if (singleMove.Captures)
            {
                try
                {
                    Pieces.Remove(destination);
                }
                catch (ArgumentException)
                {
                }
            }

            if (singleMove.PromotePiece is null)
            {
                Pieces[destination] = singleMove.Piece;
                MovedPieces.Add(singleMove.Piece);
            }
            else
            {
                Pieces[destination] = singleMove.PromotePiece;
                MovedPieces.Remove(singleMove.Piece);
                MovedPieces.Add(singleMove.PromotePiece);
            }
        }

        // TODO: Only update neccessary dangerzones.
        // It should be possible to only update the pieces with dangerzone on Source square, and on Destination square.
        // And the moved piece itself.
        UpdateDangerzones();
    }

    /// <summary>
    /// Returns whether the king of a team is in check.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public bool IsKingInCheck(TeamColor color)
    {
        Piece king = null;
        var position = default(Coordinate);

        // Get first king, with this teamColor, and it's position.
        (king, position) = (from piece in GetPieces<King>()
                            where piece.piece.Color == color
                            select piece).FirstOrDefault();

        // No king of this teamColor was found, thus the king can't be in check.
        if (king is null)
        {
            return false;
        }

        // Get list of pieces aiming on the square the king sits on.
        if (Dangerzone.TryGetValue(position, out List<Piece> pieces))
        {
            // Returns true if any of the pieces are of opposite teamColor.
            return pieces.Any(p => p.Color != color);
        }
        else
        {
            // No pieces of opposite teamColor aiming on this square, king not in check.
            return false;
        }
    }

    /// <summary>
    /// This method gets a collection of moves by the notation you put in as parameters.
    /// </summary>
    /// <param name="notation"></param>
    /// <param name="player"></param>
    /// <param name="notationType"></param>
    /// <returns></returns>
    public IEnumerable<Move> GetMovesByNotation(string notation, TeamColor player, MoveNotation notationType)
    {
        Coordinate? source = null;
        Coordinate? destination = null;
        bool? captures = null;
        char? pieceNotation = null;
        char? promotionTarget = null;
        bool customNotation = false;

        try
        {
            switch (notationType)
            {
                case MoveNotation.UCI:
                    if (notation.Length >= 4)
                    {
                        source = new Coordinate(notation.Substring(0, 2));
                        destination = new Coordinate(notation.Substring(2, 2));
                    }

                    if (notation.Length == 5)
                    {
                        promotionTarget = notation[4];
                    }

                    break;
                case MoveNotation.StandardAlgebraic:
                    // read destination square
                    int lastNumberIndex = 0;
                    for (int i = 0; i < notation.Length; i++)
                    {
                        if (char.IsDigit(notation[i]))
                        {
                            lastNumberIndex = i;
                        }

                        if (notation[i] == 'x')
                        {
                            captures = true;
                        }
                    }

                    if (lastNumberIndex == 0)
                    {
                        customNotation = true;
                        break;
                    }

                    destination = new Coordinate(notation.Substring(lastNumberIndex - 1, 2));

                    // more at the end
                    if (notation.Length - 1 > lastNumberIndex)
                    {
                        // promotion
                        if (notation[lastNumberIndex + 1] == '=')
                        {
                            promotionTarget = notation[lastNumberIndex + 2];
                            pieceNotation = '\0';
                        }
                    }

                    if (lastNumberIndex - 1 == 0)
                    {
                        pieceNotation = '\0';
                    }
                    else
                    {
                        pieceNotation = notation[0];
                    }

                    break;
            }
        }
        catch (Exception)
        {
            // if notation parsing threw exception, then it's probably custom notation.
            customNotation = true;
        }

        // Look for matching moves.
        foreach (var move in GetMoves(player))
        {
            // If looking for move by custom notation, return it here.
            if (customNotation && move.CustomNotation == notation)
            {
                if (gamemode.ValidateMove(move, this))
                {
                    yield return move;
                }

                continue;
            }

            foreach (var pieceMove in move.Submoves)
            {
                if (!(source is null) && source != pieceMove.Source)
                {
                    continue;
                }

                if (!(destination is null) && destination != pieceMove.Destination)
                {
                    continue;
                }

                if (!(captures is null) && captures != pieceMove.Captures)
                {
                    continue;
                }

                if (!(pieceNotation is null) && pieceNotation != pieceMove.Piece.Notation)
                {
                    continue;
                }

                if (!(promotionTarget is null) && promotionTarget != pieceMove.PromotesTo)
                {
                    continue;
                }

                if (!gamemode.ValidateMove(move, this))
                {
                    continue;
                }

                yield return move;
            }
        }
    }

    /// <summary>
    /// Translates move notation into a move instance.
    /// </summary>
    /// <param name="notation">The UCI or Algebraic notation describing the move.</param>
    /// <param name="teamColor">The team which move it is.</param>
    /// <param name="notationType">What notation the move is described in.</param>
    /// <returns>The move associated with the notation-string.</returns>
    public Move GetMoveByNotation(string notation, TeamColor teamColor, MoveNotation notationType) =>
        GetMovesByNotation(notation, teamColor, notationType).FirstOrDefault();

    /// <summary>
    /// Returns count of how many pieces with <c>teamColor</c> aiming on square.
    /// </summary>
    /// <param name="position">The tile position to lookup.</param>
    /// <param name="teamColor"></param>
    /// <returns></returns>
    public int IsDangerSquare(Coordinate position, TeamColor teamColor)
    {
        if (Dangerzone.TryGetValue(position, out List<Piece> pieces))
        {
            return pieces is null ? 0 : pieces.Count(p => p.Color == teamColor);
        }

        return 0;
    }

    /// <summary>
    /// Gets the sum of the pieces which can capture <c>position</c>, with black's pieces subtracted from white's pieces.
    /// </summary>
    /// <param name="position">The tile position to lookup.</param>
    /// <returns></returns>
    public int GetDangerSquareSum(Coordinate position)
    {
        if (!Dangerzone.TryGetValue(position, out List<Piece> pieces))
        {
            // No pieces can capture this tile.
            return 0;
        }

        int sum = 0;

        foreach (var item in pieces)
        {
            if (item.Color == TeamColor.White)
            {
                sum += 1;
            }
            else
            {
                sum -= 1;
            }
        }

        return sum;
    }

    /// <summary>
    /// Clears and refreshes dangerzone for all pieces.
    /// </summary>
    public void UpdateDangerzones()
    {
        Dangerzone.Clear();

        foreach (var piece in Pieces)
        {
            UpdateDangerzones(piece.Value);
        }
    }

    /// <summary>
    /// Gets the coordinate of a piece.
    /// </summary>
    /// <param name="piece"></param>
    /// <returns>The position of the piece on the chessboard.</returns>
    public Coordinate GetCoordinate(Piece piece) => Pieces.FirstOrDefault(p => p.Value == piece).Key;

    /// <summary>
    /// Returns position of a piece.
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool TryGetCoordinate(Piece piece, out Coordinate position)
    {
        try
        {
            position = Pieces.FirstOrDefault(x => x.Value == piece).Key;
            return true;
        }
        catch (ArgumentNullException)
        {
            position = default(Coordinate);
            return false;
        }
    }

    /// <summary>
    /// Gets a piece by position, (the same as using indexer).
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Piece GetPiece(Coordinate position)
    {
        if (Pieces.TryGetValue(position, out Piece piece))
            return piece;

        return null;
    }

    /// <summary>
    /// Gets all pieces by type.
    /// </summary>
    /// <typeparam name="T">The type of the pieces to retrieve.</typeparam>
    /// <returns>A list of pieces associated to thier position.</returns>
    public List<(Piece piece, Coordinate position)> GetPieces<T>()
        where T : Piece
        => (from piece in Pieces
            where piece.Value is T
            select (piece.Value, piece.Key)).ToList();

    public override bool Equals(object obj)
    {
        return Equals(obj as Chessboard);
    }

    public bool Equals(Chessboard other)
    {
        return other != null &&
               Height == other.Height &&
               Width == other.Width &&
               EqualityComparer<Dictionary<Coordinate, Piece>>.Default.Equals(Pieces, other.Pieces) &&
               EqualityComparer<Stack<Move>>.Default.Equals(Moves, other.Moves) &&
               EqualityComparer<HashSet<Piece>>.Default.Equals(MovedPieces, other.MovedPieces) &&
               EqualityComparer<Gamemode>.Default.Equals(gamemode, other.gamemode) &&
               CurrentState == other.CurrentState &&
               CurrentTeamTurn == other.CurrentTeamTurn &&
               MaterialSum == other.MaterialSum;
    }

    public override int GetHashCode()
    {
        int hashCode = 1077646461;
        hashCode = hashCode * -1521134295 + Height.GetHashCode();
        hashCode = hashCode * -1521134295 + Width.GetHashCode();

        foreach (var item in Pieces)
        {
            hashCode = hashCode * -1521134295 + item.Key.GetHashCode();
            hashCode = hashCode * -1521134295 + item.Value.GetHashCode();
        }

        foreach (var item in MovedPieces)
        {
            hashCode = hashCode * -1521134295 + item.GetHashCode();
        }

        hashCode = hashCode * -1521134295 + EqualityComparer<Gamemode>.Default.GetHashCode(gamemode);
        hashCode = hashCode * -1521134295 + CurrentState.GetHashCode();
        hashCode = hashCode * -1521134295 + CurrentTeamTurn.GetHashCode();
        hashCode = hashCode * -1521134295 + MaterialSum.GetHashCode();
        return hashCode;
    }

    /// <summary>
    /// Adds new dangerzone info of piecce.
    /// </summary>
    /// <param name="piece"></param>
    void UpdateDangerzones(Piece piece)
    {
        // Updates dangerzone
        foreach (var move in piece.GetMoves(this, true))
        {
            for (int i = 0; i < move.Submoves.Length; i++)
            {
                // If the move has no destination then it doesn't threaten any square.
                if (move.Submoves[i].Destination is null)
                {
                    continue;
                }

                Coordinate destination = move.Submoves[i].Destination.Value;

                // Make new list of pieces aiming on this square if there isn't one already.
                if (!Dangerzone.ContainsKey(destination))
                {
                    Dangerzone[destination] = new List<Piece>();
                }

                // Add this move to dangerzone.
                Dangerzone[destination].Add(move.Submoves[i].Piece);
            }
        }
    }
}