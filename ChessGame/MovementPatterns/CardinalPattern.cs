using System;
using System.Collections.Generic;

namespace ChessGame.MovementPatterns
{
    /// <summary>
    /// The pattern the rooks uses, targets fields that are cardinal to current position.
    /// Can't jump over pieces, therefor only
    /// </summary>
    public class CardinalPattern : IMovementPattern
    {
        IEnumerable<Move> IMovementPattern.GetMoves(Piece piece, Coordinate position, Board board)
        {
            for (int n = 0; n < 4; n++) //The 4 directions from the piece
            {
                int Xdir = 1;
                int Ydir = 1;
                switch (n) //Sets a direction for the checker
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

                for (int i = 1; i <= board.MaxFile; i++) //Checker
                {
                    Coordinate checkPosition = new Coordinate((i * Xdir) + position.File, (i * Ydir) + position.Rank); //Position update

                    if (checkPosition.Rank > board.MaxRank || checkPosition.Rank < 0 ||
                        checkPosition.File > board.MaxFile || checkPosition.File < 0) //If the checking position is outside of the board
                        continue;

                    // whether the position is occupied.
                    Piece occupyingPiece = board.GetPiece(checkPosition);

                    if (occupyingPiece is null) // is position empty?
                    {
                        yield return new Move(checkPosition, piece, false);
                        continue;
                    }

                    if (occupyingPiece.Color != piece.Color) // There is a enemy piece
                    {
                        yield return new Move(checkPosition, piece, true); //Sends the move 
                    }

                    break;
                }
            }
        }    
    }
}