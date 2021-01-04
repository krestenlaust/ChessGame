using System;
using System.Collections.Generic;

namespace ChessGame.MovementPatterns
{
    /// <summary>
    /// The pattern the bishop uses, targets fields that are diagonal to current position.
    /// Can't jump over pieces, therefor only
    /// </summary>
    public class DiagonalPattern : IMovementPattern
    {
        public Move[] GetMoves(Piece position, Board board)
        {
            throw new NotImplementedException();
        }

        IEnumerator<Move> IMovementPattern.GetMoves(Piece position, Board board)
        {
            throw new NotImplementedException();
        }
    }
}
