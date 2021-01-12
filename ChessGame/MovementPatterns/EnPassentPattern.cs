using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.MovementPatterns
{
    class EnPassentPattern : IMovementPattern
    {
        public IEnumerable<Move> GetMoves(Piece piece, Coordinate position, Chessboard board, bool dangersquaresOnly = false)
        {
            if (dangersquaresOnly)
            {
                yield break;
            }

            int moveDirectionY = piece.Color == TeamColor.White ? 1 : -1;

            Coordinate leftEnPassent = position + new Coordinate(-1, moveDirectionY);
            Coordinate rightEnPassent = position + new Coordinate(1, moveDirectionY);

            // should be right next to
            Coordinate enPassentTargetLeft = position + new Coordinate(-1, 0);
            if (board.GetPiece(enPassentTargetLeft) is Pieces.Pawn target)
            {
                
            }
        }
    }
}
