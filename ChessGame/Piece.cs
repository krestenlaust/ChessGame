using System.Collections.Generic;

namespace ChessGame
{
    /// <summary>
    /// The color of a piece.
    /// </summary>
    public enum PieceColor
    {
        Black,
        White
    }

    public class Piece
    {
        public Coordinate Position;
        public bool hasMoved;
        /// <summary>
        /// The character used to notate the piece in algebraic notation.
        /// </summary>
        public char Notation;
        public PieceColor Color;
        public IMovementPattern[] MovementPatterns;

        /// <summary>
        /// Returns enumerable of all available moves of a given piece.
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public IEnumerable<Move> GetMoves(Board board)
        {
            foreach (var item in MovementPatterns)
                foreach (var move in item.GetMoves(this, board))
                {
                    yield return move;
                }
        }

        public override string ToString()
        {
            return Notation + "";
        }
    }
}
