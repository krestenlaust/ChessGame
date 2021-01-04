using System.Collections.Generic;
using System.Linq;

namespace ChessGame
{
    public class Board
    {
        public int MaxRank;
        public int MaxFile;
        public List<Piece> Pieces = new List<Piece>();

        public Board(int width, int height)
        {
            MaxFile = width - 1;
            MaxRank = height - 1;
        }

        /// <summary>
        /// Gets piece by position, or places piece at position (and changes the piece's position to position).
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Piece this[Coordinate position]
        {
            get { return GetPiece(position); }
            set
            {
                if (value is Piece newPiece)
                {
                    newPiece.Position = position;
                }

                if (GetPiece(position) is Piece oldPiece)
                {
                    Pieces.Remove(oldPiece);
                }
            }
        }

        public Piece GetPiece(Coordinate position)
        {
            return Pieces.FirstOrDefault(x => x.Position == position);
        }
    }
}