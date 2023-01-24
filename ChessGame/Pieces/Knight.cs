namespace ChessGame.Pieces
{
    public class Knight : Piece
    {
        public Knight(TeamColor teamColor) : base('N', 3, teamColor)
        {
            MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.KnightPattern()
            };
        }
    }
}
