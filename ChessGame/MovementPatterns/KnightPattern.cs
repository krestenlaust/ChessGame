namespace ChessGame.MovementPatterns;

using System.Collections.Generic;

/// <summary>
/// Describes the moves the knight makes in chess.
/// </summary>
public class KnightPattern : IMovementPattern
{
    /// <inheritdoc/>
    public IEnumerable<Move> GetMoves(Piece piece, Coordinate position, Chessboard board, bool guardedSquaresOnly = false)
    {
        // The 4 directions from the piece.
        for (int n = 0; n < 4; n++)
        {
            // Sets a direction for the checker.
            int xDir = 1;
            int yDir = 1;
            switch (n)
            {
                case 0:
                    xDir = 2;
                    yDir = 1;
                    break;
                case 1:
                    xDir = 2;
                    yDir = -1;
                    break;
                case 2:
                    xDir = 1;
                    yDir = 2;
                    break;
                case 3:
                    xDir = -1;
                    yDir = 2;
                    break;
                default:
                    break;
            }

            for (int i = 0; i < 2; i++)
            {
                // First check
                Coordinate checkPosition = new Coordinate(xDir + position.File, yDir + position.Rank); // Position update

                // Second check
                if (i != 0)
                {
                    checkPosition = new Coordinate(-xDir + position.File, -yDir + position.Rank); // Position update
                }

                // If the checking position is outside of the board.
                if (checkPosition.Rank >= board.Height || checkPosition.Rank < 0 ||
                    checkPosition.File >= board.Width || checkPosition.File < 0)
                {
                    continue;
                }

                // whether the position is occupied.
                Piece occupyingPiece = board.GetPiece(checkPosition);

                // is position empty? or return danger squares?
                if (occupyingPiece is null || guardedSquaresOnly)
                {
                    yield return new Move(checkPosition, position, piece, false, piece.Color);
                    continue;
                }

                // There is a enemy piece.
                if (occupyingPiece.Color != piece.Color)
                {
                    // Sends the move.
                    yield return new Move(checkPosition, position, piece, true, piece.Color);
                }
            }
        }
    }
}