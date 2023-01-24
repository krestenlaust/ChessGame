namespace ChessGame.Pieces
{
    public class Rook : Piece
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rook"/> class.
        /// </summary>
        /// <param name="teamColor"></param>
        public Rook(TeamColor teamColor)
            : base('R', 5, teamColor)
        {
            MovementPatternList = new IMovementPattern[]
            {
                new MovementPatterns.CardinalPattern(),
            };
        }
    }
}
