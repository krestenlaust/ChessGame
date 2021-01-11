using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Pieces
{
    public class Bishop : Piece
    {
        public Bishop()
        {
            Notation = 'B';
            MovementPatternList = new IMovementPattern[] { new MovementPatterns.DiagonalPattern() };
        }
    }
}
