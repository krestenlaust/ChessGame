using System.Collections.Generic;

namespace ChessGame.MovementPatterns
{
    public class PawnPattern : IMovementPattern
    {
        public IEnumerable<Move> GetMoves(Piece piece, Coordinate position, Board board)
        {
            int moveDirectionY = piece.Color == TeamColor.White ? 1 : -1;

            // get potential flank capture positions.
            Coordinate leftAttack = position + new Coordinate(1, moveDirectionY);
            Coordinate rightAttack = position + new Coordinate(-1, moveDirectionY);

            // check left flank
            if (board.GetPiece(leftAttack) is Piece LeftAttackedPiece && LeftAttackedPiece.Color != piece.Color)
                yield return new Move(leftAttack, piece, true);

            // check right flank
            if (board.GetPiece(rightAttack) is Piece rightAttackedPiece && rightAttackedPiece.Color != piece.Color)
                yield return new Move(rightAttack, piece, true);

            Coordinate forwardPush = position + new Coordinate(0, moveDirectionY);

            // check forward
            if (board.GetPiece(forwardPush) is Piece)
            {
                // piece in the way.
                yield break;
            }

            // move 1 tile forward
            yield return new Move(forwardPush, piece, false);

            forwardPush += new Coordinate(0, moveDirectionY);

            // check long forward
            if (!(board.GetPiece(forwardPush) is Piece))
            {
                // clear ahead.
                yield return new Move(forwardPush, piece, false);
            }
        }
    }
}
