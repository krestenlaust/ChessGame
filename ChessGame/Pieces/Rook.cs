namespace ChessGame.Pieces
{
    public class Rook : Piece
    {
        public Rook(TeamColor teamColor) : base('R', 5, teamColor)
        {
            this.MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.CardinalPattern()
            };
        }
    }
}
