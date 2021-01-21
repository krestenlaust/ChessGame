using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Pieces
{
    public class Hypnotist : Piece
    {
        public Hypnotist()
        {
            Notation = 'H';
            MaterialValue = 5;
            MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.HypnoJumpPattern()
            };
        }
    }
}
