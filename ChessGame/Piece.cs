using System.Collections.Generic;

namespace ChessGame
{
    /// <summary>
    /// The color of a piece.
    /// </summary>
    public enum TeamColor : byte
    {
        Black = 0,
        White = 1
    }

    public abstract class Piece
    {
        /// <summary>
        /// The character used to notate the piece in algebraic notation.
        /// </summary>
        public readonly char Notation; // { get; protected set; }
        /// <summary>
        /// Whether a piece is White or Black.
        /// </summary>
        public readonly TeamColor Color;

        /// <summary>
        /// The different movement patterns the piece uses.
        /// </summary>
        public readonly byte MaterialValue; // { get; protected set; }
        protected IMovementPattern[] MovementPatternList;

        protected Piece(char notation, byte materialValue, TeamColor color)
        {
            this.MaterialValue = materialValue;
            this.Notation = notation;
            this.Color = color;
        }

        /// <summary>
        /// Returns enumerable of all available moves of a given piece.
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public IEnumerable<Move> GetMoves(Chessboard board, bool guardedSquaresOnly = false)
        {
            if (this.MovementPatternList is null)
            {
                yield break;
            }

            if (!board.TryGetCoordinate(this, out Coordinate position))
            {
                yield break;
            }

            foreach (var item in this.MovementPatternList)
            {
                foreach (var move in item.GetMoves(this, position, board, guardedSquaresOnly))
                {
                    yield return move;
                }
            }
        }

        public override string ToString() => this.Notation.ToString();

        public override bool Equals(object obj)
        {
            return obj is Piece piece &&
                   this.Notation == piece.Notation &&
                   this.Color == piece.Color &&
                   this.MaterialValue == piece.MaterialValue;
        }

        public override int GetHashCode()
        {
            int hashCode = -1866919884;
            hashCode = hashCode * -1521134295 + this.Notation.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Color.GetHashCode();
            hashCode = hashCode * -1521134295 + this.MaterialValue.GetHashCode();
            return hashCode;
        }
    }
}
