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
                if (this.PromotePiece is null)
                {
                    return '\0';
                }
                else
                {
                    return this.PromotePiece.Notation;
                }
            }
        }

        public PieceMove(Coordinate? destination, Coordinate? source, Piece piece, bool captures, Piece promotePiece = null)
        {
            this.Destination = destination;
            this.Source = source;
            this.Piece = piece;
            this.Captures = captures;
            this.PromotePiece = promotePiece;
        }

        [DebuggerStepThrough]
        public bool Equals(PieceMove other)
        {
            if (this.Piece != other.Piece)
            {
                return false;
            }

            if (this.Captures != other.Captures)
            {
                return false;
            }

            if (this.Destination != other.Destination)
            {
                return false;
            }

            if (this.Source != other.Source)
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

            sb.Append(this.Piece);

            if (this.Captures)
                sb.Append('x');

            sb.Append(this.Destination);

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
                    if (this.PromotePiece is null)
                    {
                        return this.Source.ToString() + this.Destination.ToString();
                    }
                    else
                    {
                        return this.Source.ToString() + this.Destination.ToString() + this.PromotePiece.Notation;
                    }
                case MoveNotation.StandardAlgebraic:
                    return this.ToString();
                default:
                    break;
            }

            return string.Empty;
        }
    }
}
