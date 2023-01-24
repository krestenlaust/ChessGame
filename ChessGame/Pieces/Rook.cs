namespace ChessGame.Pieces
{
    public class Rook : Piece
    {
        public Rook(TeamColor teamColor) : base('R', 5, teamColor)
        {
            MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.CardinalPattern()
            };
        }
    }
}
