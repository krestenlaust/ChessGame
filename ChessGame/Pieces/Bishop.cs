namespace ChessGame.Pieces
{
    public class Bishop : Piece
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Bishop"/> class.
        /// </summary>
        /// <param name="teamColor"></param>
        public Bishop(TeamColor teamColor)
            : base('B', 3, teamColor)
        {
            MovementPatternList = new IMovementPattern[]
            {
                new MovementPatterns.DiagonalPattern(),
            };
        }
    }
}
