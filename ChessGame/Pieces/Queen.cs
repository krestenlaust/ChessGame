namespace ChessGame.Pieces
{
    public class Queen : Piece
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Queen"/> class.
        /// </summary>
        /// <param name="teamColor"></param>
        public Queen(TeamColor teamColor)
            : base('Q', 9, teamColor)
        {
            MovementPatternList = new IMovementPattern[]
            {
                new MovementPatterns.CardinalPattern(),
                new MovementPatterns.DiagonalPattern(),
            };
        }
    }
}
