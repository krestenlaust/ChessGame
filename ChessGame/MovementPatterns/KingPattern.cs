using System.Collections.Generic;

namespace ChessGame.MovementPatterns
{
    public class KingPattern : IMovementPattern
    {
        public IEnumerable<Move> GetMoves(Piece piece, Coordinate position, Chessboard board, bool captureOnly = false)
        {
            // adjacent cells
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    Coordinate newPosition = new Coordinate(x + position.File, y + position.Rank);

                    if (newPosition.Rank > board.MaxRank || newPosition.Rank < 0 ||
                        newPosition.File > board.MaxFile || newPosition.File < 0) // if the checking position is outside of the board
                        continue;

                    // whether the position is occupied.
                    Piece occupyingPiece = board.GetPiece(newPosition);

                    if (occupyingPiece is null) // is position empty?
                    {
                        yield return new Move(newPosition, position, piece, false);
                        continue;
                    }

                    if (occupyingPiece.Color != piece.Color) // is piece of opposite color (can capture)
                    {
                        yield return new Move(newPosition, position, piece, true);
                    }
                }
            }

            if (piece.hasMoved) // king has moved, castling isn't possible.
            {
                yield break;
            }

            // castling - without making sure the king won't end up in check.

        }
    }
}
