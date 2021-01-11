using System.Text;

namespace ChessGame
{
    /// <summary>
    /// Describes a single piece's move.
    /// </summary>
    public struct PieceMove
    {
        public Coordinate Destination;
        public Coordinate Source;
        public Piece Piece;
        public bool Captures;

        public PieceMove(Coordinate destination, Coordinate source, Piece piece, bool captures)
        {
            Destination = destination;
            Source = source;
            Piece = piece;
            Captures = captures;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Piece);

            if (Captures)
                sb.Append('x');

            sb.Append(Destination);

            return sb.ToString();
        }
    }
}
