using System.Collections.Generic;

namespace ChessGame.MovementPatterns
{
    public class PawnPushPattern : IMovementPattern
    {
        public IEnumerable<Move> GetMoves(Piece piece, Coordinate position, Chessboard board, bool guardedSquaresOnly = false)
        {
            if (guardedSquaresOnly)
            {
                yield break;
            }

            int moveDirectionY = piece.Color == TeamColor.White ? 1 : -1;

            Coordinate forwardPush = position + new Coordinate(0, moveDirectionY);
            
            int promotionRank = piece.Color == TeamColor.White ? 6 : 1;

            // can't push pawn to eighth rank
            if (forwardPush.Rank == promotionRank)
            {
                yield break;
            }

            // check forward
            if (board.GetPiece(forwardPush) is Piece)
            {
                // piece in the way.
                yield break;
            }

            // move 1 tile forward
            yield return new Move(forwardPush, position, piece, false);

            forwardPush += new Coordinate(0, moveDirectionY);

            int longJumprank = piece.Color == TeamColor.White ? 1 : 6;

            // check long forward
            if (board.GetPiece(forwardPush) is null && position.Rank == longJumprank)
            {
                // clear ahead.
                yield return new Move(forwardPush, position, piece, false);
            }
        }
    }
}
