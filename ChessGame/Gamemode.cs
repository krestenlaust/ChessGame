using System;
using System.Collections.Generic;
using System.Linq;
using ChessGame.Pieces;

namespace ChessGame
{
    public enum GameState
    {
        Stalemate,
        Checkmate,
        Check
    }

    public abstract class Gamemode
    {
        public event Action<GameState> onGameStateUpdated;

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

        /// <summary>
        /// 
        /// </summary>
        public virtual void StartTurn(Chessboard board)
        {
            bool isKingChecked;

            // check for whether king is in check.
            if (board.IsKingInCheck(board.CurrentTurn))
            {
                isKingChecked = true;
                onGameStateUpdated?.Invoke(GameState.Check);
            }
            else
            {
                isKingChecked = false;
            }

            // no more legal moves, game is over either by stalemate or checkmate.
            if (!board.GetMoves(board.CurrentTurn).Any())
            {
                board.isGameInProgress = false;
                GameState gameState;

                // checkmate
                if (isKingChecked)
                {
                    board.Winner = board.CurrentTurn == TeamColor.White ? board.PlayerBlack : board.PlayerWhite;
                    gameState = GameState.Checkmate;
                }
                else
                {
                    gameState = GameState.Stalemate;
                }

                onGameStateUpdated?.Invoke(gameState);
                return;
            }
        }
    }
}
