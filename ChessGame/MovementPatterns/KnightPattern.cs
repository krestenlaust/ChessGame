using System.Collections.Generic;

namespace ChessGame.MovementPatterns
{
    public class KnightPattern : IMovementPattern
    {
        public IEnumerable<Move> GetMoves(Piece piece, Coordinate position, Chessboard board, bool guardedSquaresOnly = false)
        {
            for (int n = 0; n < 4; n++) //The 4 directions from the piece
            {
                int Xdir = 1;
                int Ydir = 1;
                switch (n) //Sets a direction for the checker
                {
                    case 0:
                        Xdir = 2;
                        Ydir = 1;
                        break;
                    case 1:
                        Xdir = 2;
                        Ydir = -1;
                        break;
                    case 2:
                        Xdir = 1;
                        Ydir = 2;
                        break;
                    case 3:
                        Xdir = -1;
                        Ydir = 2;
                        break;
                    default:
                        break;
                }

                for (int i = 0; i < 2; i++)
                {
                    //First check
                    Coordinate checkPosition = new Coordinate(Xdir + position.File, Ydir + position.Rank); //Position update

                    if (i != 0) //Second check
                    {
                        checkPosition = new Coordinate(-Xdir + position.File, -Ydir + position.Rank); //Position update
                    }

                    if (checkPosition.Rank >= board.Height || checkPosition.Rank < 0 ||
                        checkPosition.File >= board.Width || checkPosition.File < 0) //If the checking position is outside of the board
                        continue;

                    // whether the position is occupied.
                    Piece occupyingPiece = board.GetPiece(checkPosition);

                    if (occupyingPiece is null || guardedSquaresOnly) // is position empty? or return danger squares?
                    {
                        yield return new Move(checkPosition, position, piece, false, piece.Color);
                        continue;
                    }

                    if (occupyingPiece.Color != piece.Color) // There is a enemy piece
                    {
                        yield return new Move(checkPosition, position, piece, true, piece.Color); //Sends the move 
                    }
                }
            }
        }
    }
}