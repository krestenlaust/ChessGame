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

            int moveDirectionY = piece.Color == TeamColor.White ? 1 : -1;

            // get potential flank capture positions.
            Coordinate leftAttack = position + new Coordinate(1, moveDirectionY);
            Coordinate rightAttack = position + new Coordinate(-1, moveDirectionY);

            // check left flank
            if (board.GetPiece(leftAttack) is Piece LeftAttackedPiece && LeftAttackedPiece.Color != piece.Color || guardedSquaresOnly)
            {
                yield return new Move(new PieceMove[] {
                    new PieceMove(leftAttack, position, piece, true, new Pieces.Queen() { Color = piece.Color })
                }, piece.Color);
                yield return new Move(new PieceMove[] {
                    new PieceMove(leftAttack, position, piece, true, new Pieces.Rook() { Color = piece.Color })
                }, piece.Color);
                yield return new Move(new PieceMove[] {
                    new PieceMove(leftAttack, position, piece, true, new Pieces.Knight() { Color = piece.Color })
                }, piece.Color);
                yield return new Move(new PieceMove[] {
                    new PieceMove(leftAttack, position, piece, true, new Pieces.Bishop() { Color = piece.Color })
                }, piece.Color);
            }

            // check right flank
            if (board.GetPiece(rightAttack) is Piece rightAttackedPiece && rightAttackedPiece.Color != piece.Color || guardedSquaresOnly)
            {
                yield return new Move(new PieceMove[] {
                    new PieceMove(rightAttack, position, piece, true, new Pieces.Queen() { Color = piece.Color })
                }, piece.Color);
                yield return new Move(new PieceMove[] {
                    new PieceMove(rightAttack, position, piece, true, new Pieces.Rook() { Color = piece.Color })
                }, piece.Color);
                yield return new Move(new PieceMove[] {
                    new PieceMove(rightAttack, position, piece, true, new Pieces.Knight() { Color = piece.Color })
                }, piece.Color);
                yield return new Move(new PieceMove[] {
                    new PieceMove(rightAttack, position, piece, true, new Pieces.Bishop() { Color = piece.Color })
                }, piece.Color);
            }

            Coordinate forwardPush = position + new Coordinate(0, moveDirectionY);

            // check forward
            if (board.GetPiece(forwardPush) is Piece)
            {
                // piece in the way.
                yield break;
            }

            yield return new Move(new PieceMove[] {
                    new PieceMove(forwardPush, position, piece, false, new Pieces.Queen() { Color = piece.Color })
                }, piece.Color);
            yield return new Move(new PieceMove[] {
                    new PieceMove(forwardPush, position, piece, false, new Pieces.Rook() { Color = piece.Color })
                }, piece.Color);
            yield return new Move(new PieceMove[] {
                    new PieceMove(forwardPush, position, piece, false, new Pieces.Knight() { Color = piece.Color })
                }, piece.Color);
            yield return new Move(new PieceMove[] {
                    new PieceMove(forwardPush, position, piece, false, new Pieces.Bishop() { Color = piece.Color })
                }, piece.Color);
        }
    }
}
