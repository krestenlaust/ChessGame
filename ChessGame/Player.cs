using System;

namespace ChessGame
{
    public class Player
    {
        public readonly string Nickname;
        public int Wins;
        public int Losses;
        public int Draws;
        public event Action<Game> onTurnStarted;

        public Player(string name)
        {
            Nickname = name;
        }

        public void TurnStarted(Game game)
        {
            onTurnStarted?.Invoke(game);
        }
    }
}
