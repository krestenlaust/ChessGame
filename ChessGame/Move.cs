using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    /// <summary>
    /// Describes a complete set of moves, the average move only contains one <c>PieceMove</c>,
    /// castling e.g. contains two <c>PieceMove</c>s.
    /// </summary>
    public class Move : IEqualityComparer<Move>
    {
        /// <summary>
        /// Returns true if any of <c>Moves</c> captures.
        /// </summary>
        public bool Captures
        {
            get
            {
                foreach (var singleMove in this.Moves)
                {
                    if (singleMove.Captures)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public readonly PieceMove[] Moves;
        public readonly string CustomNotation;
        public readonly TeamColor Color;

        public Move(PieceMove[] moves, TeamColor color)
        {
            this.Moves = moves;
            this.Color = color;
        }

        public Move(PieceMove[] moves, string notation, TeamColor color)
        {
            this.Moves = moves;
            this.CustomNotation = notation;
            this.Color = color;
        }

        public Move(Coordinate position, Coordinate source, Piece piece, bool captures, TeamColor color)
        {
            this.Moves = new[]
            {
                new PieceMove(position, source, piece, captures)
            };
            this.Color = color;
        }

        public Move(Coordinate position, Coordinate source, Piece piece, bool captures, string notation, TeamColor color)
        {
            this.Moves = new[]
            {
                new PieceMove(position, source, piece, captures)
            };

            this.CustomNotation = notation;
            this.Color = color;
        }

        public static bool operator ==(Move a, Move b) => a.Equals(b);

        public static bool operator !=(Move a, Move b) => !a.Equals(b);

        public string ToString(MoveNotation notationType)
        {
            switch (notationType)
            {
                case MoveNotation.UCI:
                    return this.Moves[0].ToString(MoveNotation.UCI);
                case MoveNotation.StandardAlgebraic:
                    return this.ToString();
                default:
                    break;
            }

            return "";
        }

        public override string ToString()
        {
            // Return custom notation if it's defined.
            if (!(this.CustomNotation is null))
            {
                return this.CustomNotation;
            }

            StringBuilder sb = new StringBuilder();

            foreach (var singleMove in this.Moves)
            {
                sb.Append(',');
                sb.Append(singleMove.ToString());
            }

            return sb.ToString().Substring(1);
        }

        public override bool Equals(object obj)
        {
            return obj is Move move &&
                   this.ToString() == move.ToString() &&
                   this.CustomNotation == move.CustomNotation &&
                   this.Color == move.Color;
        }

        public override int GetHashCode()
        {
            int hashCode = -895752563;
            hashCode = hashCode * -1521134295 + this.ToString().GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.CustomNotation);
            hashCode = hashCode * -1521134295 + this.Color.GetHashCode();
            return hashCode;
        }

        public bool Equals(Move x, Move y) => x.Equals(y);

        public int GetHashCode(Move obj) => obj.GetHashCode();
    }
}
