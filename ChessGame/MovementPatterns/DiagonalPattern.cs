using System;
using System.Collections.Generic;

namespace ChessGame.MovementPatterns
{
    /// <summary>
    /// The pattern the bishop uses, targets fields that are diagonal to current position.
    /// Can't jump over pieces, therefor only
    /// </summary>
    public class DiagonalPattern : IMovementPattern
    {
        IEnumerable<Move> IMovementPattern.GetMoves(Piece piece, Board board)
        {
            Coordinate position = piece.Position; //Keeps the piece position saved

            for (int i = -board.MaxFile; i < board.MaxFile; i++) //Checking the boards lenght both left and right
            {
                if (i == 0) //If this square
                    continue;

                Coordinate newPosition = new Coordinate(i + position.File, i + position.Rank); //Position update

                if (newPosition.Rank > board.MaxRank || newPosition.Rank < 0 ||
                    newPosition.File > board.MaxFile || newPosition.File < 0) //If the checking position is outside of the board
                    continue;

                // whether the position is occupied.
                Piece occupyingPiece = board.GetPiece(newPosition);

                if (occupyingPiece is null) // is position empty?
                {
                    yield return new Move(newPosition, piece);
                    continue;
                }
                else if (occupyingPiece.Color != piece.Color)
                {
                    yield return new Move(newPosition, piece, true);
                    continue;
                }
                continue;
            }
        }
    }
}
