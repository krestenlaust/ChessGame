using System.Text;

namespace ChessGame
{
    /// <summary>
    /// Describes a single piece's move.
    /// </summary>
    public struct PieceMove
    {
        public Coordinate Position;
        public Piece Piece;
        public bool Captures;

        public PieceMove(Coordinate position, Piece piece, bool captures)
        {
            Position = position;
            Piece = piece;
            Captures = captures;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Piece);

            if (Captures)
                sb.Append('x');

            sb.Append(Position);

            return sb.ToString();
        }
    }
}
