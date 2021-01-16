using System.Collections.Generic;

namespace ChessGame.MovementPatterns
{
    class EnPassentPattern : IMovementPattern
    {
        public IEnumerable<Move> GetMoves(Piece piece, Coordinate position, Chessboard board, bool guardedSquaresOnly = false)
        {
            if (guardedSquaresOnly || board.Moves.Count == 0)
            {
                yield break;
            }

            int moveDirectionY = piece.Color == TeamColor.White ? 1 : -1;

            Move previousMove = board.Moves.Peek();
            if (!(previousMove.Moves[0].Piece is Pieces.Pawn))
            {
                // en passent not possible, wasnt previous move.
                yield break;
            }

            Coordinate previousMoveDestination = previousMove.Moves[0].Destination;
            Coordinate previousMoveSource = previousMove.Moves[0].Source;

            // if pawn didn't make long jump, then break.
            if (previousMoveSource != new Coordinate(0, moveDirectionY * 2) + previousMoveDestination)
            {
                yield break;
            }


            Coordinate leftEnPassent = position + new Coordinate(-1, moveDirectionY);

            // should be right next to
            Coordinate enPassentTargetLeft = position + new Coordinate(-1, 0);
            if (enPassentTargetLeft == previousMoveDestination)
            {
                // capture with e.p.
                yield return new Move(leftEnPassent, position, piece, true);
                yield break;
            }
            
            Coordinate rightEnPassent = position + new Coordinate(1, moveDirectionY);

            Coordinate enPassentTargetRight = position + new Coordinate(1, 0);
            if (enPassentTargetRight == previousMoveDestination)
            {
                yield return new Move(rightEnPassent, position, piece, true);
            }

        }
    }
}
