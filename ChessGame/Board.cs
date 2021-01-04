using System.Collections.Generic;
using System.Linq;

namespace ChessGame
{
    public class Board
    {
        public int MaxRank;
        public int MaxFile;
        public Dictionary<Coordinate, Piece> Pieces = new Dictionary<Coordinate, Piece>();

        public Board(int width, int height)
        {
            MaxFile = width - 1;
            MaxRank = height - 1;
        }

        public Move MoveByNotation(string notation)
        {
            
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