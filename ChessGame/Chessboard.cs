using System.Collections.Generic;
using System.Linq;

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

        public Chessboard(int width, int height)
        {
            Width = width;
            Height = height;
            Pieces = new Dictionary<Coordinate, Piece>();
            Dangerzone = new Dictionary<Coordinate, List<Piece>>();
        }

        public void DoMove(Move move)
        {
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
                catch (System.InvalidOperationException)
                {
                    // Piece doesn't exist already, doesn't matter.
                }

                Pieces[singleMove.Destination] = singleMove.Piece;

                singleMove.Piece.hasMoved = true;
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
        /// Gets piece by position, or places piece at position (and changes the piece's position to position).
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Piece this[Coordinate position]
        {
            get { return GetPiece(position); }
            set { Pieces[position] = value; }
        }

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
    }
}