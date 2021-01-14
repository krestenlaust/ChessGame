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

        protected void TurnStart(Game game)
        {
            game.MakeMove(
                GenerateMove(game.Board, game.CurrentTurn)
                );
        }

        protected virtual Move GenerateMove(Chessboard board, TeamColor teamColor)
        {
            return null;
        }
    }
}
