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
        /// <summary>
        /// If null, disappears out of the thin air.
        /// </summary>
        public readonly Coordinate? Destination;

        /// <summary>
        /// If null, appears out of the thing air.
        /// </summary>
        public readonly Coordinate? Source;
        public readonly bool Captures;
        public readonly Piece Piece;
        public readonly Piece PromotePiece;

        public char PromotesTo
        {
            get
            {
                if (PromotePiece is null)
                {
                    return '\0';
                }
                else
                {
                    return PromotePiece.Notation;
                }
            }
        }

        public PieceMove(Coordinate? destination, Coordinate? source, Piece piece, bool captures, Piece promotePiece = null)
        {
            Destination = destination;
            Source = source;
            Piece = piece;
            Captures = captures;
            PromotePiece = promotePiece;
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
                    if (PromotePiece is null)
                    {
                        return Source.ToString() + Destination.ToString();
                    }
                    else
                    {
                        return Source.ToString() + Destination.ToString() + PromotePiece.Notation;
                    }
                case MoveNotation.StandardAlgebraic:
                    return ToString();
                default:
                    break;
            }

            return string.Empty;
        }
    }
}
