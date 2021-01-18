using System;
using System.Diagnostics;
using System.Text;

namespace ChessGame
{
    public enum MoveNotation
    {
        UCI,
        StandardAlgebraic
    }

    /// <summary>
    /// Describes a single piece's move.
    /// </summary>
    public readonly struct PieceMove : IEquatable<PieceMove>
    {
        public readonly Coordinate Destination;
        public readonly Coordinate Source;
        public readonly Piece Piece;
        public readonly bool Captures;
        public readonly char PromotesTo;

        public PieceMove(Coordinate destination, Coordinate source, Piece piece, bool captures, char promotesTo = '\0')
        {
            Destination = destination;
            Source = source;
            Piece = piece;
            Captures = captures;
            PromotesTo = promotesTo;
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

        /// <summary>
        /// Returns the move expressed in Algebraic Notation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Piece);

            if (Captures)
                sb.Append('x');

            sb.Append(Destination);

            return sb.ToString();
        }

        /// <summary>
        /// Returns the move expressed in either Algebraic Notation or Universal Chess Interface-notation.
        /// </summary>
        /// <param name="notation"></param>
        /// <returns></returns>
        public string ToString(MoveNotation notation)
        {
            switch (notation)
            {
                case MoveNotation.UCI:
                    StringBuilder sb = new StringBuilder();

                    sb.Append(Source);
                    sb.Append(Destination);

                    if (PromotesTo != '\0')
                    {
                        sb.Append(PromotesTo);
                    }
                    break;
                case MoveNotation.StandardAlgebraic:
                    return ToString();
                default:
                    break;
            }

            return string.Empty;
        }
    }
}
