namespace ChessGame
{
    public class Player
    {
        public readonly string Nickname;

        public Player(string name)
        {
            Nickname = name;
        }

        public virtual void TurnStarted(Chessboard board)
        {

        }

        public override string ToString() => Nickname;
    }
}
