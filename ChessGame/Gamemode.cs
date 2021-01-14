using System;
using System.Collections.Generic;

namespace ChessGame
{
    public abstract class Gamemode
    {
        public readonly Chessboard Board;
        public TeamColor CurrentTurn = TeamColor.Black; // changes on next turn start
        public Player CurrentPlayerTurn
        {
            get
            {
                return CurrentTurn == TeamColor.White ? playerWhite : playerBlack;
            }
        }
        private readonly Player playerBlack;
        private readonly Player playerWhite;
        private readonly Stack<Move> moves = new Stack<Move>();
        public event Action onGameOver;
        public event Action onKingChecked;

        public Gamemode(Player playerWhite, Player playerBlack)
        {
            this.playerWhite = playerWhite;
            this.playerBlack = playerBlack;
            Board = GenerateBoard();
        }

        protected virtual Chessboard GenerateBoard()
        {
            return new Chessboard();
        }

        /// <summary>
        /// Starts a game by calling <c>StartNextTurn</c>.
        /// </summary>
        public virtual void StartGame()
        {
            StartNextTurn();
        }

        /// <summary>
        /// Makes a move based on move notation. Calls <c>MakeMove(Move)</c> once move is translated.
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public bool MakeMove(string move)
        {
            Move pieceMove = Board.GetMoveByNotation(move, CurrentTurn);

            if (pieceMove is null)
                return false;

            return MakeMove(pieceMove);
        }

        /// <summary>
        /// Updates chessboard state based on move, adds the move to the move stack, and calls <c>StartNextTurn</c>.
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public virtual bool MakeMove(Move move)
        {
            // make the actual move change the chessboard state.
            Board.DoMove(move);
            // add the move to the list of moves.
            moves.Push(move);

            StartNextTurn();

            return true;
        }

        /// <summary>
        /// Refreshes dangerzone, updates turn, checks for check, and notifies event listeners of new turn.
        /// </summary>
        protected virtual void StartNextTurn()
        {
            // refresh dangersquares
            Board.UpdateDangerzones();

            // change turn
            CurrentTurn = CurrentTurn == TeamColor.Black ? TeamColor.White : TeamColor.Black;

            // check for whether king is in check.
            if (Board.IsKingInCheck(CurrentTurn))
            {
                // trigger event
                onKingChecked?.Invoke();
            }

            CurrentPlayerTurn.TurnStarted(this);
        }
    }
}
