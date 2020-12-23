using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public IEnumerator<Move> GetMoves()
        {
            foreach (var item in MovementPatterns)
            {
                foreach (var move in item)
                {

                }
            }
        }

        public override string ToString()
        {
            return Notation + "";
        }
    }
}
