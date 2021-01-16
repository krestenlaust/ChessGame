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

            // check forward
            if (board.GetPiece(forwardPush) is Piece)
            {
                // piece in the way.
                yield break;
            }

            // move 1 tile forward
            yield return new Move(forwardPush, position, piece, false);

            forwardPush += new Coordinate(0, moveDirectionY);

            // check long forward
            if (board.GetPiece(forwardPush) is null && !piece.hasMoved)
            {
                // clear ahead.
                yield return new Move(forwardPush, position, piece, false);
            }
        }
    }
}
