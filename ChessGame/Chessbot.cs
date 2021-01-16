using System;

namespace ChessGame
{
    public abstract class Chessbot
    {
        public Player GeneratePlayer()
        {
            Player player = new Player("Chessbot");
            player.onTurnStarted += TurnStart;

            return player;
        }

        protected void TurnStart(Chessboard board)
        {
            board.MakeMove(
                GenerateMove(board)
                );
        }

        protected virtual Move GenerateMove(Chessboard board)
        {
            return null;
        }
    }
}
