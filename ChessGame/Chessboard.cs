using System;
using System.Collections.Generic;
using System.Linq;
using ChessGame.Pieces;

namespace ChessGame
{
    /// <summary>
    /// A class that describes a game of chess.
    /// </summary>
    public class Chessboard
    {
        public readonly int Height;
        public readonly int Width;
        public readonly Dictionary<Coordinate, Piece> Pieces;
        /// <summary>
        /// Describes intersection squares.
        /// </summary>
        public readonly Dictionary<Coordinate, List<Piece>> Dangerzone;
        public readonly Stack<Move> Moves;
        public readonly HashSet<Piece> MovedPieces;
        public readonly Player PlayerWhite;
        public readonly Player PlayerBlack;
        public bool isGameInProgress;
        public Player Winner;

        private readonly Gamemode gamemode;
        public TeamColor CurrentTurn { get; private set; } // changes on next turn start
        public Player CurrentPlayerTurn
        {
            get
            {
                return CurrentTurn == TeamColor.White ? PlayerWhite : PlayerBlack;
            }
        }
        public int MaterialSum
        {
            get
            {
                return Pieces.Values.Sum(p => p.Color == TeamColor.Black ? -p.MaterialValue : p.MaterialValue);
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
            CurrentTurn = board.CurrentTurn;
            PlayerBlack = board.PlayerBlack;
            PlayerWhite = board.PlayerWhite;
            gamemode = board.gamemode;
            
            Pieces = new Dictionary<Coordinate, Piece>(board.Pieces);
            Dangerzone = new Dictionary<Coordinate, List<Piece>>(board.Dangerzone);
            MovedPieces = new HashSet<Piece>(board.MovedPieces);
            Moves = new Stack<Move>(board.Moves);
        }

        public Chessboard(int width, int height, Gamemode gamemode, Player playerWhite, Player playerBlack)
        {
            Width = width;
            Height = height;
            this.gamemode = gamemode;
            PlayerWhite = playerWhite;
            PlayerBlack = playerBlack;
            CurrentTurn = TeamColor.Black;

            Pieces = new Dictionary<Coordinate, Piece>();
            Dangerzone = new Dictionary<Coordinate, List<Piece>>();
            Moves = new Stack<Move>();
            MovedPieces = new HashSet<Piece>();
        }

        /// <summary>
        /// Starts a game by calling <c>StartNextTurn</c>.
        /// </summary>
        public void StartGame()
        {
            isGameInProgress = true;
            StartNextTurn();
        }

        /// <summary>
        /// Makes a move based on move notation. Calls <c>MakeMove(Move)</c> once move is translated.
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public bool PerformMove(string move, MoveNotation notationType)
        {
            Move pieceMove = GetMoveByNotation(move, CurrentTurn, notationType);

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
            // make the actual move change the chessboard state.
            ExecuteMove(move, true);
            // add the move to the list of moves.
            Moves.Push(move);

            StartNextTurn();

            return true;
        }

        /// <summary>
        /// Refreshes dangerzone, updates turn, checks for check.
        /// </summary>
        public void StartNextTurn()
        {
            // refresh dangersquares
            //UpdateDangerzones();

            Player previousPlayer = CurrentPlayerTurn;

            // change turn
            CurrentTurn = CurrentTurn == TeamColor.Black ? TeamColor.White : TeamColor.Black;

            gamemode.StartTurn(this);

            CurrentPlayerTurn.TurnStarted(this);
        }

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
        /// Executes a move by updating the pieces accordingly.
        /// </summary>
        /// <param name="move"></param>
        public void ExecuteMove(Move move, bool updateDangerzone = false)
        {
            if (move is null)
            {
                return;
            }

            foreach (var singleMove in move.Moves)
            {
                if (singleMove.Captures)
                {
                    Pieces.Remove(singleMove.Destination);
                }

                if (TryGetCoordinate(singleMove.Piece, out Coordinate key))
                {
                    Pieces.Remove(key);
                }

                Pieces[singleMove.Destination] = singleMove.Piece;

                MovedPieces.Add(singleMove.Piece);
            }

            if (updateDangerzone)
            {
                UpdateDangerzones();
            }
        }

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

        // GetMoves
        /// <summary>
        /// Translates algebraic notation into move.
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
            }catch (Exception)
            {
                customNotation = true;
            }

            foreach (var move in GetMoves(CurrentTurn))
            {
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
                    if (!Dangerzone.ContainsKey(singleMove.Destination))
                    {
                        Dangerzone[singleMove.Destination] = new List<Piece>();
                    }

                    Dangerzone[singleMove.Destination].Add(singleMove.Piece);
                }
            }
        }

        public void UpdateDangerzones()
        {
            Dangerzone.Clear();

            foreach (var piece in Pieces)
            {
                UpdateDangerzones(piece.Value);
            }
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

        public Coordinate GetCoordinate(Piece piece) => Pieces.FirstOrDefault(p => p.Value == piece).Key;

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

        public Piece GetPiece(Coordinate position)
        {
            if (Pieces.TryGetValue(position, out Piece piece))
                return piece;

            return null;
        }

        public List<Piece> GetPieces<T>() => (from piece in Pieces.Values
                                         where piece is T
                                         select piece).ToList();
    }
}