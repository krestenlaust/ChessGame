using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.MovementPatterns
{
    class EnPassentPattern : IMovementPattern
    {
        public IEnumerable<Move> GetMoves(Piece piece, Coordinate position, Chessboard board, bool captureOnly = false)
        {

        }
    }
}
