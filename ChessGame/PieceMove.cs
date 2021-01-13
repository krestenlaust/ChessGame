using System;
using System.Diagnostics;
using System.Text;

namespace ChessGame
{
    /// <summary>
    /// Describes a single piece's move.
    /// </summary>
    public readonly struct PieceMove : IEquatable<PieceMove>
    {
        public readonly Coordinate Destination;
        public readonly Coordinate Source;
        public readonly Piece Piece;
        public readonly bool Captures;

        public PieceMove(Coordinate destination, Coordinate source, Piece piece, bool captures)
        {
            Destination = destination;
            Source = source;
            Piece = piece;
            Captures = captures;
        }

        [DebuggerStepThrough]
        public bool Equals(PieceMove other)
        {
            if (Piece != other.Piece)
            {
                return false;
            }

            if (Captures != other.Captures)
            {
                return false;
            }

            if (Destination != other.Destination)
            {
                return false;
            }

            if (Source != other.Source)
            {
                return false;
            }

            return true;
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
