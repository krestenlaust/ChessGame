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
        public abstract Chessboard GenerateBoard(Player playerWhite, Player playerBlack);

        /// <summary>
        /// Validates a move for a given position. Maybe merge with <c>MakeMove(Move)</c>
        /// </summary>
        /// <param name="move"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public virtual bool ValidateMove(Move move, Chessboard board)
        {
            // if move puts king in check — it's invalid.
            Chessboard boardSimulation = new Chessboard(board);
            boardSimulation.ExecuteMove(move);

            // king is in check, move is invalid
            if (boardSimulation.IsKingInCheck(board.CurrentTurn))
            {
                return false;
            }

            // move is valid
            return true;
        }
    }
}
