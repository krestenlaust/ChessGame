namespace ChessGame
{
    /// <summary>
    /// The player type is the base class either used or inherited from to perform moves.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// The name displayed in user-interfaces.
        /// </summary>
        public readonly string Nickname;

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="name">The nickname of the player.</param>
        public Player(string name)
        {
            Nickname = name;
        }

        /// <summary>
        /// Called when it's <c>this</c> player objects turn.
        /// </summary>
        /// <param name="board">The state of the board.</param>
        public virtual void TurnStarted(Chessboard board)
        {
        }

        /// <summary>
        /// Returns player nickname.
        /// </summary>
        /// <returns>Nickname of player.</returns>
        public override string ToString() => Nickname;
    }
}
