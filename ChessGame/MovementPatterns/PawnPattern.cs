using System;
using System.Collections.Generic;

namespace ChessGame.MovementPatterns
{
    public class PawnPattern : IMovementPattern
    {
        public IEnumerable<Move> GetMoves(Piece piece, Board board)
        {
            int moveDirectionY = piece.Color == PieceColor.White ? 1 : -1;

            // store position
            Coordinate position = piece.Position;

            // get potential flank capture positions.
            Coordinate leftAttack = position + new Coordinate(1, moveDirectionY);
            Coordinate rightAttack = position + new Coordinate(-1, moveDirectionY);

            if (board.GetPiece(leftAttack) is Piece attackedPiece)
            {
                yield return new Move(leftAttack, piece, true);
            }
            if (!(board.GetPiece(rightAttack) is null))
            {
                yield return new Move(rightAttack, piece, true);
            }
            if (!(board.GetPiece()))
            {

            }
        }
    }
}
