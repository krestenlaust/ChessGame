namespace ChessGame.Pieces
{
    /// <summary>
    /// A fairy chess piece we came up with.
    /// </summary>
    public class Hypnotist : Piece
    {
        public Hypnotist(TeamColor teamColor)
            : base('H', 5, teamColor)
        {
            MovementPatternList = new IMovementPattern[]
            {
                new MovementPatterns.HypnoJumpPattern(),
            };
        }
    }
}
