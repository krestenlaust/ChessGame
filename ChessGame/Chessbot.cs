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

        protected abstract Move GenerateMove(Chessboard board);
    }
}
