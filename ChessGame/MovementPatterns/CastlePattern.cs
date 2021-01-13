﻿using System.Collections.Generic;

namespace ChessGame.MovementPatterns
{
    /// <summary>
    /// Describes the castling pattern with any of the two rooks.
    /// </summary>
    public class CastlePattern : IMovementPattern
    {
        public IEnumerable<Move> GetMoves(Piece piece, Coordinate position, Chessboard board, bool dangersquaresOnly = false)
        {
            // has the piece, to castle, moved? Only return dangersquares?
            if (dangersquaresOnly || piece.hasMoved)
            {
                yield break;
            }

            bool blockedLeft = false;
            bool blockedRight = false;

            for (int i = 1; i <= 4; i++)
            {
                if (!blockedLeft)
                {
                    Coordinate checkPosition = new Coordinate(-i, 0) + position;
                    if (board.GetPiece(checkPosition) is Piece blockingPiece)
                    {
                        if (blockingPiece is Pieces.Rook && !blockingPiece.hasMoved)
                        {
                            yield return new Move(
                                new PieceMove[] {
                                    new PieceMove(position - new Coordinate(2, 0), position, piece, false),
                                    new PieceMove(position - new Coordinate(1, 0), checkPosition, blockingPiece, false)
                                }, "O-O-O");
                        }

                        blockedLeft = true;
                    }
                }

                if (!blockedRight)
                {
                    Coordinate checkPosition = new Coordinate(i, 0) + position;
                    if (board.GetPiece(checkPosition) is Piece blockingPiece)
                    {
                        if (blockingPiece is Pieces.Rook && !blockingPiece.hasMoved)
                        {
                            yield return new Move(
                                new PieceMove[] {
                                    new PieceMove(position + new Coordinate(2, 0), position, piece, false),
                                    new PieceMove(position + new Coordinate(1, 0), checkPosition, blockingPiece, false)
                                }, "O-O");
                        }

                        blockedRight = true;
                    }
                }
            }
        }
    }
}
