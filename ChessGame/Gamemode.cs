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
        public event Action onTurnChanged;

        public Player Winner;
        public readonly Player PlayerWhite;
        public readonly Player PlayerBlack;

        public Gamemode(Player playerWhite, Player playerBlack)
        {
            PlayerWhite = playerWhite;
            PlayerBlack = playerBlack;
        }

        // TODO: maybe implement classic chess by standard (make this virtual).
        public abstract Chessboard GenerateBoard();

        /// <summary>
        /// Validates a move for a given position. Maybe merge with <c>MakeMove(Move)</c>
        /// </summary>
        /// <param name="move"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public virtual bool ValidateMove(Move move, Chessboard board)
        {
            // if move is outside board, then it's invalid.
            foreach (var singleMove in move.Moves)
            {
                if (singleMove.Destination is null)
                {
                    continue;
                }

                if (!board.InsideBoard(singleMove.Destination.Value))
                {
                    return false;
                }
            }

            // if move puts king in check — it's invalid.
            Chessboard boardSimulation = new Chessboard(board);
            boardSimulation.PerformMove(move, false);

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
        /// <param name="board"></param>
        /// <returns>False if game has ended.</returns>
        public virtual bool StartTurn(Chessboard board)
        {
            onTurnChanged?.Invoke();

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
                    Winner = board.CurrentTurn == TeamColor.White ? PlayerBlack : PlayerWhite;
                    gameState = GameState.Checkmate;
                }
                else
                {
                    gameState = GameState.Stalemate;
                }

                onGameStateUpdated?.Invoke(gameState);
                return false;
            }

            return true;
        }
    }
}
