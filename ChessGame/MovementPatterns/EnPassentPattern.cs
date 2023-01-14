using System.Collections.Generic;

namespace ChessGame.MovementPatterns
{
    public class EnPassentPattern : IMovementPattern
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

            Coordinate previousMoveDestination = previousMove.Moves[0].Destination.Value;
            Coordinate previousMoveSource = previousMove.Moves[0].Source.Value;

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
                Piece capturedPiece = board.GetPiece(enPassentTargetLeft);

                if (capturedPiece is null)
                {

                }
                else
                {
                    // capture with e.p.
                    yield return new Move(new PieceMove[]
                    {
                        new PieceMove(leftEnPassent, position, piece, true),
                        new PieceMove(null, enPassentTargetLeft, capturedPiece, false)
                    }, piece.Color);
                }
                yield break;
            }

            Coordinate rightEnPassent = position + new Coordinate(1, moveDirectionY);

            Coordinate enPassentTargetRight = position + new Coordinate(1, 0);
            if (enPassentTargetRight == previousMoveDestination)
            {
                Piece capturedPiece = board.GetPiece(enPassentTargetRight);

                if (capturedPiece is null)
                {

                }
                else
                {
                    // capture with e.p.
                    yield return new Move(new PieceMove[]
                    {
                        new PieceMove(rightEnPassent, position, piece, true),
                        new PieceMove(null, enPassentTargetRight, capturedPiece, false)
                    }, piece.Color);
                }
            }

        }
    }
}
