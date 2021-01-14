using System;
using System.Collections.Generic;
using System.Linq;
using ChessGame.Pieces;

namespace ChessGame
{
    public readonly struct Chessboard
    {
        public readonly int Height;
        public readonly int Width;
        public readonly Dictionary<Coordinate, Piece> Pieces;
        /// <summary>
        /// Describes intersection squares.
        /// </summary>
        public readonly Dictionary<Coordinate, List<Piece>> Dangerzone;
        public int MaterialSum
        {
            get
            {
                return Pieces.Values.Sum(p => p.Color == TeamColor.Black ? -p.MaterialValue : p.MaterialValue);
            }
        }

        public Chessboard(Chessboard board)
        {
            Height = board.Height;
            Width = board.Width;
            Pieces = new Dictionary<Coordinate, Piece>(board.Pieces);
            Dangerzone = new Dictionary<Coordinate, List<Piece>>(board.Dangerzone);
        }

        public Chessboard(int width, int height)
        {
            Width = width;
            Height = height;
            Pieces = new Dictionary<Coordinate, Piece>();
            Dangerzone = new Dictionary<Coordinate, List<Piece>>();
        }

        public void DoMove(Move move)
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

                try
                {
                    Pieces.Remove(Pieces.First(piece => piece.Value == singleMove.Piece).Key);
                }
                catch (InvalidOperationException)
                {
                    // Piece doesn't exist already, doesn't matter.
                }

                Pieces[singleMove.Destination] = singleMove.Piece;

                singleMove.Piece.hasMoved = true;
            }
        }

        public bool IsKingInCheck(TeamColor color)
        {
            King king = null;

            
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
                throw new ChessExceptions.KingNotFoundChessException();
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
                    yield return move;
                }
            }
        }

        public Move GetMoveByNotation(string notation, TeamColor player)
        {
            char pieceNotation = ' ';
            string customNotation = null;
            Coordinate destination = new Coordinate();

            // pawn move
            if (char.IsDigit(notation[1]))
            {
                pieceNotation = '\0';
                destination = new Coordinate(notation);
            }
            else if (char.IsDigit(notation[2]))
            {
                pieceNotation = notation[0];
                destination = new Coordinate(notation.Substring(1));
            }
            else
            {
                customNotation = notation;
            }

            foreach (var piece in Pieces)
            {
                if (piece.Value.Color != player)
                    continue;

                if (piece.Value.Notation != pieceNotation && customNotation is null)
                    continue;

                foreach (var targetMove in piece.Value.GetMoves(this))
                {
                    if (customNotation == targetMove.ToString())
                    {
                        return targetMove;
                    }

                    foreach (var singleMove in targetMove.Moves)
                    {
                        if (singleMove.Piece != piece.Value)
                            continue;

                        if (singleMove.Destination != destination)
                            continue;

                        return targetMove;
                    }
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
            catch (System.ArgumentNullException)
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

        public List<T> GetPieces<T>() => Pieces.OfType<T>().ToList();
    }
}