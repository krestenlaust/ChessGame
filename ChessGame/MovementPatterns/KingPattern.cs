using System.Collections.Generic;

namespace ChessGame.MovementPatterns
{
    public class KingPattern : IMovementPattern
    {
        public IEnumerable<Move> GetMoves(Piece piece, Coordinate position, Chessboard board, bool guardedSquaresOnly = false)
        {
            // adjacent cells
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    // ignore own position
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    Coordinate newPosition = new Coordinate(x + position.File, y + position.Rank);

                    // if the checking position is outside of the board - ignore it.
                    if (!board.InsideBoard(newPosition))
                    {
                        continue;
                    }

                    // moved to before check dangersquare to possibly prevent weird king movement.
                    if (guardedSquaresOnly) // return all protected squares
                    {
                        yield return new Move(newPosition, position, piece, false, piece.Color);
                        continue;
                    }

                    TeamColor oppositeColor = piece.Color == TeamColor.Black ? TeamColor.White : TeamColor.Black;

                    // The king would put itself in check.
                    if (board.IsDangerSquare(newPosition, oppositeColor) > 0)
                    {
                        continue;
                    }

                    // whether the position is occupied.
                    Piece occupyingPiece = board.GetPiece(newPosition);

                    // is position empty?
                    if (occupyingPiece is null)
                    {
                        yield return new Move(newPosition, position, piece, false, piece.Color);
                        continue;
                    }

                    // is piece of opposite color (can capture)
                    if (occupyingPiece.Color != piece.Color)
                    {
                        yield return new Move(newPosition, position, piece, true, piece.Color);
                    }
                }
            }
        }
    }
}
