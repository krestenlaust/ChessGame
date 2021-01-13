using System.Collections.Generic;

namespace ChessGame.MovementPatterns
{
    public class PawnCapturePattern : IMovementPattern
    {
        public IEnumerable<Move> GetMoves(Piece piece, Coordinate position, Chessboard board, bool dangersquaresOnly = false)
        {
            int moveDirectionY = piece.Color == TeamColor.White ? 1 : -1;

            // get potential flank capture positions.
            Coordinate leftAttack = position + new Coordinate(1, moveDirectionY);
            Coordinate rightAttack = position + new Coordinate(-1, moveDirectionY);

            // check left flank
            if (board.GetPiece(leftAttack) is Piece LeftAttackedPiece && LeftAttackedPiece.Color != piece.Color || dangersquaresOnly)
            {
                yield return new Move(leftAttack, position, piece, true);
            }

            // check right flank
            if (board.GetPiece(rightAttack) is Piece rightAttackedPiece && rightAttackedPiece.Color != piece.Color || dangersquaresOnly)
            {
                yield return new Move(rightAttack, position, piece, true);
            }
        }
    }
}
