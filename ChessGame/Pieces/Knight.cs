using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Pieces
{
    public class Knight : Piece
    {
        public Knight()
        {
            Notation = 'N';
            MovementPatternList = new IMovementPattern[] { new MovementPatterns.KnightPattern() };
        }
    }
}
