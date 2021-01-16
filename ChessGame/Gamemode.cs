using System;
using System.Collections.Generic;
using System.Linq;
using ChessGame.Pieces;

namespace ChessGame
{
    public abstract class Gamemode
    {
        //public readonly Chessboard Board;
        
        //private readonly Player playerBlack;
        //private readonly Player playerWhite;

        public Gamemode()
        {
        }

        // TODO: maybe implement classic chess by standard (make this virtual).
        protected abstract Chessboard GenerateBoard();

        /// <summary>
        /// Validates a move for a given position. Maybe merge with <c>MakeMove(Move)</c>
        /// </summary>
        /// <param name="move"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public virtual bool ValidateMove(Move move, Chessboard board)
        {
            if (!board.IsKingInCheck(board.CurrentTurn))
            {
                return true;
            }

            TeamColor oppositeColor = board.CurrentTurn == TeamColor.Black ? TeamColor.White : TeamColor.Black;


            bool possiblyValidMove = false;

            foreach (var pieceMove in move.Moves)
            {
                // When the king is put in check, one (or more) of the following conditions
                // have to be true for the move to not immedietly be invalid.
                // Condition 1: A friendly piece is moved into dangerzone.
                // Condition 2: An enemy piece, that is listed on the dangersquare the king stands on, is captured.
                // Condition 3: The king is moved to a non-dangerzone tile.

                bool destinationDangerzone = board.IsDangerSquare(pieceMove.Destination, oppositeColor) > 0;

                if (!(destinationDangerzone || // Condition 1 & 2
                    (pieceMove.Piece is King && !destinationDangerzone))) // condition 3
                {
                    // non of the conditions apply, ignore move.
                    continue;
                }

                possiblyValidMove = true;
            }

            // TODO: validate moves with simulations
            if (!possiblyValidMove)
            {
                return false;
            }

            Chessboard boardSimulation = new Chessboard(board);
            boardSimulation.DoMove(move);

            // king is still in check, move is invalid
            if (boardSimulation.IsKingInCheck(board.CurrentTurn))
            {
                return false;
            }

            // move is valid
            return true;
        }
    }
}
