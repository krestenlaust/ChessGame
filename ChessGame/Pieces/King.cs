namespace ChessGame.Pieces
{
    public class King : Piece
    {
        public King(TeamColor teamColor) : base('K', byte.MaxValue, teamColor)
        {
            this.MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.KingPattern(),
                new MovementPatterns.CastlePattern()
            };
        }
    }
}
