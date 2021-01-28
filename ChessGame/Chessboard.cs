using ChessGame.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessGame
{
    public enum MoveNotation : byte
    {
        UCI,
        StandardAlgebraic
    }

    /// <summary>
    /// A class that describes a game of chess.
    /// </summary>
    public class Chessboard : IEquatable<Chessboard>
    {
        public readonly byte Height;
        public readonly byte Width;
        public readonly Dictionary<Coordinate, Piece> Pieces;
        /// <summary>
        /// Describes intersection squares. An intersection square is a square which one or more pieces threaten at once.
        /// </summary>
        public readonly Dictionary<Coordinate, List<Piece>> Dangerzone;
        /// <summary>
        /// Previous moves, that resolve to this position.
        /// </summary>
        public readonly Stack<Move> Moves;
        /// <summary>
        /// A list of pieces, that have moved.
        /// </summary>
        public readonly HashSet<Piece> MovedPieces;
        private readonly Gamemode gamemode;

        public GameState CurrentState { get; internal set; }
        public TeamColor CurrentTeamTurn { get; set; }
        public Player CurrentPlayerTurn
        {
            get
            {
                return CurrentTeamTurn == TeamColor.White ? gamemode.PlayerWhite : gamemode.PlayerBlack;
            }
        }
        public int MaterialSum
        {
            get
            {
                return Pieces.Values.Sum(p => p.Color == TeamColor.Black ? -p.MaterialValue : p.MaterialValue);
            }
        }
        public bool isGameFinished
        {
            get
            {
                return CurrentState == GameState.Checkmate || 
                    CurrentState == GameState.Stalemate;
            }
        }

        /// <summary>
        /// Makes a copy of <c>board</c>, player references stay the same.
        /// </summary>
        /// <param name="board"></param>
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
        /// Instantiate board and simulate <c>move</c>.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="move"></param>
        public Chessboard(Chessboard board, Move move)
        {
            Height = board.Height;
            Width = board.Width;
            CurrentTeamTurn = board.CurrentTeamTurn;
            gamemode = board.gamemode;
            CurrentState = board.CurrentState;

            Pieces = new Dictionary<Coordinate, Piece>(board.Pieces);
            MovedPieces = new HashSet<Piece>(board.MovedPieces);
            Moves = new Stack<Move>(board.Moves);

            // is refreshed in simulatemove
            Dangerzone = new Dictionary<Coordinate, List<Piece>>();

            SimulateMove(move);
        }

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
        /// Gets piece by position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Piece this[Coordinate position]
        {
            get { return GetPiece(position); }
            set { Pieces[position] = value; }
        }

        /// <summary>
        /// Starts a game by calling <c>StartNextTurn</c>.
        /// </summary>
        public void StartGame()
        {
            CurrentState = GameState.Started;
            StartNextTurn();
        }

        /// <summary>
        /// Makes a move based on move notation. Calls <c>MakeMove(Move)</c> once move is translated.
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public bool PerformMove(string move, MoveNotation notationType)
        {
            Move pieceMove = GetMoveByNotation(move, CurrentTeamTurn, notationType);

            if (pieceMove is null)
                return false;

            return PerformMove(pieceMove);
        }

        /// <summary>
        /// Updates chessboard state based on move, adds the move to the move stack, and calls <c>StartNextTurn</c>.
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public bool PerformMove(Move move)
        {
            ExecuteMove(move);
            Moves.Push(move);

            CurrentTeamTurn = (TeamColor)(((int)CurrentTeamTurn + 1) % 2);

            StartNextTurn();

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
        /// Refreshes dangerzone, updates turn, checks for check.
        /// </summary>
        public void StartNextTurn()
        {
            // change turn
            if (gamemode.StartTurn(this))
            {
                CurrentPlayerTurn.TurnStarted(this);
            }
            else
            {
            }
        }

        /// <summary>
        /// Gets all legal moves for current team.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Move> GetMoves() => GetMoves(CurrentTeamTurn);

        public IEnumerable<Move> GetMovesSorted()
        {
            return (from move in GetMoves()
                    orderby move.Captures
                    select move);
        }

        /// <summary>
        /// Gets all legal moves for a team.
        /// </summary>
        /// <param name="teamColor"></param>
        /// <returns></returns>
        public IEnumerable<Move> GetMoves(TeamColor teamColor)
        {
            foreach (var piece in Pieces.Values.ToList())
            {
                if (piece.Color != teamColor)
                {
                    continue;
                }

                foreach (var move in piece.GetMoves(this))
                {
                    if (!gamemode.ValidateMove(move, this))
                    {
                        continue;
                    }

                    yield return move;
                }
            }
        }

        /// <summary>
        /// Checks if a given position is outside the board (and thereby invalid).
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

            foreach (var singleMove in move.Moves)
            {
                // remove piece
                if (singleMove.Destination is null)
                {
                    Pieces.Remove(singleMove.Source.Value);
                    return;
                }

                Coordinate destination = singleMove.Destination.Value;

                // remove previous instance
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

            foreach (var item in GetPieces<King>())
            {
                if (item.Color != color)
                {
                    continue;
                }

                king = item;
            }

            if (king is null)
            {
                return false;
            }

            Coordinate position = GetCoordinate(king);

            if (Dangerzone.TryGetValue(position, out List<Piece> pieces))
            {
                // returns true if any of the pieces are of a different color.
                return pieces.Any(p => p.Color != color);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Translates move notation into a move instance.
        /// </summary>
        /// <param name="notation"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public Move GetMoveByNotation(string notation, TeamColor player, MoveNotation notationType)
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

                            if (notation[lastNumberIndex - 2] == 'x')
                            {

                            }
                        }
                        break;
                }
            }
            catch (Exception)
            {
                // if notation parsing threw exception, then it's probably custom notation.
                customNotation = true;
            }

            foreach (var move in GetMoves(player))
            {
                if (player != move.Color)
                {
                    continue;
                }

                if (customNotation && move.CustomNotation == notation)
                {
                    if (gamemode.ValidateMove(move, this))
                    {
                        return move;
                    }

                    continue;
                }

                foreach (var pieceMove in move.Moves)
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

                    return move;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns count of how many pieces with <c>color</c> aiming on square.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public int IsDangerSquare(Coordinate position, TeamColor color)
        {
            if (Dangerzone.TryGetValue(position, out List<Piece> pieces))
            {
                return pieces is null ? 0 : pieces.Count(p => p.Color == color);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Removes old references of piece, and adds new.
        /// </summary>
        /// <param name="piece"></param>
        private void UpdateDangerzones(Piece piece)
        {
            // remove all instances
            foreach (var item in Dangerzone)
            {
                if (item.Value is null)
                {
                    continue;
                }

                item.Value.Remove(piece);
            }

            // update dangersquares.
            foreach (var move in piece.GetMoves(this, true))
            {
                foreach (var singleMove in move.Moves)
                {
                    if (singleMove.Destination is null)
                    {
                        continue;
                    }

                    if (!Dangerzone.ContainsKey(singleMove.Destination.Value))
                    {
                        Dangerzone[singleMove.Destination.Value] = new List<Piece>();
                    }

                    Dangerzone[singleMove.Destination.Value].Add(singleMove.Piece);
                }
            }
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
                position = new Coordinate();
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
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<Piece> GetPieces<T>() where T : Piece => (from piece in Pieces.Values
                                                              where piece is T
                                                              select piece).ToList();

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

        public static bool operator ==(Chessboard left, Chessboard right)
        {
            return EqualityComparer<Chessboard>.Default.Equals(left, right);
        }

        public static bool operator !=(Chessboard left, Chessboard right)
        {
            return !(left == right);
        }
    }
}