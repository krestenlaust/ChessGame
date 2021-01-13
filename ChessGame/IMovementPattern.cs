using System;
using System.Collections.Generic;

namespace ChessGame
{
    /// <summary>
    /// Describes a specific movement pattern of a piece.
    /// </summary>
    public interface IMovementPattern
    {
        /// <summary>
        /// Returns the moves a piece can make based on the <c>board</c>.
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="position"></param>
        /// <param name="board"></param>
        /// <param name="dangersquaresOnly">Return squares that are under threat (squares that are being aimed down).</param>
        /// <returns></returns>
        IEnumerable<Move> GetMoves(Piece piece, Coordinate position, Chessboard board, bool dangersquaresOnly=false);
        /// TODO: anyCaptureOnly should probably be turned into something that just makes a piece return all moves possible.
    }
}