using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.MovementPatterns
{
    public class PromotionPattern : IMovementPattern
    {
        public IEnumerable<Move> GetMoves(Piece piece, Coordinate position, Chessboard board, bool guardedSquaresOnly = false)
        {
            if (guardedSquaresOnly)
            {
                yield break;
            }

            // zero-index
            int promotionRank = piece.Color == TeamColor.White ? 6 : 1;

            if (position.Rank != promotionRank)
            {
                yield break;
            }


            throw new NotImplementedException();
        }
    }
}
