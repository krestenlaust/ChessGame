using System.Linq;

namespace ChessGame
{
    public class Board
    {
        public int Width;
        public int Height;
        private Piece[] pieces;

        public Board(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public Piece GetPiece(Coordinate position)
        {
            return pieces.FirstOrDefault(x => x.Position == position);
        }
    }
}