using System.Collections.Generic;

namespace ChessGame.MovementPatterns
{
    /// <summary>
    /// The pattern the rook uses, targets fields that are cardinal to current position.
    /// Can't jump over pieces, therefor only
    /// </summary>
    public class CardinalPattern : IMovementPattern
    {
        public IEnumerable<Move> GetMoves(Piece piece, Coordinate position, Chessboard board, bool guardedSquaresOnly = false)
        {
            for (int n = 0; n < 4; n++) // The 4 directions from the piece
            {
                int Xdir = 1;
                int Ydir = 1;
                switch (n) // Sets a direction for the checker
                {
                    case 0:
                        Xdir = 1;
                        Ydir = 0;
                        break;
                    case 1:
                        Xdir = -1;
                        Ydir = 0;
                        break;
                    case 2:
                        Xdir = 0;
                        Ydir = 1;
                        break;
                    case 3:
                        Xdir = 0;
                        Ydir = -1;
                        break;
                    default:
                        break;
                }

                for (int i = 1; i < board.Height * board.Width; i++) // Checker
                {
                    // Position update
                    Coordinate checkPosition = new Coordinate((i * Xdir) + position.File, (i * Ydir) + position.Rank);

                    // If the checking position is outside of the board
                    if (checkPosition.Rank >= board.Height || checkPosition.Rank < 0 ||
                        checkPosition.File >= board.Width || checkPosition.File < 0)
                        break;

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
                    if (occupyingPiece.Color != piece.Color || guardedSquaresOnly)
                    {
                        yield return new Move(checkPosition, position, piece, true, piece.Color); // Sends the move 
                    }

                    break;
                }
            }
        }
    }
}