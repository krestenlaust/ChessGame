using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public interface IMovementPattern
    {
        IEnumerable<Move> GetMoves(Piece position, Board board);
    }
}
