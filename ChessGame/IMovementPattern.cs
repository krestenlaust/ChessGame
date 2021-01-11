using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    /// <summary>
    /// Describes a specific movement pattern of a piece.
    /// </summary>
    public interface IMovementPattern
    {
        IEnumerable<Move> GetMoves(Piece piece, Coordinate location, Board board, bool captureOnly=false);
    }
}