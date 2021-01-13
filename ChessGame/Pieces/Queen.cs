using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Pieces
{
    public class Queen : Piece
    {
        public Queen()
        {
            Notation = 'Q';
            MovementPatternList = new IMovementPattern[] { 
                new MovementPatterns.CardinalPattern(), 
                new MovementPatterns.DiagonalPattern() 
            };
        }
    }
}
