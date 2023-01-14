namespace ChessGame.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(TeamColor teamColor) : base('B', 3, teamColor)
        {
            this.MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.DiagonalPattern()
            };
        }
    }
}
