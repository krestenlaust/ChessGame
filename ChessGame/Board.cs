using System.Collections.Generic;
using System.Linq;

namespace ChessGame
{
    public class Board
    {
        public readonly int MaxRank;
        public readonly int MaxFile;
        public Dictionary<Coordinate, Piece> Pieces = new Dictionary<Coordinate, Piece>();
        /// <summary>
        /// Describes intersection squares.
        /// </summary>
        public Dictionary<Coordinate, List<Piece>> Dangerzone = new Dictionary<Coordinate, List<Piece>>();

        public Board(int width, int height)
        {
            MaxFile = width - 1;
            MaxRank = height - 1;
        }

        public void Move(Move move)
        {
            foreach (var singleMove in move.Moves)
            {
                if (singleMove.Captures)
                {
                    Pieces.Remove(singleMove.Destination);
                }

                Pieces.Remove(Pieces.First(piece => piece.Value == singleMove.Piece).Key);
                Pieces[singleMove.Destination] = singleMove.Piece;

                singleMove.Piece.hasMoved = true;
            }
        }

        public Move MoveByNotation(string notation, TeamColor player)
        {
            char pieceNotation;
            Coordinate destination;

            // pawn move
            if (char.IsDigit(notation[1]))
            {
                pieceNotation = '\0';
                destination = new Coordinate(notation);
            }
            else
            {
                pieceNotation = notation[0];
                destination = new Coordinate(notation.Substring(1));
            }

            foreach (var piece in Pieces)
            {
                if (piece.Value.Color != player)
                    continue;

                if (piece.Value.Notation != pieceNotation)
                    continue;

                foreach (var targetMove in piece.Value.GetMoves(this))
                {
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

        private void CalculateDangerzones()
        {
            foreach (var piece in Pieces)
            {
                foreach (var move in piece.Value.GetMoves(this))
                {
                    
                }
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

            }catch (System.ArgumentNullException)
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