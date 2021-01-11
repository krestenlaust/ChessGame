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
        /// 
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="position"></param>
        /// <param name="board"></param>
        /// <param name="anyCaptureOnly">Only return capture-moves, capture no matter other pieces (even if there isn't a piece).</param>
        /// <returns></returns>
        IEnumerable<Move> GetMoves(Piece piece, Coordinate position, Chessboard board, bool anyCaptureOnly=false);
    }
}