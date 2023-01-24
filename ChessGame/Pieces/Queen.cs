namespace ChessGame.Pieces
{
    public class Queen : Piece
    {
        public Queen(TeamColor teamColor) : base('Q', 9, teamColor)
        {
            MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.CardinalPattern(),
                new MovementPatterns.DiagonalPattern()
            };
        }
    }
}
