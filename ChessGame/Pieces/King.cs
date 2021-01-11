using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Pieces
{
    public class King : Piece
    {
        public King()
        {
            Notation = 'K';
            MovementPatternList = new IMovementPattern[] { new MovementPatterns.KingPattern(), new MovementPatterns.CastlePattern() };
        }
    }
}
