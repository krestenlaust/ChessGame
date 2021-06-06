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
        /// <summary>
        /// A Player object that is equal to the player whose turn it is
        /// </summary>
        public Player CurrentPlayerTurn 
        {
            get
            {
                return this.CurrentTeamTurn == TeamColor.White ? this.gamemode.PlayerWhite : this.gamemode.PlayerBlack;
            }
        }
        /// <summary>
        /// A integearena to determine the material sum of the game
        /// </summary>
        public int MaterialSum 
        {
            get
            {
                return this.Pieces.Values.Sum(p => p.Color == TeamColor.Black ? -p.MaterialValue : p.MaterialValue);
            }
        }
        /// <summary>
        /// A boolean to tell whether or not the game has finished
        /// </summary>
        public bool isGameFinished
        {
            get
            {
                return this.CurrentState == GameState.Checkmate || 
                    this.CurrentState == GameState.Stalemate ||
                    this.CurrentState == GameState.DeadPosition;
            }
        }

        /// <summary>
        /// Makes a copy of <c>board</c>, player references stay the same.
        /// </summary>
        /// <param name="board"></param>
        public Chessboard(Chessboard board)
        {
            this.Height = board.Height;
            this.Width = board.Width;
            this.CurrentTeamTurn = board.CurrentTeamTurn;
            this.gamemode = board.gamemode;
            this.CurrentState = board.CurrentState;

            this.Pieces = new Dictionary<Coordinate, Piece>(board.Pieces);
            this.MovedPieces = new HashSet<Piece>(board.MovedPieces);
            this.Moves = new Stack<Move>(board.Moves);
            // not needed before executing move
            this.Dangerzone = new Dictionary<Coordinate, List<Piece>>();
        }

        /// <summary>
        /// Instantiate board and simulate <c>move</c>.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="move"></param>
        public Chessboard(Chessboard board, Move move)
        {
            this.Height = board.Height;
            this.Width = board.Width;
            this.CurrentTeamTurn = board.CurrentTeamTurn;
            this.gamemode = board.gamemode;
            this.CurrentState = board.CurrentState;

            this.Pieces = new Dictionary<Coordinate, Piece>(board.Pieces);
            this.MovedPieces = new HashSet<Piece>(board.MovedPieces);
            this.Moves = new Stack<Move>(board.Moves.Reverse());

            // is refreshed in simulatemove
            this.Dangerzone = new Dictionary<Coordinate, List<Piece>>();

            this.SimulateMove(move);
        }

        public Chessboard(byte width, byte height, Gamemode gamemode)
        {
            this.Width = width;
            this.Height = height;
            this.gamemode = gamemode;
            this.CurrentTeamTurn = TeamColor.White;
            this.CurrentState = GameState.NotStarted;

            this.Pieces = new Dictionary<Coordinate, Piece>();
            this.Dangerzone = new Dictionary<Coordinate, List<Piece>>();
            this.Moves = new Stack<Move>();
            this.MovedPieces = new HashSet<Piece>();
        }

        /// <summary>
        /// Gets piece by position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Piece this[Coordinate position]
        {
            get { return this.GetPiece(position); }
            set {
                if (value is null)
                {
                    this.Pieces.Remove(position);
                }
                else
                {
                    this.Pieces[position] = value;
                }
            }
        }

        /// <summary>
        /// Makes a move based on move notation. Calls <c>MakeMove(Move)</c> once move is translated.
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public bool PerformMove(string move, MoveNotation notationType, bool startNextTurn = true)
        {
            Move pieceMove = this.GetMoveByNotation(move, this.CurrentTeamTurn, notationType);

            if (pieceMove is null)
                return false;

            return this.PerformMove(pieceMove, startNextTurn);
        }

        /// <summary>
        /// Updates chessboard state based on move, adds the move to the move stack, and calls <c>StartNextTurn</c>.
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public bool PerformMove(Move move, bool startNextTurn = true)
        {
            this.ExecuteMove(move);
            this.Moves.Push(move);

            this.CurrentTeamTurn = (TeamColor)(((int)this.CurrentTeamTurn + 1) % 2);

            if (startNextTurn)
            {
                this.StartNextTurn();
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
            this.ExecuteMove(move);
            this.Moves.Push(move);

            this.CurrentTeamTurn = (TeamColor)(((int)this.CurrentTeamTurn + 1) % 2);

            this.gamemode.UpdateGameState(this);

            return true;
        }

        /// <summary>
        /// Starts game if it hasn't started yet. Refreshes dangerzone, updates turn, checks for check.
        /// </summary>
        public void StartNextTurn()
        {
            if (this.CurrentState == GameState.NotStarted)
            {
                this.CurrentState = GameState.Started;
            }

            // change turn
            if (this.gamemode.StartTurn(this))
            {
                this.CurrentPlayerTurn?.TurnStarted(this);
            }
        }

        /// <summary>
        /// Gets all legal moves for current team.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Move> GetMoves() => this.GetMoves(this.CurrentTeamTurn);

        public IEnumerable<Move> GetMovesSorted()
        {
            return from move in this.GetMoves()
                   orderby move.Captures
                   select move;
        }

        /// <summary>
        /// Gets all legal moves for a team.
        /// </summary>
        /// <param name="teamColor"></param>
        /// <returns></returns>
        public IEnumerable<Move> GetMoves(TeamColor teamColor)
        {
            foreach (var piece in this.Pieces.Values.ToList())
            {
                if (piece.Color != teamColor)
                {
                    continue;
                }

                foreach (var move in piece.GetMoves(this))
                {
                    if (!this.gamemode.ValidateMove(move, this))
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
            if (position.Rank >= this.Height || position.Rank < 0)
            {
                return false;
            }

            if (position.File >= this.Width || position.File < 0)
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
                    this.Pieces.Remove(singleMove.Source.Value);
                    return;
                }

                Coordinate destination = singleMove.Destination.Value;

                // remove previous instance
                if (!(singleMove.Source is null))
                {
                    this.Pieces.Remove(singleMove.Source.Value);
                }

                if (singleMove.Captures)
                {
                    try
                    {
                        this.Pieces.Remove(destination);
                    }
                    catch (ArgumentException)
                    {
                    }
                }

                if (singleMove.PromotePiece is null)
                {
                    this.Pieces[destination] = singleMove.Piece;
                    this.MovedPieces.Add(singleMove.Piece);
                }
                else
                {
                    this.Pieces[destination] = singleMove.PromotePiece;
                    this.MovedPieces.Remove(singleMove.Piece);
                    this.MovedPieces.Add(singleMove.PromotePiece);
                }
            }

            this.UpdateDangerzones();
        }

        /// <summary>
        /// Returns whether the king of a team is in check.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public bool IsKingInCheck(TeamColor color)
        {
            Piece king = null;
            Coordinate position = new Coordinate();

            // Get first king, with this color, and it's position.
            (king, position) = (from piece in this.GetPieces<King>()
                                where piece.Item1.Color == color
                                select piece).FirstOrDefault();

            // No king of this color was found, thus the king can't be in check.
            if (king is null)
            {
                return false;
            }

            // Get list of pieces aiming on the square the king sits on.
            if (this.Dangerzone.TryGetValue(position, out List<Piece> pieces))
            {
                // Returns true if any of the pieces are of opposite color.
                return pieces.Any(p => p.Color != color);
            }
            else
            {
                // No pieces of opposite color aiming on this square, king not in check.
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
            foreach (var move in this.GetMoves(player))
            {
                // If looking for move by custom notation, return it here.
                if (customNotation && move.CustomNotation == notation)
                {
                    if (this.gamemode.ValidateMove(move, this))
                    {
                        yield return move;
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

                    if (!this.gamemode.ValidateMove(move, this))
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
        /// <param name="notation"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public Move GetMoveByNotation(string notation, TeamColor player, MoveNotation notationType) => 
            this.GetMovesByNotation(notation, player, notationType).FirstOrDefault();

        /// <summary>
        /// Returns count of how many pieces with <c>color</c> aiming on square.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public int IsDangerSquare(Coordinate position, TeamColor color)
        {
            if (this.Dangerzone.TryGetValue(position, out List<Piece> pieces))
            {
                return pieces is null ? 0 : pieces.Count(p => p.Color == color);
            }
            
            return 0;
        }

        /// <summary>
        /// Returns sum of dangersquares.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public int GetDangerSquareSum(Coordinate position)
        {
            int sum = 0;
            if (this.Dangerzone.TryGetValue(position, out List<Piece> pieces))
            {
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
            }

            return sum;
        }

        /// <summary>
        /// Removes old references of piece, and adds new.
        /// </summary>
        /// <param name="piece"></param>
        private void UpdateDangerzones(Piece piece, bool removeOld = false)
        {
            // Remove all instances of this piece.
            if (removeOld)
            { 
                foreach (var item in this.Dangerzone)
                {
                    if (item.Value is null)
                    {
                        continue;
                    }

                    item.Value.Remove(piece);
                }
            }

            // Updates dangerzone
            foreach (var move in piece.GetMoves(this, true))
            {
                foreach (var singleMove in move.Moves)
                {
                    // If the move has no destination then it doesn't threaten any square.
                    if (singleMove.Destination is null)
                    {
                        continue;
                    }

                    Coordinate destination = singleMove.Destination.Value;

                    // Make new list of pieces aiming on this square if there isn't one already.
                    if (!this.Dangerzone.ContainsKey(destination))
                    {
                        this.Dangerzone[destination] = new List<Piece>();
                    }

                    // Add this move to dangerzone.
                    this.Dangerzone[destination].Add(singleMove.Piece);
                }
            }
        }

        /// <summary>
        /// Clears and refreshes dangerzone for all pieces.
        /// </summary>
        public void UpdateDangerzones()
        {
            this.Dangerzone.Clear();

            foreach (var piece in this.Pieces)
            {
                this.UpdateDangerzones(piece.Value);
            }
        }

        /// <summary>
        /// Gets the coordinate of a piece.
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public Coordinate GetCoordinate(Piece piece) => this.Pieces.FirstOrDefault(p => p.Value == piece).Key;

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
                position = this.Pieces.FirstOrDefault(x => x.Value == piece).Key;
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
            if (this.Pieces.TryGetValue(position, out Piece piece))
                return piece;

            return null;
        }

        /// <summary>
        /// Gets all pieces by type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<(Piece, Coordinate)> GetPieces<T>() where T : Piece => (from piece in this.Pieces
                                                              where piece.Value is T
                                                              select (piece.Value, piece.Key)).ToList();

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Chessboard);
        }

        public bool Equals(Chessboard other)
        {
            return other != null &&
                   this.Height == other.Height &&
                   this.Width == other.Width &&
                   EqualityComparer<Dictionary<Coordinate, Piece>>.Default.Equals(this.Pieces, other.Pieces) &&
                   EqualityComparer<Stack<Move>>.Default.Equals(this.Moves, other.Moves) &&
                   EqualityComparer<HashSet<Piece>>.Default.Equals(this.MovedPieces, other.MovedPieces) &&
                   EqualityComparer<Gamemode>.Default.Equals(this.gamemode, other.gamemode) &&
                   this.CurrentState == other.CurrentState &&
                   this.CurrentTeamTurn == other.CurrentTeamTurn &&
                   this.MaterialSum == other.MaterialSum;
        }

        public override int GetHashCode()
        {
            int hashCode = 1077646461;
            hashCode = hashCode * -1521134295 + this.Height.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Width.GetHashCode();
            foreach (var item in this.Pieces)
            {
                hashCode = hashCode * -1521134295 + item.Key.GetHashCode();
                hashCode = hashCode * -1521134295 + item.Value.GetHashCode();
            }
            foreach (var item in this.MovedPieces)
            {
                hashCode = hashCode * -1521134295 + item.GetHashCode();
            }
            hashCode = hashCode * -1521134295 + EqualityComparer<Gamemode>.Default.GetHashCode(this.gamemode);
            hashCode = hashCode * -1521134295 + this.CurrentState.GetHashCode();
            hashCode = hashCode * -1521134295 + this.CurrentTeamTurn.GetHashCode();
            hashCode = hashCode * -1521134295 + this.MaterialSum.GetHashCode();
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