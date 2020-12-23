using System.Linq;

namespace ChessGame
{
    public class Board
    {
        public int MaxRank;
        public int MaxFile;
        private Piece[] pieces;

        public Board(int width, int height)
        {
            MaxFile = width;
            MaxRank = height;
        }

        public Piece GetPiece(Coordinate position)
        {
            return pieces.FirstOrDefault(x => x.Position == position);
        }
    }
}