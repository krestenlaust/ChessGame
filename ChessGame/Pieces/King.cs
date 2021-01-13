namespace ChessGame.Pieces
{
    public class King : Piece
    {
        public King()
        {
            Notation = 'K';
            MaterialValue = 0;
            MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.KingPattern(),
                new MovementPatterns.CastlePattern()
            };
        }
    }
}
