using System.Collections.Generic;

namespace ChessGame.MovementPatterns
{
    public class HypnoJumpPattern : IMovementPattern
    {
        public IEnumerable<Move> GetMoves(Piece piece, Coordinate position, Chessboard board, bool guardedSquaresOnly = false)
        {
            Coordinate targetPosition = position + new Coordinate(0, 3);

            if (!board.InsideBoard(targetPosition))
            {
                yield break;
            }

            Piece occupyingPiece = board.GetPiece(targetPosition);
            if (occupyingPiece is null)
            {
                yield return new Move(new PieceMove[]
                {
                    new PieceMove(targetPosition, position, piece, false),
                }, piece.Color);
            }
            else if (occupyingPiece.Color != piece.Color)
            {
                yield return new Move(new PieceMove[]
                {
                    new PieceMove(targetPosition, position, piece, true),
                    new PieceMove(position, targetPosition, occupyingPiece, false),
                }, piece.Color);
            }
        }
    }
}
