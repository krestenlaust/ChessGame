namespace ChessGame.MovementPatterns;

using System.Collections.Generic;

/// <summary>
/// The pattern the bishop uses, targets fields that are diagonal to current position.
/// Can't jump over pieces, therefore only ???.
/// </summary>
public class DiagonalPattern : IMovementPattern
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
                    xDir = 1;
                    yDir = 1;
                    break;
                case 1:
                    xDir = 1;
                    yDir = -1;
                    break;
                case 2:
                    xDir = -1;
                    yDir = -1;
                    break;
                case 3:
                    xDir = -1;
                    yDir = 1;
                    break;
                default:
                    break;
            }

            // Checker
            for (int i = 1; i < board.Width; i++)
            {
                Coordinate checkPosition = new Coordinate((i * xDir) + position.File, (i * yDir) + position.Rank); // Position update

                if (checkPosition.Rank >= board.Height || checkPosition.Rank < 0 ||
                    checkPosition.File >= board.Width || checkPosition.File < 0) // If the checking position is outside of the board
                    continue;

                // whether the position is occupied.
                Piece occupyingPiece = board.GetPiece(checkPosition);

                // is position empty?
                if (occupyingPiece is null)
                {
                    yield return new Move(checkPosition, position, piece, false, piece.Color);
                    continue;
                }
                else if (guardedSquaresOnly) // if not empty
                {
                    yield return new Move(checkPosition, position, piece, false, piece.Color);
                    break;
                }

                // There is a enemy piece
                if (occupyingPiece.Color != piece.Color)
                {
                    // Sends the move.
                    yield return new Move(checkPosition, position, piece, true, piece.Color);
                }

                break;
            }
        }
    }
}